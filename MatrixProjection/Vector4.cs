namespace MatrixProjection {

    public struct Vector4 {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public Vector4(float x, float y, float z, float w = 1) {

            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        // W gets dropped
        public static explicit operator Vector3(Vector4 v) => new Vector3(v.X, v.Y, v.Z);

        public static explicit operator Vector4(Vector3 v) => new Vector4(v.X, v.Y, v.Z);

        public override string ToString() => $"({X},{Y},{Z},{W})";
    }
}
