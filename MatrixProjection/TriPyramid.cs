using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class TriPyramid : Shape {

        public override Vector[] Vertices { get; protected set; }

        public TriPyramid() {

            Vertices = new Vector[4];

            Vertices[0] = new Vector(0.5f, -0.5f, (float)Math.Sqrt(3.0f) / 4.0f);
            Vertices[1] = new Vector(-0.5f, -0.5f, (float)Math.Sqrt(3.0f) / 4.0f);
            Vertices[2] = new Vector(0.0f, -0.5f, -((float)Math.Sqrt(3.0f) / 4.0f));
            Vertices[3] = new Vector(0.0f, 1.0f, 0.0f);
        }

        public override void DrawShape(Draw draw, Vector[] projected, bool render = true) {

            for (int i = 0; i < Vertices.Length - 1; i++) {

                draw.DrawLine(projected[i], projected[(i + 1) % 3], render);
                draw.DrawLine(projected[i], projected[3], render);
            }
        }
    }
}
