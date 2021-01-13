using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Engram : Shape {

        public override Vector[] Vertices { get; protected set; }

        private (int vertex1, int vertex2)[] connections;

        public Engram() {

            Vertices = MakeDodecahedron(0.5f);
            GetConnections();
        }

        public override void DrawShape(DrawString draw, Vector[] projected) {

            for (int i = 0; i < connections.Length; i++) {

                draw.AddLine(projected[connections[i].vertex1], projected[connections[i].vertex2]);
            }
        }

        /// <summary>
        /// Generates a list of vertices (in arbitrary order) for a tetrahedron centered on the origin.
        /// </summary>
        /// <param name="r">The distance of each vertex from origin.</param>
        /// <returns></returns>
        private Vector[] MakeDodecahedron(float r) {

            // Calculate constants that will be used to generate vertices
            float phi = (float)(1 + Math.Sqrt(5)) / 2; // The golden ratio

            float a = 1.0f / (float)Math.Sqrt(3);
            float b = a / phi;
            float c = a * phi;

            List<Vector> vertices = new List<Vector>();

            // Generate each vertex
            foreach (int i in new[] { -1, 1 }) {

                foreach (int j in new[] { -1, 1 }) {

                    vertices.Add(new Vector(
                                        0,
                                        i * c * r,
                                        j * b * r));
                    vertices.Add(new Vector(
                                        i * c * r,
                                        j * b * r,
                                        0));
                    vertices.Add(new Vector(
                                        i * b * r,
                                        0,
                                        j * c * r));

                    foreach (int k in new[] { -1, 1 }) {

                        vertices.Add(new Vector(
                                            i * a * r,
                                            j * a * r,
                                            k * a * r));
                    }
                }
            }

            return vertices.ToArray();
        }

        private void GetConnections() {

            connections = new (int, int)[30];

            float bestDistance = (float)Math.Round(GetBestDistance(), 3);

            for (int i = 0; i < Vertices.Length; i++) {

                for (int j = 0; j < Vertices.Length; j++) {

                    float currentDistance = (float)Math.Round(Vector.Distance(Vertices[i], Vertices[j]), 3);

                    if (currentDistance == bestDistance) {

                        for (int k = 0; k < connections.Length; k++) {

                            if ((i == connections[k].vertex1 && j == connections[k].vertex2) ||
                                (j == connections[k].vertex1 && i == connections[k].vertex2)) {

                                break;

                            } else if (connections[k] == default) {

                                connections[k] = (i, j);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private float GetBestDistance() {

            float bestDistance = float.MaxValue;

            for (int i = 1; i < Vertices.Length; i++) {

                float currentDistance = Vector.Distance(Vertices[0], Vertices[i]);

                if (currentDistance < bestDistance) {

                    bestDistance = currentDistance;
                }
            }

            return bestDistance;
        }
    }
}
