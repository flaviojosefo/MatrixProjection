using System;

namespace MatrixProjection {

    public class SimpleTri : Mesh {

        public override Triangle[] Polygons { get; protected set; } = new Triangle[1];

        public SimpleTri() {

            // The center of the triangle
            Vector3 triCenter = Vector3.Zero;

            // The distance from the center to each vertex
            float dist = 2f;

            // The top vertex from which the other 2 will be created
            Vector3 firstVertex = triCenter + new Vector3(0, dist);

            // The vertex to the right of the top vertex
            Vector3 secondVertex = RotateVertex(firstVertex, -120.0f);

            // The vertex to the left of the top vertex
            Vector3 thirdVertex = RotateVertex(firstVertex, 120.0f);

            // Create the triangle adding the vertices in a clockwise direction
            CreateTri (
                firstVertex,
                secondVertex,
                thirdVertex
            );
        }

        // Rotates a vertices' x and y coords
        private Vector3 RotateVertex(Vector3 vertex, float angle) {

            float angleRad = angle * (float)(Math.PI / 180.0f);

            float rotX = (vertex.X * (float)Math.Cos(angleRad)) - (vertex.Y * (float)Math.Sin(angleRad));
            float rotY = (vertex.Y * (float)Math.Cos(angleRad)) - (vertex.X * (float)Math.Sin(angleRad));

            return new Vector3(rotX, rotY, vertex.Z);
        }
    }
}
