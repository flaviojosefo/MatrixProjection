using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            int frameRate = 60;
            int deltaTime = 1000 / frameRate;

            Draw draw = new Draw(120, 50, false);

            Vector[] cube = new Vector[8];

            cube[0] = new Vector(10, 10, 10);
            cube[1] = new Vector(-10, 10, 10);
            cube[2] = new Vector(-10, -10, 10);
            cube[3] = new Vector(10, -10, 10);
            cube[4] = new Vector(10, 10, -10);
            cube[5] = new Vector(-10, 10, -10);
            cube[6] = new Vector(-10, -10, -10);
            cube[7] = new Vector(10, -10, -10);

            Vector[] projected = new Vector[cube.Length];

            Matrix3D orthoProjection = new Matrix3D() {

                Matrix = new float[2, 3] {
                    {1, 0, 0},
                    {0, 1, 0}
                }
            };

            float angle = 0;

            // Loop
            while (true) {

                Matrix3D rotationX = new Matrix3D() {

                    Matrix = new float[3, 3] {
                        {1, 0, 0},
                        {0, (float)Math.Cos(angle), (float)-Math.Sin(angle)},
                        {0, (float)Math.Sin(angle), (float)Math.Cos(angle)}
                    }
                };

                Matrix3D rotationY = new Matrix3D() {

                    Matrix = new float[3, 3] {
                        {(float)Math.Cos(angle), 0, (float)Math.Sin(angle)},
                        {0, 1, 0},
                        {(float)-Math.Sin(angle), 0, (float)Math.Cos(angle)}
                    }
                };

                Matrix3D rotationZ = new Matrix3D() {

                    Matrix = new float[3, 3] {
                        {(float)Math.Cos(angle), (float)-Math.Sin(angle), 0},
                        {(float)Math.Sin(angle), (float)Math.Cos(angle), 0},
                        {0, 0, 1}
                    }
                };

                for (int i = 0; i < cube.Length / 2; i++) {

                    draw.DrawLine(projected[i], projected[(i + 1) % 4], false);
                    draw.DrawLine(projected[i + 4], projected[((i + 1) % 4) + 4], false);
                    draw.DrawLine(projected[i], projected[i + 4], false);
                }

                for (int i = 0; i < cube.Length; i++) {

                    Vector rotated = Matrix3D.MatMul(cube[i], rotationY);
                    rotated = Matrix3D.MatMul(rotated, rotationX);
                    rotated = Matrix3D.MatMul(rotated, rotationZ);
                    projected[i] = Matrix3D.MatMul(rotated, orthoProjection);
                }

                for (int i = 0; i < cube.Length / 2; i++) {

                    draw.DrawLine(projected[i], projected[(i + 1) % 4]);
                    draw.DrawLine(projected[i + 4], projected[((i + 1) % 4) + 4]);
                    draw.DrawLine(projected[i], projected[i + 4]);
                }

                angle -= 0.01f;

                Thread.Sleep(deltaTime);
            }
        }
    }
}
