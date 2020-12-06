using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public struct Vector {

        private float x, y, z;

        public Vector (float x, float y, float z = 0) {

            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() {

            return $"({x},{y},{z})";
        }
    }
}
