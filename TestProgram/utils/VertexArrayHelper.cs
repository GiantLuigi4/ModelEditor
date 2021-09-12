using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using tfc.wrappers.opentk;

namespace TestProgram.utils {
    class VertexArrayHelper {
        VertexArrayObject vao;
        VertexBufferObject vbo;

        public VertexArrayHelper(VertexArrayObject vao, VertexBufferObject vbo) {
            this.vao = vao;
            this.vbo = vbo;
            vao.bind();
            vbo.bind();
            vao.unbind();
        }

        public void draw(PrimitiveType type) {
            vao.bind();
            vao.drawArrays(type);
            vao.unbind();
        }

        public void delete() {
            vao.delete();
            vbo.delete();
        }
    }
}
