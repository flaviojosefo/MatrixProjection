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

            Vertices[0] = new Vector(0.0f, -0.5f, -0.6f);
            Vertices[1] = new Vector(-Vertices[0].Z * (float)Math.Sin((Math.PI * -120) / 180), -0.5f, Vertices[0].Z * (float)Math.Cos((Math.PI * -120) / 180));
            Vertices[2] = new Vector(-Vertices[0].Z * (float)Math.Sin((Math.PI * 120) / 180), -0.5f, Vertices[0].Z * (float)Math.Cos((Math.PI * 120) / 180));
            Vertices[3] = new Vector(0.0f, 1.0f, 0.0f);
        }

        public override void DrawShape(DrawString draw, Vector[] projected) {

            for (int i = 0; i < Vertices.Length - 1; i++) {

                draw.AddLine(projected[i], projected[(i + 1) % 3]);
                draw.AddLine(projected[i], projected[3]);
            }
        }
    }
}
