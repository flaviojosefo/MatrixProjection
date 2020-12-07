using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            Draw draw = new Draw(120, 50, false);

            Vector v1 = new Vector(0, 0, 0);
            Vector v2 = new Vector(5, 5, 0); 
            Vector v3 = new Vector(5, -5, 0);
            Vector v4 = new Vector(-5, -5, 0);
            Vector v5 = new Vector(-5, 5, 0);

            Matrix3D m1 = new Matrix3D() {

                Matrix = new float[3, 3] {
                    {1, 2, 1},
                    {0, 1, 0},
                    {2, 3, 4}
                }
            };

            Matrix3D m2 = new Matrix3D() {

                Matrix = new float[3, 2] {
                    {2, 5},
                    {6, 7},
                    {1, 8}
                }
            };

            //Console.WriteLine(v1);
            //Console.WriteLine();
            //Console.WriteLine(m1);

            //draw.DrawPoint(v1);
            //draw.DrawPoint(v2);
            //draw.DrawPoint(v3);
            //draw.DrawPoint(v4);
            //draw.DrawPoint(v5);

            //draw.DrawLine(v2, v4);
            //draw.DrawLine(v3, v5);

            Console.WriteLine(Matrix3D.MatMul(m1, m2));

            Console.ReadKey();
        }
    }
}
