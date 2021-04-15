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

        private readonly Triangle[] projected;
        private readonly float projectionScale;

        private float xAngle;
        private float yAngle;
        private float zAngle;

        private int cursorY = 2;

        private Thread input;

        private bool ortho;

        private bool loop = true;

        private bool rotate = true;
        private bool rotateX = false;
        private bool rotateY = true;
        private bool rotateZ = false;

        private readonly Matrix3D orthoProjection = new Matrix3D() {

            Matrix = new float[2, 3] {
                {1, 0, 0},
                {0, 1, 0},
            }
        };

        private readonly Matrix3D perspProjection = new Matrix3D() { Matrix = new float[4, 4] };

        //Stopwatch time1 = new Stopwatch();

        public Scene(int frameRate, Mesh mesh, float projectionScale = 20.0f) {

            deltaTime = 1000 / frameRate;

            this.mesh = mesh;

            projected = new Triangle[mesh.Polygons.Length];

            this.projectionScale = projectionScale;
        }

        public void Start() {

            draw = new DrawString();

            input = new Thread(ManageInput);
            input.Start();

            // Perspective Projection

            /*Sources:
             https://stackoverflow.com/questions/53245632/general-formula-for-perspective-projection-matrix
             https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/building-basic-perspective-projection-matrix
             */

            float nearPlane = 0.1f;
            float farPlane = 100.0f;
            float fov = 60.0f;
            float fovRad = 1.0f / (float)Math.Tan(fov * 0.5f * (Math.PI / 180.0f));

            perspProjection.Matrix = new float[4, 4] {
                {fovRad,0,0,0},
                {0,fovRad,0,0},
                {0,0,-farPlane / (farPlane - nearPlane),-(farPlane * nearPlane) / (farPlane - nearPlane)},
                {0,0,1,0}
            };

            // ######################
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

                projected[i] = new Triangle(new Vector[mesh.Polygons[i].VertexCount]);

                for (int j = 0; j < mesh.Polygons[i].VertexCount; j++) {

                    // Apply rotation to vertex
                    Vector rotated = Matrix3D.MatMul(mesh.Polygons[i].Vertices[j], rotMatrix);

                    // Translate vertex (slightly) to not draw on top of camera
                    Vector translated = new Vector(rotated.X, rotated.Y, rotated.Z + 1.2f);

                    projected[i].Vertices[j] = ortho ? Matrix3D.MatMul(translated, orthoProjection) : Matrix3D.MatMul4x4(translated, perspProjection);

                    // Scale Vectors
                    projected[i].Vertices[j] *= projectionScale;
                }
            }

            if (rotate) {

                if (rotateX) xAngle -= 0.01f;
                if (rotateY) yAngle -= 0.01f;
                if (rotateZ) zAngle -= 0.01f;
            }

            draw.PlotFaces(projected);
            //draw.PlotMesh(projected);
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
