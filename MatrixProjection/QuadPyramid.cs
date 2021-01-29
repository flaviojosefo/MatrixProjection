using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class QuadPyramid : Mesh {

        public override Vector[][] Polygons { get; protected set; } = new Vector[6][];

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
