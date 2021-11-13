using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatrixProjection {

    public class Scene {

        private Camera camera;
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

        private Mat4x4 orthoProjection = new float[4, 4] {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 1}
        };

        private Mat4x4 perspProjection;

        //Stopwatch time1 = new Stopwatch();

        public Scene(int frameRate, Mesh mesh, float projectionScale = 20.0f) {

            deltaTime = 1000 / frameRate;

            this.mesh = mesh;

            projected = new Triangle[mesh.Polygons.Length];

            this.projectionScale = projectionScale;
        }

        public void Start() {

            camera = new Camera();
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

            perspProjection = new float[4, 4] {
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

            //Mat4x4 camTranform = Mat4x4.MatMul(Mat4x4.VecToMat(camera.Position), Mat4x4.VecToMat())

            Light simpleLight = new Light(new Vector(2, 0, 0));

            Mat4x4 rotationX = new float[4, 4] {
                {1, 0, 0, 0},
                {0, (float)Math.Cos(xAngle), (float)-Math.Sin(xAngle), 0},
                {0, (float)Math.Sin(xAngle), (float)Math.Cos(xAngle), 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotationY = new float[4, 4] {
                {(float)Math.Cos(yAngle), 0, (float)Math.Sin(yAngle), 0},
                {0, 1, 0, 0},
                {(float)-Math.Sin(yAngle), 0, (float)Math.Cos(yAngle), 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotationZ = new float[4, 4] {
                {(float)Math.Cos(zAngle), (float)-Math.Sin(zAngle), 0, 0},
                {(float)Math.Sin(zAngle), (float)Math.Cos(zAngle), 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            // XYZ rotation = (((Z * Y) * X) * Vector) or (Z×Y×X)×V
            Mat4x4 rotMatrix = Mat4x4.MatMul(rotationZ, rotationY);
            rotMatrix = Mat4x4.MatMul(rotMatrix, rotationX);

            for (int i = 0; i < mesh.Polygons.Length; i++) {

                projected[i] = new Triangle(new Vector[mesh.Polygons[i].VertexCount]);

                for (int j = 0; j < mesh.Polygons[i].VertexCount; j++) {

                    // Apply rotation to vertex
                    Vector rotated = Mat4x4.MatMul(mesh.Polygons[i].Vertices[j], rotMatrix);

                    // Translate vertex (slightly) to not draw on top of camera
                    Vector translated = new Vector(rotated.X, rotated.Y, rotated.Z + 1.2f);

                    Mat4x4 mvp = ortho ? Mat4x4.MatMul(orthoProjection, camera.ViewMatrix) : Mat4x4.MatMul(perspProjection, camera.ViewMatrix);

                    projected[i].Vertices[j] = Mat4x4.MatMul(translated, mvp);

                    // Scale Vectors
                    projected[i].Vertices[j] *= projectionScale;
                }
            }

            if (rotate) {

                if (rotateX) xAngle -= 0.01f;
                if (rotateY) yAngle -= 0.01f;
                if (rotateZ) zAngle -= 0.01f;
            }

            //draw.PlotShadedFaces(projected, simpleLight);
            //draw.PlotFaces(projected);
            draw.PlotMesh(projected);
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

            string[] camInfo = new string[6];

            camInfo[0] = "■-----------------------------■";
            camInfo[1] = "|                             |";
            camInfo[2] = "|     WASD to Move Camera     |";
            camInfo[3] = "|   E and Q for Up and Down   |";
            camInfo[4] = "|                             |";
            camInfo[5] = "■-----------------------------■";

            for (int i = 0; i < camInfo.Length; i++) {

                int k = Console.WindowHeight - 1 + i - camInfo.Length;
                draw.AddText(new Vector(0, k), camInfo[i]);
            }

            draw.AddText(new Vector(5, cursorY), '►');
        }

        private void ManageInput() {

            switch (Console.ReadKey(true).Key) {

                case ConsoleKey.UpArrow:
                    cursorY = cursorY > 2 ? cursorY - 1 : 8;
                    break;

                case ConsoleKey.DownArrow:
                    cursorY = cursorY < 8 ? cursorY + 1 : 2;
                    break;

                case ConsoleKey.Enter:
                    SelectOption();
                    break;

                case ConsoleKey.W:
                    camera.Translate(new Vector(0, 0, 0.05f));    // Forward
                    break;

                case ConsoleKey.A:
                    camera.Translate(new Vector(-0.05f, 0, 0));    // Left
                    break;

                case ConsoleKey.S:
                    camera.Translate(new Vector(0, 0, -0.05f));     // Back
                    break;

                case ConsoleKey.D:
                    camera.Translate(new Vector(0.05f, 0, 0));     // Right
                    break;

                case ConsoleKey.E:
                    camera.Translate(new Vector(0, 0.05f, 0));     // Up
                    break;

                case ConsoleKey.Q:
                    camera.Translate(new Vector(0, -0.05f, 0));    // Down
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
