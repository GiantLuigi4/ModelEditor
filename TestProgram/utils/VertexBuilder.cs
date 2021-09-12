using System;
using System.Collections.Generic;
using System.Text;

namespace TestProgram.utils {
    class VertexBuilder<T> where T : unmanaged {
        List<T> vertexList;
        int index = 0;

        public VertexBuilder(int initialAllocation) {
            // TODO: make this useful
            vertexList = new List<T>(initialAllocation);
        }

        public virtual VertexBuilder<T> position(T x, T y, T z, T w) {
            vertexList.Add(x);
            vertexList.Add(y);
            vertexList.Add(z);
            vertexList.Add(w);
            return this;
        }

        public virtual VertexBuilder<T> color(T r, T g, T b, T a) {
            vertexList.Add((T) ((dynamic) r / 255.0));
            vertexList.Add((T) ((dynamic) g / 255.0));
            vertexList.Add((T) ((dynamic) b / 255.0));
            vertexList.Add((T) ((dynamic) a / 255.0));
            return this;
        }

        public virtual void endVertex() {
            index++;
        }

        public virtual VertexBuilder<T> texture(int u, int v) {
            vertexList.Add((T) ((dynamic) u / 255.0));
            vertexList.Add((T) ((dynamic) v / 255.0));
            return this;
        }

        public virtual int countVerticies() {
            return index;
        }

        public virtual T[] toArray() {
            return vertexList.ToArray();
        }
    }
}
