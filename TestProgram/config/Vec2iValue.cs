using System;
using System.Collections.Generic;
using System.Text;

namespace TestProgram.config {
    class Vec2iValue {
        public int x, y;

        public Vec2iValue(int x, int y) {
            this.x = x;
            this.y = y;
        }
        
        public string asJson() {
            StringBuilder builder = new StringBuilder();

            builder.Append("{")
                .Append("\"x\":").Append(x).Append(",")
                .Append("\"y\":").Append(y).Append("}");

            return builder.ToString();
        }
    }
}
