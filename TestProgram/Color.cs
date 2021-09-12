namespace TestProgram {
    class Color {
        int val;

        public Color(int val) {
            this.val = val;
        }

        // https://stackoverflow.com/a/4801397
        public Color(int r, int g, int b) {
            /*this.val = r;
            val = (val << 8) + g;
            val = (val << 8) + b;*/
            this.val = (255 << 24) + (r << 16) + (g << 8) + b;
        }

        public int getAlpha() {
            return (val >> 24) & 0xFF;
        }

        public int getRed() {
            return (val >> 16) & 0xFF;
        }

        public int getGreen() {
            return (val >> 8) & 0xFF;
        }

        public int getBlue() {
            return (val) & 0xFF;
        }
    }
}
