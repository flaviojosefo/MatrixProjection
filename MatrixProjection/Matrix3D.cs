using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Matrix3D {

        public float[,] Matrix { get; set; }

        public Matrix3D() { }

        public override string ToString() {

            string mat = "";

            for (int i = 0; i < Matrix.GetLength(0); i++) {

                for (int j = 0; j < Matrix.GetLength(1); j++) {

                    mat += $"{Matrix[i, j]}, ";
                }

                mat += '\n';
            }

            return mat;
        }
    }
}
