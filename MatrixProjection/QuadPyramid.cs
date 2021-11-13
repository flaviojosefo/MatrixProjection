using System;

namespace MatrixProjection {

    public class QuadPyramid : Mesh {

        public override Triangle[] Polygons { get; protected set; } = new Triangle[6];

        public QuadPyramid() {

            // Front
            CreateTri(new Vector(-0.5f, -0.5f, -0.5f),
                      new Vector(0.0f, 1.25f, 0.0f),
                      new Vector(0.5f, -0.5f, -0.5f));

            // Right
            CreateTri(new Vector(0.5f, -0.5f, -0.5f),
                      new Vector(0.0f, 1.25f, 0.0f),
                      new Vector(0.5f, -0.5f, 0.5f));

            // Back
            CreateTri(new Vector(0.5f, -0.5f, 0.5f),
                      new Vector(0.0f, 1.25f, 0.0f),
                      new Vector(-0.5f, -0.5f, 0.5f));

            // Left
            CreateTri(new Vector(-0.5f, -0.5f, 0.5f),
                      new Vector(0.0f, 1.25f, 0.0f),
                      new Vector(-0.5f, -0.5f, -0.5f));

            // Bottom
            CreateTri(new Vector(-0.5f, -0.5f, -0.5f),
                      new Vector(0.5f, -0.5f, 0.5f),
                      new Vector(-0.5f, -0.5f, 0.5f));

            CreateTri(new Vector(-0.5f, -0.5f, -0.5f),
                      new Vector(0.5f, -0.5f, -0.5f),
                      new Vector(0.5f, -0.5f, 0.5f));
        }
    }
}
