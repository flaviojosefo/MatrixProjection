using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MatrixProjection {

    public class CustomMesh : Mesh {

        public override Triangle[] Polygons { get; protected set; }

        public CustomMesh(Triangle[] polygons) => Polygons = polygons;

        // Generates a CustomMesh based on information present on an '.obj' file
        public static CustomMesh GenerateFromOBJ(string filePath) {

            // The collections that will aid in creating the mesh
            List<Vector3> vertices = new List<Vector3>();
            List<Triangle> triangles = new List<Triangle>();

            // Start reading the file
            using (TextReader reader = File.OpenText(filePath)) {

                // Check if there are more lines (without actually reading them)
                while (reader.Peek() > -1) {

                    // Save all strings present on a line separated by a space
                    string[] values = reader.ReadLine().Split(' ');

                    // Check if a line presents vertex information
                    if (values[0] == "v") {

                        // Negate X since obj uses a right-handed coordinate system
                        // and this engine uses a left-handed one
                        Vector3 vertex = new Vector3(-float.Parse(values[1], CultureInfo.InvariantCulture),  // X
                                                      float.Parse(values[2], CultureInfo.InvariantCulture),  // Y
                                                      float.Parse(values[3], CultureInfo.InvariantCulture)); // Z

                        // Add the created vertex into the list
                        vertices.Add(vertex);

                    // Check if a line presents face information
                    } else if (values[0] == "f") {

                        // Cache the number of vertices on this face
                        int faceVertices = values.Length - 1;

                        // Check if the face is a TRIANGLE
                        if (faceVertices == 3) {

                            // Save a triangle in CLOCKWISE order
                            triangles.Add(new Triangle(vertices[int.Parse(values[1].Split('/').First()) - 1],
                                                       vertices[int.Parse(values[3].Split('/').First()) - 1],
                                                       vertices[int.Parse(values[2].Split('/').First()) - 1]));

                        // Check if the face is a QUAD
                        } else if (faceVertices == 4) {

                            // Divide the quad into 2 triangles
                            for (int i = 2; i < faceVertices; i++) {

                                // Save a triangle in CLOCKWISE order
                                triangles.Add(new Triangle(vertices[int.Parse(values[1].Split('/').First()) - 1],
                                                           vertices[int.Parse(values[i + 1].Split('/').First()) - 1],
                                                           vertices[int.Parse(values[i].Split('/').First()) - 1]));
                            }

                        // Check if the face is neither of the above
                        } else {

                            // Throw error
                            throw new Exception("Error: Face indices amount could not be handled.\n" +
                                                "Make sure the '.obj' file only contains faces which are triangles or quads.");
                        }
                    }
                }

                // Stop reading the file
                reader.Close();
            }

            // Return the mesh with the created triangles
            return new CustomMesh(triangles.ToArray());
        }
    }
}
