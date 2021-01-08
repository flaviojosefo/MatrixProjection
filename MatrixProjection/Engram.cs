using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Engram : Shape {

        public override Vector[] Vertices { get; protected set; }

        private (int index1, int index2)[] connections;

        public Engram() {

            Vertices = MakeDodecahedron(0.5f);
            GetConnections();
        }

        public override void DrawShape(DrawString draw, Vector[] projected) {

            for (int i = 0; i < connections.Length; i++) {

                draw.AddLine(projected[connections[i].index1], projected[connections[i].index2]);
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

            connections = new (int v1, int v2)[30];

            float bestDistance = (float)Math.Round(GetBestDistance(), 3);

            for (int i = 0; i < Vertices.Length; i++) {

                int connectIndex = 0;

                for (int j = 0; j < Vertices.Length; j++) {

                    bool skip = false;

                    if (Vertices[i] == Vertices[j]) continue;

                    float currentDistance = (float)Math.Round(Vector.Distance(Vertices[i], Vertices[j]), 3);

                    if (currentDistance == bestDistance) {

                        for (int k = 0; k < connections.Length; k++) {

                            if ((i == connections[k].index1 && j == connections[k].index2) ||
                                (j == connections[k].index1 && i == connections[k].index2)) {

                                skip = true;
                                break;

                            } else if (connections[k] == default) {

                                connectIndex = k;
                                break;
                            }
                        }

                        if (skip) continue;

                        connections[connectIndex] = (i, j);
                    }
                }
            }
        }

        private float GetBestDistance() {

            float bestDistance = float.MaxValue;

            for (int i = 0; i < 1; i++) {

                for (int j = 0; j < Vertices.Length; j++) {

                    if (Vertices[i] == Vertices[j]) continue;

                    float currentDistance = Vector.Distance(Vertices[i], Vertices[j]);

                    if (currentDistance < bestDistance) {

                        bestDistance = currentDistance;
                    }
                }
            }

            return bestDistance;
        }
    }
}
