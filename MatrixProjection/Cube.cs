using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Cube : Shape {

        public override Vector[] Vertices { get; protected set; }

        public Cube() {

            Vertices = new Vector[8];

            Vertices[0] = new Vector(0.5f, 0.5f, 0.5f);
            Vertices[1] = new Vector(-0.5f, 0.5f, 0.5f);
            Vertices[2] = new Vector(-0.5f, -0.5f, 0.5f);
            Vertices[3] = new Vector(0.5f, -0.5f, 0.5f);
            Vertices[4] = new Vector(0.5f, 0.5f, -0.5f);
            Vertices[5] = new Vector(-0.5f, 0.5f, -0.5f);
            Vertices[6] = new Vector(-0.5f, -0.5f, -0.5f);
            Vertices[7] = new Vector(0.5f, -0.5f, -0.5f);
        }

        public override void DrawShape(Draw draw, Vector[] projected, bool render = true) {

            for (int i = 0; i < Vertices.Length / 2; i++) {

                draw.DrawLine(projected[i], projected[i + 4], render);
                draw.DrawLine(projected[i], projected[(i + 1) % 4], render);
                draw.DrawLine(projected[i + 4], projected[((i + 1) % 4) + 4], render);
            }
        }
    }
}
