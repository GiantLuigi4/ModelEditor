using tfc.wrappers.opentk;

using OpenTK.Graphics.OpenGL4;
using TestProgram.utils;
using TestProgram.config;
using System;
using OpenTK.Windowing.Common.Input;

namespace TestProgram {
    class Program {
        static void Main(string[] args) {
            Window window = new Window();

            bool isOpen = true;
            window.addCloseListener((e) => {
                isOpen = false;
            });

            Config config = new Config();
            if (checkExistence("config.json")) {
                config.load(fastJSON5.JSON5.ToDynamic(readFile("config.json")));
            }
            write("config.json", config.asJson());

            window.setSize(config.defaultSize.x, config.defaultSize.y);

            window.grabContext();
            /*VertexBuilder<float> builder = new VertexBuilder<float>(0);
            {
                builder.position(-1, -1, 0, 1).color(config.backgroundColor.red, config.backgroundColor.green, config.backgroundColor.blue, config.backgroundColor.alpha).texture(0, 0).endVertex();
                builder.position( 1, -1, 0, 1).color(config.backgroundColor.red, config.backgroundColor.green, config.backgroundColor.blue, config.backgroundColor.alpha).texture(1, 0).endVertex();
                builder.position( 1,  1, 0, 1).color(config.backgroundColor.red, config.backgroundColor.green, config.backgroundColor.blue, config.backgroundColor.alpha).texture(1, 1).endVertex();

                builder.position( 1,  1, 0, 1).color(config.backgroundColor.red, config.backgroundColor.green, config.backgroundColor.blue, config.backgroundColor.alpha).texture(1, 1).endVertex();
                builder.position(-1,  1, 0, 1).color(config.backgroundColor.red, config.backgroundColor.green, config.backgroundColor.blue, config.backgroundColor.alpha).texture(0, 1).endVertex();
                builder.position(-1, -1, 0, 1).color(config.backgroundColor.red, config.backgroundColor.green, config.backgroundColor.blue, config.backgroundColor.alpha).texture(0, 0).endVertex();
            }*/
            //VertexArrayObject vaoBackground;
            //VertexBufferObject vboBackground;
            //VertexBufferObject vboLeftPanel;
            ShaderProgram program;
            {
                writeIfNotExist("shader.vsh", "#version 330 core\nin vec4 position;\n in vec4 col;\n\nout vec4 color;\n\nvoid main() {\n\tgl_Position = position;\n\tcolor = col;\n}\n");
                writeIfNotExist("shader.fsh", "#version 330 core\n\nin vec4 color;\n\nout vec4 FragColor;\nvoid main() {\n\tFragColor = color;\n}\n");

                Shader frag = new Shader(ShaderType.FragmentShader, readFile("shader.fsh"));
                Shader vert = new Shader(ShaderType.VertexShader, readFile("shader.vsh"));
                program = new ShaderProgram(new Shader[] { frag, vert });
                program.link();
                program.getUniformLocation("projectionMatrix");
            }
            VertexArrayHelper background = generateVAO(1, config.secondaryBackgroundColor, program);
            VertexArrayHelper leftPanel = generateVAO(config.layout.leftPanelSize, config.backgroundColor, program);
            VertexArrayHelper rightPanel = generateVAO(config.layout.rightPanelSize, config.backgroundColor, program);

            window.releaseContext();

            window.setVisible(true);

            bool isScalingLeftPanel = false;
            bool isScalingRightPanel = false;

            while (isOpen) {
                window.grabContext();

                bool defaultCursor = true;

                float scaledMX = window.getMouseX() / window.getWidth();
                float scaledMY = window.getMouseY() / window.getHeight();
                float paddingX = 4 / (float) window.getWidth();

                GL.Viewport(0, 0, window.getWidth(), window.getHeight());

                GL.ClearColor(0, 0.5f, 0, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                program.start();

                Matrix4 matrix = Matrix4.identity();
                program.uniformMatrix4("projectionMatrix", matrix.toArray());

                background.draw(PrimitiveType.Triangles);

                matrix = matrix.multiply(Matrix4.createTranslationMatrix(new Vector4(window.getWidth() / -100f + 1, 0, 0, 1)));
//                matrix = matrix.multiply(Matrix4.createTranslationMatrix(new Vector4(1, 0, 0, 1)));
                Matrix4 scaledMatrix = matrix.multiply(Matrix4.createScaleMatrix(new Vector4(1f / window.getWidth() * 100, 1, 1, 1)));

                Vector4 leftPanelX = scaledMatrix.transform(new Vector4(config.layout.leftPanelSize - 1, 0, 1, 1));
                leftPanelX.x += 1;

                if (leftPanelX.x - paddingX <= scaledMX && leftPanelX.x + paddingX >= scaledMX || isScalingLeftPanel) {
                    float size = scaledMX * window.getWidth() / 100f;
                    if (size < 1) size = 1;

                    window.setCursor(MouseCursor.HResize);
                    defaultCursor = false;
                    isScalingLeftPanel = window.isMouseButtonDown(0);
                    
                    if (isScalingLeftPanel) {
                        leftPanel = generateVAO(size, config.backgroundColor, program);
                        config.layout.leftPanelSize = size;
                    }
                }

                program.uniformMatrix4("projectionMatrix", scaledMatrix.toArray());

                leftPanel.draw(PrimitiveType.Triangles);

                Matrix4 oldMatrix = matrix;
                matrix = matrix.multiply(Matrix4.createTranslationMatrix(new Vector4(window.getWidth() / 50f - config.layout.rightPanelSize * 1, 0, 0, 1)));
                scaledMatrix = matrix.multiply(Matrix4.createScaleMatrix(new Vector4(1f / window.getWidth() * 100, 1, 1, 1)));

                Vector4 rightPanelX = scaledMatrix.transform(new Vector4(-1, 0, 1, 1));
                Console.WriteLine(rightPanelX.x);

                matrix = oldMatrix.multiply(Matrix4.createTranslationMatrix(new Vector4(window.getWidth() / 50f - config.layout.rightPanelSize * 2, 0, 0, 1)));
                scaledMatrix = matrix.multiply(Matrix4.createScaleMatrix(new Vector4(1f / window.getWidth() * 100, 1, 1, 1)));

                if (rightPanelX.x - paddingX <= scaledMX && rightPanelX.x + paddingX >= scaledMX || isScalingRightPanel) {
                    float size = (window.getWidth() - scaledMX * window.getWidth()) / 100f;
                    if (size < 1) size = 1;

                    window.setCursor(MouseCursor.HResize);
                    defaultCursor = false;
                    Console.WriteLine(size);
                    isScalingRightPanel = window.isMouseButtonDown(0);

                    if (isScalingRightPanel) {
                        rightPanel = generateVAO(size, config.backgroundColor, program);
                        config.layout.rightPanelSize = size;
                    }
                }
                
                program.uniformMatrix4("projectionMatrix", scaledMatrix.toArray());
                rightPanel.draw(PrimitiveType.Triangles);
                program.finish();

                if (defaultCursor) window.setCursor(MouseCursor.Default);

                window.endFrame();
                window.releaseContext();
            }

            window.grabContext();
            background.delete();
            leftPanel.delete();
            rightPanel.delete();
            program.deleteAttached();
            program.delete();
            window.releaseContext();

            write("config.json", config.asJson());
        }

        public static VertexBufferObject generateVBO(float width, ColorValue color) {
            VertexBuilder<float> builder = new VertexBuilder<float>(0);
            {
                Console.Write("W:");
                Console.WriteLine(width * 2 - 1);
                builder.position(-1,             -1, 0, 1).color(color.red, color.green, color.blue, color.alpha).texture(0, 0).endVertex();
                builder.position( width * 2 - 1, -1, 0, 1).color(color.red, color.green, color.blue, color.alpha).texture(1, 0).endVertex();
                builder.position( width * 2 - 1,  1, 0, 1).color(color.red, color.green, color.blue, color.alpha).texture(1, 1).endVertex();

                builder.position( width * 2 - 1,  1, 0, 1).color(color.red, color.green, color.blue, color.alpha).texture(1, 1).endVertex();
                builder.position(-1,              1, 0, 1).color(color.red, color.green, color.blue, color.alpha).texture(0, 1).endVertex();
                builder.position(-1,             -1, 0, 1).color(color.red, color.green, color.blue, color.alpha).texture(0, 0).endVertex();
            }
            VertexBufferObject buffer = new VertexBufferObject(BufferTarget.ArrayBuffer, builder.toArray());
            return buffer;
        }

        public static VertexArrayHelper generateVAO(float width, ColorValue color, ShaderProgram program) {
            {
                VertexArrayObject vao = new VertexArrayObject(6);
                vao.bind();
                // vboBackground = new VertexBufferObject(BufferTarget.ArrayBuffer, builder.toArray());
                VertexBufferObject vbo = generateVBO(width, color);

                int total = 4 + 2 + 4;
                int offset = 0;

                int posLoc = program.getAttribLocation("position");
                GL.VertexAttribPointer(posLoc, 4, VertexAttribPointerType.Float, false, total * sizeof(float), offset);
                GL.EnableVertexAttribArray(posLoc);

                int colorLoc = program.getAttribLocation("col");
                GL.VertexAttribPointer(colorLoc, 4, VertexAttribPointerType.Float, false, total * sizeof(float), (offset += 4) * sizeof(float));
                GL.EnableVertexAttribArray(colorLoc);

                int texLoc = program.getAttribLocation("tex");
                GL.VertexAttribPointer(texLoc, 2, VertexAttribPointerType.Float, false, total * sizeof(float), (offset += 4) * sizeof(float));
                GL.EnableVertexAttribArray(texLoc);
                vao.unbind();
                return new VertexArrayHelper(vao, vbo);
            }
        }

        public static string readFile(string path) {
            return System.IO.File.ReadAllText(path);
        }

        public static void write(string path, string text) {
            System.IO.File.WriteAllText(path, text);
        }

        public static void writeIfNotExist(string path, string text) {
            if (!System.IO.File.Exists(path)) {
                System.IO.File.WriteAllText(path, text);
            }
        }

        public static bool checkExistence(string path) {
            return System.IO.File.Exists(path);
        }
    }
}
