using System;

namespace MatrixProjection {

    public struct Triangle {

        private readonly Vector3[] vertices;
        public Vector3[] Vertices => vertices;

        public int VertexCount { get => vertices.Length; }

        public ShadeChar Symbol { get; set; } // Flat Shading
        public ConsoleColor Color { get; set; }

        public Vector3 Normal {

            get {

                Vector3 u = vertices[1] - vertices[0];
                Vector3 v = vertices[2] - vertices[0];

                return Vector3.CrossProduct(u, v).Normalized;
            }
        }

        public Triangle(Vector3[] vertices) {

            Array.Resize(ref vertices, 3);

            this.vertices = vertices;

            Symbol = ShadeChar.Full;
            Color = ConsoleColor.White;
        }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3) {

            vertices = new Vector3[3];

            vertices[0] = v1;
            vertices[1] = v2;
            vertices[2] = v3;

            Symbol = ShadeChar.Full;
            Color = ConsoleColor.White;
        }

        public Vector3 this[int i] { get => vertices[i]; set => vertices[i] = value; }

        public override string ToString() {

            return $"[{vertices[0]} | {vertices[1]} | {vertices[2]}]";
        }
    }
}
