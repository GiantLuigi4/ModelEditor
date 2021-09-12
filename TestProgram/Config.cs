using fastJSON5;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using TestProgram.config;

namespace TestProgram {
    class Config {
        public ColorValue backgroundColor = new ColorValue(43, 43, 49, 255);
        public ColorValue secondaryBackgroundColor = new ColorValue(40, 40, 45, 255);
        public ColorValue foregroundColor = new ColorValue(46, 46, 53, 255);
        public ColorValue hoveredColor = new ColorValue(50, 50, 60, 255);
        public ColorValue borderColor = new ColorValue(30, 32, 30, 255);
        public Vec2iValue defaultSize = new Vec2iValue(500, 500);
        public LayoutConfig layout = new LayoutConfig();

        public string asJson() {
            StringBuilder builder = new StringBuilder();

            builder.Append("{");

            builder.Append("\"background\":").Append(backgroundColor.asJson()).Append(",");
            builder.Append("\"foreground\":").Append(foregroundColor.asJson()).Append(",");
            builder.Append("\"hovered\":").Append(hoveredColor.asJson()).Append(",");
            builder.Append("\"border\":").Append(borderColor.asJson()).Append(",");
            builder.Append("\"defaultSize\":").Append(defaultSize.asJson()).Append(",");
            builder.Append("\"layout\":").Append(layout.asJson());

            builder.Append("}");

            Console.WriteLine(builder.ToString());
            return fastJSON5.JSON5.Beautify(builder.ToString());
        }

        public void load(dynamic dynamic) {
            if (hasProperty(dynamic, "background")) fillColor(backgroundColor, dynamic.background);
            if (hasProperty(dynamic, "foreground")) fillColor(foregroundColor, dynamic.foreground);
            if (hasProperty(dynamic, "hovered")) fillColor(hoveredColor, dynamic.hovered);
            if (hasProperty(dynamic, "border")) fillColor(borderColor, dynamic.border);
            if (hasProperty(dynamic, "defaultSize")) fillVec(defaultSize, dynamic.defaultSize);
            if (hasProperty(dynamic, "layout")) layout = LayoutConfig.from(dynamic.layout);
        }

        private static void fillVec(Vec2iValue value, dynamic dynamic) {
            value.x = (int) dynamic.x;
            value.y = (int) dynamic.y;
        }

        private static void fillColor(ColorValue value, dynamic dynamic) {
            value.red = (int) dynamic.red;
            value.green = (int) dynamic.green;
            value.blue = (int) dynamic.blue;
            value.alpha = (int) dynamic.alpha;
        }

        public static bool hasProperty(dynamic dynamic, string name) {
            if (dynamic is ExpandoObject) return ((IDictionary<string, object>)dynamic).ContainsKey(name);
            // idk much about dynamic objects yet, but I think this should work for all dynamic objects?
            foreach (string str in dynamic.GetDynamicMemberNames()) if (str.Equals(name)) return true;
            return false;
//            return dynamic.GetType().GetProperty(name) != null;
        }
    }
}
