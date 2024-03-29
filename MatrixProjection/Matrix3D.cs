﻿using System;

namespace MatrixProjection {

    // DEPRECATED (but has useful math)

    public struct Matrix3D {

        public float[,] Matrix { get; set; }

        public Matrix3D Inverse => GetInverse();

        public static Matrix3D VectToMat(Vector3 v) {

            return new Matrix3D() {

                Matrix = new float[3, 1] {
                    {v.X},
                    {v.Y},
                    {v.Z}
                }
            };
        }

        public static Vector3 MatToVec(Matrix3D m) {

            return new Vector3(m.Matrix[0, 0], 
                              m.Matrix[1, 0], 
                              m.Matrix.GetLength(0) > 2 ? m.Matrix[2, 0] : 0);
        }

        public static Vector3 MatMul(Vector3 v, Matrix3D m1) {

            Matrix3D m2 = VectToMat(v);

            Matrix3D m3 = MatMul(m1, m2);

            return MatToVec(m3);
        }

        public static Matrix3D MatMul(Matrix3D m1, Vector3 v) {

            Matrix3D m2 = VectToMat(v);

            return MatMul(m1, m2);
        }

        public static Vector3 MatMul4x4(Vector3 v, Matrix3D m) {

            Vector4 multVector = new Vector4(
                v.X * m.Matrix[0, 0] + v.Y * m.Matrix[0, 1] + v.Z * m.Matrix[0, 2] + m.Matrix[0, 3],
                v.X * m.Matrix[1, 0] + v.Y * m.Matrix[1, 1] + v.Z * m.Matrix[1, 2] + m.Matrix[1, 3],
                v.X * m.Matrix[2, 0] + v.Y * m.Matrix[2, 1] + v.Z * m.Matrix[2, 2] + m.Matrix[2, 3],
                v.X * m.Matrix[3, 0] + v.Y * m.Matrix[3, 1] + v.Z * m.Matrix[3, 2] + m.Matrix[3, 3]);

            return (Vector3)multVector;
        }

        public static Matrix3D MatMul(Matrix3D m1, Matrix3D m2) {

            int rowsM1 = m1.Matrix.GetLength(0);
            int colsM1 = m1.Matrix.GetLength(1);
            int rowsM2 = m2.Matrix.GetLength(0);
            int colsM2 = m2.Matrix.GetLength(1);

            if (colsM1 != rowsM2) {

                Console.Write("Columns of M1 MUST match Rows of M2");
                //return null;
            }

            float[,] newMatrix = new float[rowsM1, colsM2];

            for (int i = 0; i < rowsM1; i++) {

                for (int j = 0; j < colsM2; j++) {

                    float sum = 0;

                    for (int k = 0; k < colsM1; k++) {

                        sum += m1.Matrix[i, k] * m2.Matrix[k, j];
                    }

                    newMatrix[i, j] = sum;
                }
            }

            return new Matrix3D() { Matrix = newMatrix };
        }

        // Implement to 4x4 Matrix only?
        private Matrix3D GetInverse() {

            return new Matrix3D();
        }

        public override string ToString() {

            string mat = "";

            for (int i = 0; i < Matrix.GetLength(0); i++) {

                for (int j = 0; j < Matrix.GetLength(1); j++) {

                    mat += $"{Matrix[i, j]} ";
                }

                mat += '\n';
            }

            return mat;
        }
    }
}
