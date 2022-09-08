using System;

namespace MatrixProjection {

    public abstract class Mesh {

        // Mesh = Collection of polygons, which is a collection of vertices (tris or quads)
        public abstract Triangle[] Polygons { get; protected set; }

        // Clockwise quad creation
        //protected void CreateQuad(Vector3 origin, Vector3 axis0, Vector3 axis1) {

        //    Vector3[] quad = new Vector3[4];

        //    quad[0] = origin - axis0 - axis1;
        //    quad[1] = origin - axis0 + axis1;
        //    quad[2] = origin + axis0 + axis1;
        //    quad[3] = origin + axis0 - axis1;

        //    int firstEmpty = Array.IndexOf(Polygons, null);
        //    Polygons[firstEmpty] = quad;
        //}

        // Triangles need to be supplied on a CLOCKWISE order
        protected void CreateTri(Vector3 v1, Vector3 v2, Vector3 v3) {

            Vector3[] tri = new Vector3[3];

            tri[0] = v1;
            tri[1] = v2;
            tri[2] = v3;

            int firstEmpty = Array.IndexOf(Polygons, default);
            Polygons[firstEmpty] = new Triangle(tri);
        }
    }
}
