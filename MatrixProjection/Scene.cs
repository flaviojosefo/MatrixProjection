using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatrixProjection {

    public class Scene {

        private DrawString draw;

        private readonly float deltaTime;

        private readonly Mesh mesh;

        private readonly Vector[][] projected;
        private readonly float projectionScale;

        private float xAngle;
        private float yAngle;
        private float zAngle;

        private int cursorY = 2;

        private Thread input;

        private bool ortho;

        private bool loop = true;

        private bool rotate = true;
        private bool rotateX = true;
        private bool rotateY = true;
        private bool rotateZ = true;

        Matrix3D orthoProjection = new Matrix3D() {

            Matrix = new float[2, 3] {
                {1, 0, 0},
                {0, 1, 0},
            }
        };

        //Stopwatch time1 = new Stopwatch();

        public Scene(int frameRate, Mesh mesh, float projectionScale = 20.0f) {

            deltaTime = 1000 / frameRate;

            this.mesh = mesh;

            projected = new Vector[mesh.Polygons.Length][];

            this.projectionScale = projectionScale;
        }

        public void Start() {

            draw = new DrawString();

            input = new Thread(ManageInput);
            input.Start();
        }

        public void Update() {

            // Loop
            while (loop) {

                //time1.Restart();

                draw.NewFrame();

                Render3D();
                RenderUI();

                draw.DrawFrame();

                //time1.Stop();
                //Console.SetCursorPosition(1, 0);
                //Console.Write(' ');
                //Console.SetCursorPosition(0, 0);
                //Console.Write(time1.ElapsedMilliseconds);

                Thread.Sleep((int)deltaTime);
            }
        }

        // Render Meshes, not Shapes
        private void Render3D() {

            if (rotate) {

                Matrix3D rotationX = new Matrix3D() {

                    Matrix = new float[3, 3] {
                        {1, 0, 0},
                        {0, (float)Math.Cos(xAngle), (float)-Math.Sin(xAngle)},
                        {0, (float)Math.Sin(xAngle), (float)Math.Cos(xAngle)}
                    }
                };

                Matrix3D rotationY = new Matrix3D() {

                    Matrix = new float[3, 3] {
                        {(float)Math.Cos(yAngle), 0, (float)Math.Sin(yAngle)},
                        {0, 1, 0},
                        {(float)-Math.Sin(yAngle), 0, (float)Math.Cos(yAngle)}
                    }
                };

                Matrix3D rotationZ = new Matrix3D() {

                    Matrix = new float[3, 3] {
                        {(float)Math.Cos(zAngle), (float)-Math.Sin(zAngle), 0},
                        {(float)Math.Sin(zAngle), (float)Math.Cos(zAngle), 0},
                        {0, 0, 1}
                    }
                };

                // XYZ rotation = (((Z * Y) * X) * Vector) or (Z×Y×X)×V
                Matrix3D rotMatrix = Matrix3D.MatMul(rotationZ, rotationY);
                rotMatrix = Matrix3D.MatMul(rotMatrix, rotationX);

                for (int i = 0; i < mesh.Polygons.Length; i++) {

                    projected[i] = new Vector[mesh.Polygons[i].Length];

                    for (int j = 0; j < mesh.Polygons[i].Length; j++) {

                        Vector rotated = Matrix3D.MatMul(mesh.Polygons[i][j], rotMatrix);

                        float distance = 1.5f;
                        float z = 1.0f / (distance - rotated.Z);

                        Matrix3D perspProjection = new Matrix3D() {

                            Matrix = new float[2, 3] {
                                {z, 0, 0},
                                {0, z, 0},
                            }
                        };

                        projected[i][j] = ortho ? Matrix3D.MatMul(rotated, orthoProjection) : Matrix3D.MatMul(rotated, perspProjection);

                        // Scale Vectors
                        projected[i][j] *= projectionScale;
                    }
                }

                if (rotateX) xAngle -= 0.01f;
                if (rotateY) yAngle -= 0.01f;
                if (rotateZ) zAngle -= 0.01f;
            }

            draw.AddMesh(projected);
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

                draw.AddText(new Vector(0, i), menu[i]);
            }

            draw.AddText(new Vector(5, cursorY), '►');
        }

        private void ManageInput() {

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
                    xAngle = yAngle = zAngle = 0.0f; // Reset
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
