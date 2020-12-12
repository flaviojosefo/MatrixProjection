using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            //Stopwatch time1 = new Stopwatch();

            int frameRate = 60;
            float deltaTime = (1000 / frameRate) - 13;  // Average calculation time

            Draw draw = new Draw(120, 50, false);
            Vector[] cube = new Vector[8];

            cube[0] = new Vector(0.5f, 0.5f, 0.5f);
            cube[1] = new Vector(-0.5f, 0.5f, 0.5f);
            cube[2] = new Vector(-0.5f, -0.5f, 0.5f);
            cube[3] = new Vector(0.5f, -0.5f, 0.5f);
            cube[4] = new Vector(0.5f, 0.5f, -0.5f);
            cube[5] = new Vector(-0.5f, 0.5f, -0.5f);
            cube[6] = new Vector(-0.5f, -0.5f, -0.5f);
            cube[7] = new Vector(0.5f, -0.5f, -0.5f);

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

                //time1.Restart();

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

                // Erase
                for (int i = 0; i < cube.Length / 2; i++) {

                    draw.DrawLine(projected[i], projected[(i + 1) % 4], false);
                    draw.DrawLine(projected[i + 4], projected[((i + 1) % 4) + 4], false);
                    draw.DrawLine(projected[i], projected[i + 4], false);
                }

                // Calculate
                for (int i = 0; i < cube.Length; i++) {

                    // XYZ rotation = (((Z * Y) * X) * Vector)
                    Matrix3D ZY = Matrix3D.MatMul(rotationZ, rotationY);
                    Matrix3D fullRot = Matrix3D.MatMul(ZY, rotationX);
                    Vector rotated = Matrix3D.MatMul(cube[i], fullRot);

                    float distance = 1.5f;
                    float z = 1.0f / (distance - rotated.Z);

                    Matrix3D perspProjection = new Matrix3D() {

                        Matrix = new float[2, 3] {
                            {z, 0, 0},
                            {0, z, 0},
                        }
                    };

                    projected[i] = Matrix3D.MatMul(rotated, perspProjection);

                    // Scale Vectors
                    projected[i] *= 20.0f;
                }

                // Draw
                for (int i = 0; i < cube.Length / 2; i++) {

                    draw.DrawLine(projected[i], projected[(i + 1) % 4]);
                    draw.DrawLine(projected[i + 4], projected[((i + 1) % 4) + 4]);
                    draw.DrawLine(projected[i], projected[i + 4]);
                }

                angle -= 0.01f;

                //time1.Stop();
                //Console.SetCursorPosition(1, 0);
                //Console.Write(' ');
                //Console.SetCursorPosition(0, 0);
                //Console.Write(time1.ElapsedMilliseconds);

                Thread.Sleep((int)deltaTime);
            }
        }
    }
}
