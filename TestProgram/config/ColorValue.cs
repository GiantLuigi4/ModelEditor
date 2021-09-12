using System;
using System.Text;

namespace TestProgram.config {
    class ColorValue {
        public int red, green, blue, alpha;

        public ColorValue(int r, int g, int b, int a) {
            red = r;
            green = g;
            blue = b;
            alpha = a;
        }

        public string asJson() {
            StringBuilder builder = new StringBuilder();

            builder.Append("{")
                .Append("\"red\":").Append(red).Append(",")
                .Append("\"green\":").Append(green).Append(",")
                .Append("\"blue\":").Append(blue).Append(",")
                .Append("\"alpha\":").Append(alpha).Append("}");

            return builder.ToString();
        }
    }
}
