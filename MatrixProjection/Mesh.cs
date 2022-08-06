using System;
using System.Collections.Generic;
using System.Linq;

namespace MatrixProjection {

    public abstract class Mesh {

        // Mesh = Collection of polygons, which is a collection of vertices (tris or quads)
        public abstract Triangle[] Polygons { get; protected set; }

        // Clockwise quad creation
        //protected void CreateQuad(Vector origin, Vector axis0, Vector axis1) {

        //    Vector[] quad = new Vector[4];

        //    quad[0] = origin - axis0 - axis1;
        //    quad[1] = origin - axis0 + axis1;
        //    quad[2] = origin + axis0 + axis1;
        //    quad[3] = origin + axis0 - axis1;

        //    int firstEmpty = Array.IndexOf(Polygons, null);
        //    Polygons[firstEmpty] = quad;
        //}

        // Clockwise tri creation
        protected void CreateTri(Vector v1, Vector v2, Vector v3) {

            Vector[] tri = new Vector[3];

            tri[0] = v1;
            tri[1] = v2;
            tri[2] = v3;

            int firstEmpty = Array.IndexOf(Polygons, default);
            Polygons[firstEmpty] = new Triangle(tri);
        }
    }
}
