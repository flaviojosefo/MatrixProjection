using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatrixProjection {

    public class Scene {

        // Width / Height (Pixel ratio)
        private const float ASPECT_RATIO = 8 / 6.0f;

        private Camera camera;
        private DrawString draw;

        private readonly float deltaTime;

        private readonly Mesh mesh;

        private readonly float projectionScale;

        private float xAngle;
        private float yAngle;
        private float zAngle;

        private int cursorY = 2;

        private Thread input;

        private bool ortho;

        private bool isMesh;

        private bool loop = true;

        private bool rotate = true;
        private bool rotateX = false;
        private bool rotateY = true;
        private bool rotateZ = false;

        private Mat4x4 orthoProjection = new float[4, 4] {
                {1 * ASPECT_RATIO, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 1}
        };

        private Mat4x4 perspProjection;

        //Stopwatch time1 = new Stopwatch();

        public Scene(int frameRate, Mesh mesh, float projectionScale = 100.0f) {

            deltaTime = 1000 / frameRate;

            this.mesh = mesh;

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

            float fovRad = 1.0f / (float)Math.Tan(camera.Fov * 0.5f * (Math.PI / 180.0f));

            perspProjection = new float[4, 4] {
                {fovRad * ASPECT_RATIO,0,0,0},
                {0,fovRad,0,0},
                {0,0,-camera.FarPlane / (camera.FarPlane - camera.NearPlane),-(camera.FarPlane * camera.NearPlane) / (camera.FarPlane - camera.NearPlane)},
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

        // Render 3D Objects
        private void Render3D() {

            //Mat4x4 camTranform = Mat4x4.MatMul(Mat4x4.VecToMat(camera.Position), Mat4x4.VecToMat())

            Light simpleLight = new Light { Direction = new Vector(0, 0, 1) };
            simpleLight.Direction.Normalize();

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

            Mat4x4 mvp = ortho ? Mat4x4.MatMul(orthoProjection, camera.ViewMatrix) : Mat4x4.MatMul(perspProjection, camera.ViewMatrix);

            Triangle[] updatedTri = new Triangle[mesh.Polygons.Length];

            for (int i = 0; i < mesh.Polygons.Length; i++) {

                updatedTri[i] = new Triangle(new Vector[mesh.Polygons[i].VertexCount]);

                for (int j = 0; j < mesh.Polygons[i].VertexCount; j++) {

                    // Apply rotation to vertex
                    Vector rotated = Mat4x4.MatMul(mesh.Polygons[i][j], rotMatrix);

                    // Translate vertex (slightly) to not draw on top of camera
                    Vector translated = new Vector(rotated.X, rotated.Y, rotated.Z + 3.0f);

                    updatedTri[i][j] = translated;
                }

                float lightDP = Math.Max(0.1f, Vector.DotProduct(updatedTri[i].Normal, simpleLight.Direction));
                ShadeTri(ref updatedTri[i], lightDP);
            }

            Triangle[] projected = new Triangle[mesh.Polygons.Length];

            for (int i = 0; i < projected.Length; i++) {

                projected[i] = updatedTri[i];

                for (int j = 0; j < projected[i].VertexCount; j++) {

                    projected[i][j] = Mat4x4.MatMul(projected[i][j], mvp);

                    // Scale Vectors
                    projected[i][j] *= ortho ? 20.0f : projectionScale;  // Magic numbers :(
                }
            }

            if (rotate) {

                if (rotateX) xAngle -= 0.03f;
                if (rotateY) yAngle -= 0.03f;
                if (rotateZ) zAngle -= 0.03f;
            }

            if (isMesh) {

                draw.PlotMesh(projected);

            } else {

                draw.PlotFaces(projected);
            }
        }

        private void ShadeTri(ref Triangle tri, float dotProduct) {

            //if (dotProduct < 0.0f) return ShadeChar.Null;

            if (dotProduct <= 0.1f) {

                tri.Color = ConsoleColor.DarkGray;
                tri.Symbol = ShadeChar.Low;

            } else if (dotProduct < 0.5f) {

                tri.Color = ConsoleColor.Gray;
                tri.Symbol = ShadeChar.Medium;

            } else if (dotProduct < 0.7f) {

                tri.Color = ConsoleColor.Gray;
                tri.Symbol = ShadeChar.High;

            } else {

                tri.Color = ConsoleColor.White;
                tri.Symbol = ShadeChar.Full;
            }
        }

        

        // Render 2nd (on top)
        private void RenderUI() {

            string[] menu = new string[12];

            menu[0] = "■----------------------■";
            menu[1] = "|                      |";
            menu[2] = $"|      Ortho    [{(ortho ? 'X' : ' ')}]    |";
            menu[3] = $"|      Mesh     [{(isMesh ? 'X' : ' ')}]    |";
            menu[4] = $"|      Rotate   [{(rotate ? 'X' : ' ')}]    |";
            menu[5] = $"|      Rotate X [{(rotateX ? 'X' : ' ')}]    |";
            menu[6] = $"|      Rotate Y [{(rotateY ? 'X' : ' ')}]    |";
            menu[7] = $"|      Rotate Z [{(rotateZ ? 'X' : ' ')}]    |";
            menu[8] = "|      Reset           |";
            menu[9] = "|      Back            |";
            menu[10] = "|                      |";
            menu[11] = "■----------------------■";

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
                    cursorY = cursorY > 2 ? cursorY - 1 : 9;
                    break;

                case ConsoleKey.DownArrow:
                    cursorY = cursorY < 9 ? cursorY + 1 : 2;
                    break;

                case ConsoleKey.Enter:
                    SelectOption();
                    break;

                case ConsoleKey.W:
                    camera.Position += camera.Forward * 0.05f;   // Forward -> Going towards +Z
                    break;

                case ConsoleKey.A:
                    camera.Position -= camera.Right * 0.05f;     // Left
                    break;

                case ConsoleKey.S:
                    camera.Position -= camera.Forward * 0.05f;   // Back
                    break;

                case ConsoleKey.D:
                    camera.Position += camera.Right * 0.05f;     // Right
                    break;

                case ConsoleKey.E:
                    camera.Position += camera.Up * 0.05f;        // Up
                    break;

                case ConsoleKey.Q:
                    camera.Position -= camera.Up * 0.05f;        // Down
                    break;

                case ConsoleKey.NumPad4:
                    camera.Yaw -= 2.0f;
                    break;

                case ConsoleKey.NumPad6:
                    camera.Yaw += 2.0f;
                    break;

                case ConsoleKey.NumPad5:
                    camera.Pitch -= 2.0f;
                    break;

                case ConsoleKey.NumPad8:
                    camera.Pitch += 2.0f;
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
                    isMesh = !isMesh;
                    break;

                case 3:
                    rotate = !rotate;
                    break;

                case 4:
                    rotateX = !rotateX;
                    break;

                case 5:
                    rotateY = !rotateY;
                    break;

                case 6:
                    rotateZ = !rotateZ;
                    break;

                case 7:
                    xAngle = yAngle = zAngle = 0.0f; // Reset

                    camera.Position = new Vector();
                    camera.Yaw = camera.Pitch = camera.Roll = 0;
                    break;

                case 8:
                    loop = false;
                    input.Abort();
                    break;
            }
        }
    }
}
