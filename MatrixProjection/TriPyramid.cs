using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class TriPyramid : Mesh {

        public override Triangle[] Polygons { get; protected set; } = new Triangle[4];

        public TriPyramid() {

            // The Vector from which we make the perfect equilateral triangular base
            // Changing this Vector increases the base's area
            Vector firstVertex = new Vector(0.0f, -0.6f, 0.6f);

            // The vertex at the top of the pyramid
            Vector topVertex = new Vector(0.0f, 1.0f, 0.0f);

            // Pre calculate the base, so we don't have to repeat Sin and Cos related calculations
            Vector[] triBase = new Vector[3] {
                firstVertex,
                new Vector(-firstVertex.Z * (float)Math.Sin((Math.PI * -120.0f) / 180.0f), firstVertex.Y, firstVertex.Z * (float)Math.Cos((Math.PI * -120.0f) / 180.0f)),
                new Vector(-firstVertex.Z * (float)Math.Sin((Math.PI * 120.0f) / 180.0f), firstVertex.Y, firstVertex.Z * (float)Math.Cos((Math.PI * 120.0f) / 180.0f))
            };

            // Front
            CreateTri(triBase[2],
                      topVertex,
                      triBase[1]);

            // Right
            CreateTri(triBase[1],
                      topVertex,
                      triBase[0]);

            // Left
            CreateTri(triBase[0],
                      topVertex,
                      triBase[2]);

            // Bottom
            CreateTri(triBase[2],
                      triBase[1],
                      triBase[0]);
        }
    }
}
