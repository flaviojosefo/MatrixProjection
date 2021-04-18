using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // W acts as a scalar
        public static explicit operator Vector(Vector4 v) => new Vector(v.X, v.Y, v.Z) / v.W;

        public static explicit operator Vector4(Vector v) => new Vector4(v.X, v.Y, v.Z);
    }
}
