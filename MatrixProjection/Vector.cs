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

        public static Vector operator +(Vector v1, Vector v2) {

            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2) {

            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator /(Vector v, float scalar) {

            return new Vector(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        public override string ToString() {

            return $"({X},{Y},{Z})";
        }
    }
}
