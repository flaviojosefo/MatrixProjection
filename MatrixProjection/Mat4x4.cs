using System;

namespace MatrixProjection {

    public struct Mat4x4 {

        private float[,] matrix;

        public int Length => matrix.Length;

        public Mat4x4 Inverse => GetInverse();

        public static Mat4x4 Identity {
            get {
                return new float[4, 4] {
                    {1,0,0,0},
                    {0,1,0,0},
                    {0,0,1,0},
                    {0,0,0,1}
                };
            }
        }

        public float this[int i, int j] { get => matrix[i, j]; }

        public float[,] this[float[,] m] { set => matrix = m; }

        public static implicit operator Mat4x4(float[,] m) => new Mat4x4 { matrix = m };

        public static Mat4x4 Vec4ToMat(Vector4 v) {

            return new float[1, 4] {
                {v.X,v.Y,v.Z,v.W}
            };
        }

        public static Mat4x4 VecToMat(Vector v) {

            return Vec4ToMat((Vector4)v);
        }

        public static Vector MatToVec(Mat4x4 m) {

            return (Vector)MatToVec4(m);
        }

        public static Vector4 MatToVec4(Mat4x4 m) {

            return new Vector4(m[0, 0],
                               m[0, 1],
                               m[0, 2],
                               m[0, 3]);
        }

        // Row-major order multiplication
        public static Vector MatMul(Vector v, Mat4x4 m) {

            return MatMul((Vector4)v, m);
        }

        // Row-major order multiplication
        public static Vector MatMul(Vector4 v, Mat4x4 m) {

            Vector4 multVector = new Vector4(
                v.X * m[0, 0] + v.Y * m[0, 1] + v.Z * m[0, 2] + v.W * m[0, 3],
                v.X * m[1, 0] + v.Y * m[1, 1] + v.Z * m[1, 2] + v.W * m[1, 3],
                v.X * m[2, 0] + v.Y * m[2, 1] + v.Z * m[2, 2] + v.W * m[2, 3],
                v.X * m[3, 0] + v.Y * m[3, 1] + v.Z * m[3, 2] + v.W * m[3, 3]);

            return (Vector)multVector;
        }

        // Row-major order multiplication
        public static Mat4x4 MatMul(Mat4x4 m1, Mat4x4 m2) {

            float[,] newMatrix = new float[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); i++) {

                for (int j = 0; j < m2.GetLength(1); j++) {

                    float sum = 0;

                    for (int k = 0; k < m1.GetLength(1); k++) {

                        sum += m1[i, k] * m2[k, j];
                    }

                    newMatrix[i, j] = sum;
                }
            }

            return newMatrix;
        }

        public static Mat4x4 Transpose(Mat4x4 m) {

            float[,] transposed = new float[m.GetLength(0), m.GetLength(1)];

            for (int i = 0; i < m.GetLength(0); i++) {

                for (int j = 0; j < m.GetLength(1); j++) {

                    transposed[i, j] = m[j, i];
                }
            }

            return transposed;
        }

        private Mat4x4 GetInverse() {

            return new Mat4x4();
        }

        private int GetLength(int dimension) => matrix.GetLength(dimension);

        // Print out matrix
        public override string ToString() {

            string mat = "";

            for (int i = 0; i < matrix.GetLength(0); i++) {

                for (int j = 0; j < matrix.GetLength(1); j++) {

                    mat += $"{matrix[i, j]} ";
                }

                mat += '\n';
            }

            return mat;
        }
    }
}
