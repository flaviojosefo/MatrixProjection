﻿using System;

namespace MatrixProjection {

    public class TriPyramid : Mesh {

        public override Triangle[] Polygons { get; protected set; } = new Triangle[4];

        public TriPyramid() {

            // The Vector3 from which we make the perfect equilateral triangular base
            // Changing this Vector3 increases the base's area
            Vector3 firstVertex = new Vector3(0.0f, -0.6f, 0.6f);

            // The vertex at the top of the pyramid
            Vector3 topVertex = new Vector3(0.0f, 1.0f, 0.0f);

            // Pre calculate the base, so we don't have to repeat Sin and Cos related calculations
            Vector3[] triBase = new Vector3[3] {
                firstVertex,
                new Vector3(-firstVertex.Z * (float)Math.Sin(-120.0f * (Math.PI / 180.0f)), firstVertex.Y, firstVertex.Z * (float)Math.Cos(-120.0f * (Math.PI / 180.0f))),
                new Vector3(-firstVertex.Z * (float)Math.Sin(120.0f * (Math.PI / 180.0f)), firstVertex.Y, firstVertex.Z * (float)Math.Cos(120.0f * (Math.PI / 180.0f)))
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
