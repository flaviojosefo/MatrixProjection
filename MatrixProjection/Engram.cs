using System;
using System.Collections.Generic;

namespace MatrixProjection {

    public class Engram : Mesh {

        public override Triangle[] Polygons { get; protected set; } = new Triangle[36];

        // Had to do it manually since the dodecahedron is created with arbitrary vectors
        public Engram() {

            float radius = 0.8f;

            Vector[] verts = MakeDodecahedron(radius);

            // Pentagon 1
            CreateTri(verts[0], verts[1], verts[3]);
            CreateTri(verts[0], verts[4], verts[1]);
            CreateTri(verts[0], verts[5], verts[4]);

            // Pentagon 2
            CreateTri(verts[0], verts[13], verts[11]);
            CreateTri(verts[0], verts[11], verts[14]);
            CreateTri(verts[0], verts[14], verts[5]);

            // Pentagon 3
            CreateTri(verts[0], verts[3], verts[2]);
            CreateTri(verts[0], verts[2], verts[12]);
            CreateTri(verts[0], verts[12], verts[13]);

            // Pentagon 4
            CreateTri(verts[5], verts[14], verts[17]);
            CreateTri(verts[5], verts[17], verts[7]);
            CreateTri(verts[5], verts[7], verts[4]);

            // Pentagon 5
            CreateTri(verts[3], verts[1], verts[6]);
            CreateTri(verts[3], verts[6], verts[8]);
            CreateTri(verts[3], verts[8], verts[2]);

            // Pentagon 6
            CreateTri(verts[13], verts[12], verts[18]);
            CreateTri(verts[13], verts[18], verts[16]);
            CreateTri(verts[13], verts[16], verts[11]);

            // Pentagon 7
            CreateTri(verts[2], verts[8], verts[10]);
            CreateTri(verts[2], verts[10], verts[18]);
            CreateTri(verts[2], verts[18], verts[12]);

            // Pentagon 8
            CreateTri(verts[1], verts[4], verts[7]);
            CreateTri(verts[1], verts[7], verts[9]);
            CreateTri(verts[1], verts[9], verts[6]);

            // Pentagon 9
            CreateTri(verts[11], verts[16], verts[19]);
            CreateTri(verts[11], verts[19], verts[17]);
            CreateTri(verts[11], verts[17], verts[14]);

            // Pentagon 10
            CreateTri(verts[6], verts[9], verts[15]);
            CreateTri(verts[6], verts[15], verts[10]);
            CreateTri(verts[6], verts[10], verts[8]);

            // Pentagon 11
            CreateTri(verts[16], verts[18], verts[10]);
            CreateTri(verts[16], verts[10], verts[15]);
            CreateTri(verts[16], verts[15], verts[19]);

            // Pentagon 12
            CreateTri(verts[17], verts[19], verts[15]);
            CreateTri(verts[17], verts[15], verts[9]);
            CreateTri(verts[17], verts[9], verts[7]);
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
    }
}
