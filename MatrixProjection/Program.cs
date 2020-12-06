using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            Draw draw = new Draw(120, 50, false);

            Vector v1 = new Vector(1, 0, 2);

            Matrix3D m1 = new Matrix3D() {

                Matrix = new float[3, 3] {
                    {1, 0, 0},
                    {0, 1, 0},
                    {0, 0, 1}
                }
            };

            Console.WriteLine(v1);
            Console.WriteLine();
            Console.WriteLine(m1);

            Console.ReadKey();
        }
    }
}
