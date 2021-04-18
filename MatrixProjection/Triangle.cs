using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public struct Triangle {

        public Vector[] Vertices { get; }

        public int VertexCount { get => Vertices.Length; }

        public Vector Normal {

            get {

                Vector u = Vertices[1] - Vertices[0];
                Vector v = Vertices[2] - Vertices[0];

                return -Vector.CrossProduct(u, v).Normalized;
            }
        }

        public Triangle(Vector[] vertices) {

            Array.Resize(ref vertices, 3);

            Vertices = vertices;
        }

        public Triangle(Vector v1, Vector v2, Vector v3) {

            Vertices = new Vector[3];

            Vertices[0] = v1;
            Vertices[1] = v2;
            Vertices[2] = v3;
        }
    }
}
