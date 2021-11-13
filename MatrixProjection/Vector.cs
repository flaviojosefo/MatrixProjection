using System;

namespace MatrixProjection {

    public struct Vector {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float Magnitude => GetMagnitude();

        public Vector Normalized => this / Magnitude;

        public Vector(float x, float y, float z = 0) {

            X = x;
            Y = y;
            Z = z;
        }

        public static Vector operator -(Vector v) {

            return new Vector(-v.X, -v.Y, -v.Z);
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

        public static Vector operator *(float scalar, Vector v) {

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

        public static float DotProduct(Vector v1, Vector v2) {

            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static float Distance(Vector v1, Vector v2) {

            return (v2 - v1).Magnitude;
        }

        public static Vector PlaneIntersection(Vector planePoint, Vector planeNormal, Vector lineStart, Vector lineEnd) {

            // Add math here

            return new Vector();
        }

        private float GetMagnitude() {

            return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public void Normalize() { this = Normalized; }

        public override string ToString() {

            return $"({X},{Y},{Z})";
        }
    }
}
