using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Matrix3D {

        public float[,] Matrix { get; set; }

        public Matrix3D() { }

        public static Matrix3D MatMul(Matrix3D m1, Matrix3D m2) {

            int rowsM1 = m1.Matrix.GetLength(0);
            int colsM1 = m1.Matrix.GetLength(1);
            int rowsM2 = m2.Matrix.GetLength(0);
            int colsM2 = m2.Matrix.GetLength(1);

            if (colsM1 != rowsM2) {

                Console.Write("Columns of M1 MUST match rows of M2");
                return null;
            }

            float[,] newMatrix = new float[rowsM1,colsM2];

            for (int i = 0; i < rowsM1; i++) {

                for (int j = 0; j < colsM2; j++) {

                    float sum = 0;

                    for (int k = 0; k < colsM1; k++) {

                        sum += m1.Matrix[i, k] * m2.Matrix[k, j];
                    }

                    newMatrix[i, j] = sum;
                }
            }

            return new Matrix3D() {

                Matrix = newMatrix
            };
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
