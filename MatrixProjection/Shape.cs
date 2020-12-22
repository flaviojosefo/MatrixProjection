using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public abstract class Shape {

        public abstract Vector[] Vertices { get; protected set; }

        public abstract void DrawShape(Draw draw, Vector[] projected, bool render = true);
    }
}
