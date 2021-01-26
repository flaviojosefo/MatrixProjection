using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Cube : Mesh {

        public Cube(float size) {

            Polygons = new Vector[6][];

            CreateQuad(new Vector(size * 0.5f, 0, 0),
                       new Vector(0, -size * 0.5f, 0),
                       new Vector(0, 0, size * 0.5f));

            CreateQuad(new Vector(-size * 0.5f, 0, 0),
                       new Vector(0, size * 0.5f, 0),
                       new Vector(0, 0, size * 0.5f));

            CreateQuad(new Vector(0, size * 0.5f, 0),
                       new Vector(size * 0.5f, 0),
                       new Vector(0, 0, size * 0.5f));

            CreateQuad(new Vector(0, -size * 0.5f, 0),
                       new Vector(-size * 0.5f, 0),
                       new Vector(0, 0, size * 0.5f));

            CreateQuad(new Vector(0, 0, size * 0.5f),
                       new Vector(-size * 0.5f, 0),
                       new Vector(0, size * 0.5f, 0));

            CreateQuad(new Vector(0, 0, -size * 0.5f),
                       new Vector(size * 0.5f, 0),
                       new Vector(0, size * 0.5f, 0));
        }
    }
}
