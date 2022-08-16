using System;
using System.Runtime.CompilerServices;

namespace MatrixProjection {

    public struct Vector3 {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float Magnitude => GetMagnitude();

        public Vector3 Normalized => this / Magnitude;

        private static readonly Vector3 zeroV = new Vector3(0f, 0f, 0f);
        private static readonly Vector3 oneV = new Vector3(1f, 1f, 1f);
        private static readonly Vector3 rightV = new Vector3(1f, 0f, 0f);
        private static readonly Vector3 upV = new Vector3(0f, 1f, 0f);
        private static readonly Vector3 forwardV = new Vector3(0f, 0f, 1f);

        public static Vector3 Zero {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => zeroV;
        }
        public static Vector3 One {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => oneV;
        }
        public static Vector3 Right {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => rightV;
        }
        public static Vector3 Up {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => upV;
        }
        public static Vector3 Forward {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => forwardV;
        }

        public Vector3(float x, float y, float z = 0) {

            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator -(Vector3 v) {

            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2) {

            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2) {

            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v, float scalar) {

            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector3 operator *(float scalar, Vector3 v) {

            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector3 operator /(Vector3 v, float scalar) {

            return new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2) {

            return ((v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z));
        }

        public static bool operator !=(Vector3 v1, Vector3 v2) {

            return ((v1.X != v2.X) || (v1.Y != v2.Y) || (v1.Z != v2.Z));
        }

        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2) {

            return new Vector3((v1.Y * v2.Z) - (v1.Z * v2.Y),
                              (v1.Z * v2.X) - (v1.X * v2.Z),
                              (v1.X * v2.Y) - (v1.Y * v2.X));
        }

        public static float DotProduct(Vector3 v1, Vector3 v2) {

            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static float Distance(Vector3 v1, Vector3 v2) {

            return (v2 - v1).Magnitude;
        }

        public static Vector3 PlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 lineStart, Vector3 lineEnd) {

            // Add math here

            return new Vector3();
        }

        private float GetMagnitude() {

            return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public void Normalize() { this = Normalized; }

        public override string ToString() => $"({X},{Y},{Z})";

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => base.Equals(obj);
    }
}
