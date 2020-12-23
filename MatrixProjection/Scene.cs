using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatrixProjection {

    public class Scene {

        private Draw draw;

        private readonly float deltaTime;

        private readonly Shape shape;

        private Vector[] projected;

        private float angle;

        private int cursorY = 2;

        private Thread input;

        private bool loop = true;

        private bool ortho;

        private bool rotate = true;
        private bool rotateX = true;
        private bool rotateY = true;
        private bool rotateZ = true;

        Matrix3D orthopProjection = new Matrix3D() {

            Matrix = new float[2, 3] {
                {1, 0, 0},
                {0, 1, 0},
            }
        };

        //Stopwatch time1 = new Stopwatch();

        public Scene(int frameRate, Shape shape) {

            deltaTime = (1000 / frameRate) - 13;  // Average calculation time

            this.shape = shape;

            projected = new Vector[shape.Vertices.Length];
        }

        public void Start() {

            draw = new Draw();

            input = new Thread(ManageInput);
            input.Start();
        }

        public void Update() {

            // Loop
            while (loop) {

                //time1.Restart();

                Render3D();
                RenderUI();

                //time1.Stop();
                //Console.SetCursorPosition(1, 0);
                //Console.Write(' ');
                //Console.SetCursorPosition(0, 0);
                //Console.Write(time1.ElapsedMilliseconds);

                Thread.Sleep((int)deltaTime);
            }
        }

        // Render 1st
        private void Render3D() {

            if (!rotate) return;

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
            shape.DrawShape(draw, projected, false);

            // Calculate
            for (int i = 0; i < shape.Vertices.Length; i++) {

                // XYZ rotation = (((Z * Y) * X) * Vector)
                Matrix3D ZY = Matrix3D.MatMul(rotationZ, rotationY);
                Matrix3D fullRot = Matrix3D.MatMul(ZY, rotationX);
                Vector rotated = Matrix3D.MatMul(shape.Vertices[i], fullRot);

                float distance = 1.5f;
                float z = 1.0f / (distance - rotated.Z);

                Matrix3D perspProjection = new Matrix3D() {

                    Matrix = new float[2, 3] {
                        {z, 0, 0},
                        {0, z, 0},
                    }
                };

                projected[i] = ortho ? Matrix3D.MatMul(rotated, orthopProjection) : Matrix3D.MatMul(rotated, perspProjection);

                // Scale Vectors
                projected[i] *= 20.0f;
            }

            // Draw
            shape.DrawShape(draw, projected);

            angle -= 0.01f;
        }

        // Render 2nd
        private void RenderUI() {

            string[] menu = new string[11];

            menu[0] = "■----------------------■";
            menu[1] = "|                      |";
            menu[2] = $"|      Ortho    [{(ortho ? 'X' : ' ')}]    |";
            menu[3] = $"|      Rotate   [{(rotate ? 'X' : ' ')}]    |";
            menu[4] = $"|      Rotate X [{(rotateX ? 'X' : ' ')}]    |";
            menu[5] = $"|      Rotate Y [{(rotateY ? 'X' : ' ')}]    |";
            menu[6] = $"|      Rotate Z [{(rotateZ ? 'X' : ' ')}]    |";
            menu[7] = "|      Reset           |";
            menu[8] = "|      Back            |";
            menu[9] = "|                      |";
            menu[10] = "■----------------------■";

            for (int i = 0; i < menu.Length; i++) {

                Console.SetCursorPosition(0, i);
                Console.Write(menu[i]);
            }

            Console.SetCursorPosition(5, cursorY);
            Console.Write('►');
        }

        private void ManageInput() {

            //Console.Clear(); // Fixes weird writing issue

            switch (Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    cursorY = cursorY > 2 ? cursorY - 1 : 8;
                    break;

                case ConsoleKey.DownArrow:
                    cursorY = cursorY < 8 ? cursorY + 1 : 2;
                    break;

                case ConsoleKey.Enter:
                    SelectOption();
                    break;
            }
            ManageInput();
        }

        private void SelectOption() {

            switch (cursorY - 1) {

                case 1:
                    ortho = !ortho;
                    break;

                case 2:
                    rotate = !rotate;
                    break;

                case 3:
                    rotateX = !rotateX;
                    break;

                case 4:
                    rotateY = !rotateY;
                    break;

                case 5:
                    rotateZ = !rotateZ;
                    break;

                case 6:
                    angle = 0; // Reset
                    rotate = true;
                    break;

                case 7:
                    loop = false;
                    input.Abort();
                    break;
            }
        }
    }
}
