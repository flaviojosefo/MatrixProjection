using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public struct Triangle {

        private readonly Vector[] vertices;

        public int VertexCount { get => vertices.Length; }

        public ShadeChar Symbol { get; set; } // Flat Shading
        public ConsoleColor Color { get; set; }

        public Vector Normal {

            get {

                Vector u = vertices[1] - vertices[0];
                Vector v = vertices[2] - vertices[0];

                return -Vector.CrossProduct(u, v).Normalized;
            }
        }

        public Triangle(Vector[] vertices) {

            Array.Resize(ref vertices, 3);

            this.vertices = vertices;

            Symbol = ShadeChar.Full;
            Color = ConsoleColor.White;
        }

        public Triangle(Vector v1, Vector v2, Vector v3) {

            vertices = new Vector[3];

            vertices[0] = v1;
            vertices[1] = v2;
            vertices[2] = v3;

            Symbol = ShadeChar.Full;
            Color = ConsoleColor.White;
        }

        public Vector this[int i] { get => vertices[i]; set => vertices[i] = value; }
    }
}
