using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public struct Vector {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float Magnitude => GetMagnitude();

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
        public static Vector operator *(Vector v, float scalar) {

            return new Vector(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector operator /(Vector v, float scalar) {

            return new Vector(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        public static bool operator ==(Vector v1, Vector v2) {

            return ((v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z));
        }

        public static bool operator !=(Vector v1, Vector v2) {

            return ((v1.X != v2.X) || (v1.Y != v2.Y) || (v1.Z != v2.Z));
        }

        public static Vector CrossProduct(Vector v1, Vector v2) {

            return new Vector((v1.Y * v2.Z) - (v1.Z * v2.Y),
                              (v1.Z * v2.X) - (v1.X * v2.Z),
                              (v1.X * v2.Y) - (v1.Y * v2.X));
        }

        public static float Distance(Vector v1, Vector v2) {

            return (v2 - v1).Magnitude;
        }

        private float GetMagnitude() {

            return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public override string ToString() {

            return $"({X},{Y},{Z})";
        }
    }
}
