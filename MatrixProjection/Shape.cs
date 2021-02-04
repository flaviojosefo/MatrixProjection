using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public abstract class Shape {

        public Vector[] Vertices { get; protected set; }

        public abstract void DrawShape(DrawString draw, Vector[] projected);
    }
}
