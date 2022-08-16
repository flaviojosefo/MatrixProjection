using System;

namespace MatrixProjection {

    // LEFT-Handed and ROW-Major
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

        public static Mat4x4 VecToMat(Vector3 v) {

            return Vec4ToMat((Vector4)v);
        }

        public static Vector3 MatToVec(Mat4x4 m) {

            return (Vector3)MatToVec4(m);
        }

        public static Vector4 MatToVec4(Mat4x4 m) {

            return new Vector4(m[0,0], m[0,1], m[0,2], m[0,3]);
        }

        // For vectors that store the 'W' component
        public static Vector4 MatMul(Vector4 v, Mat4x4 m) {

            return MatToVec4(MatMul(Vec4ToMat(v), m));
        }

        // For vectors that represent 'positions'
        public static Vector3 MatMul(Vector3 v, Mat4x4 m) {

            return (Vector3)MatMul((Vector4)v, m);
        }

        // For vectors that represent 'directions'
        public static Vector3 MatMul(Mat4x4 m, Vector3 v) {

            Vector4 v4 = new Vector4(v.X, v.Y, v.Z, 0);

            return (Vector3)MatMul(v4, m);
        }

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

            float[,] transposed = new float[m.GetLength(1), m.GetLength(0)];

            for (int i = 0; i < m.GetLength(1); i++) {

                for (int j = 0; j < m.GetLength(0); j++) {

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
