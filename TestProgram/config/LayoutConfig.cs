using System;
using System.Collections.Generic;
using System.Text;

namespace TestProgram.config {
    class LayoutConfig {
        public float leftPanelSize = 2;
        public float rightPanelSize = 2;

        public string asJson() {
            StringBuilder builder = new StringBuilder();
            builder.Append("{")
                .Append("\"leftPanelSize\":").Append(leftPanelSize).Append(",")
                .Append("\"rightPanelSize\":").Append(rightPanelSize)
                .Append("}");
            return builder.ToString();
        }

        public static LayoutConfig from(dynamic dynamic) {
            LayoutConfig config = new LayoutConfig();
            if (Config.hasProperty(dynamic, "leftPanelSize")) {
                config.leftPanelSize = (float) dynamic.leftPanelSize;
                Console.WriteLine(config.leftPanelSize);
            }
            if (Config.hasProperty(dynamic, "rightPanelSize")) config.rightPanelSize = (float) dynamic.rightPanelSize;
            return config;
        }
    }
}
