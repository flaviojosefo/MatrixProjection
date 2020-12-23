using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Engram : Shape {

        public override Vector[] Vertices { get; protected set; }

        public Engram() {

            Vertices = new Vector[20];

            // Essentially a Regular Dodecahedron
        }

        public override void DrawShape(Draw draw, Vector[] projected, bool render = true) {

            for (int i = 0; i < Vertices.Length / 2; i++) {

                // Magic here
            }
        }
    }
}
