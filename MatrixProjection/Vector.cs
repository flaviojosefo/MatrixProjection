using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public struct Vector {

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public Vector (float x, float y, float z = 0) {

            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() {

            return $"({X},{Y},{Z})";
        }
    }
}
