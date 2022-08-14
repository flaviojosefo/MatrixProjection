using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MatrixProjection {

    public class Scene {

        private Camera camera;
        private Light light;

        private IRenderer renderer;
        private FrameBuffer frameBuffer;

        private readonly float deltaTime;

        private readonly RenderObject rObject;

        private int cursorY = 2;

        private Thread input;
        private Thread inputThread;

        private bool loop = true;

        private bool rotate = true;
        private bool rotateX = false;
        private bool rotateY = true;
        private bool rotateZ = false;

        //Stopwatch timer = new Stopwatch();

        public Scene(int frameRate, RenderObject rObject) {

            deltaTime = 1000 / frameRate;

            this.rObject = rObject;
        }

        public void Start() {

            camera = new Camera();
            light = new Light { Direction = new Vector3(0, 0, 1) };

            renderer = new Rasterizer(camera, light);
            frameBuffer = new FrameBuffer();

            input = new Thread(ManageInput);
            input.Start();

            // Move object (slightly) back to not draw on top of camera
            rObject.Transform.Move(new Vector3(0, 0, 3));
        }

        public void Update() {

            // Loop
            while (loop) {

                //time1.Restart();

                frameBuffer.NewFrame();

                Render3D();
                RenderUI();

                frameBuffer.DrawFrame();

                //timer.Stop();
                //Console.SetCursorPosition(1, 0);
                //Console.Write(' ');
                //Console.SetCursorPosition(0, 0);
                //Console.Write(timer.ElapsedMilliseconds);

                Thread.Sleep((int)deltaTime);
            }
        }

        // Render 3D Objects
        private void Render3D() {

            renderer.Render(rObject);

            frameBuffer.GetFragmentData(renderer.Fragments);

            if (rotate) {

                if (rotateX) rObject.Transform.Rotate(new Vector3(-0.04f, 0, 0));
                if (rotateY) rObject.Transform.Rotate(new Vector3(0, -0.04f, 0));
                if (rotateZ) rObject.Transform.Rotate(new Vector3(0, 0, -0.04f));
            }
        }

        // Render 2nd (on top)
        private void RenderUI() {

            string[] menu = new string[14];

            menu[0] = "■----------------------■";
            menu[1] = "|                      |";
            menu[2] = $"|      Ortho    [{(camera.IsOrthographic() ? 'X' : ' ')}]    |";
            menu[3] = $"|      Verts    [{(renderer.RenderMode == RenderMode.Vertices ? 'X' : ' ')}]    |";
            menu[4] = $"|      Mesh     [{(renderer.RenderMode == RenderMode.Mesh ? 'X' : ' ')}]    |";
            menu[5] = $"|      Surface  [{(renderer.RenderMode == RenderMode.Solid ? 'X' : ' ')}]    |";
            menu[6] = $"|      Rotate   [{(rotate ? 'X' : ' ')}]    |";
            menu[7] = $"|      Rotate X [{(rotateX ? 'X' : ' ')}]    |";
            menu[8] = $"|      Rotate Y [{(rotateY ? 'X' : ' ')}]    |";
            menu[9] = $"|      Rotate Z [{(rotateZ ? 'X' : ' ')}]    |";
            menu[10] = "|      Reset           |";
            menu[11] = "|      Back            |";
            menu[12] = "|                      |";
            menu[13] = "■----------------------■";

            for (int i = 0; i < menu.Length; i++) {

                frameBuffer.AddText(new Vector3(0, i), menu[i]);
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
                frameBuffer.AddText(new Vector3(0, k), camInfo[i]);
            }

            frameBuffer.AddText(new Vector3(5, cursorY), '►');
        }

        private void ManageInput() {

            switch (Console.ReadKey(true).Key) {

                case ConsoleKey.UpArrow:
                    cursorY = cursorY > 2 ? cursorY - 1 : 11;
                    break;

                case ConsoleKey.DownArrow:
                    cursorY = cursorY < 11 ? cursorY + 1 : 2;
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
                    camera.Yaw -= 2.0f;                          // Turn Left
                    break;

                case ConsoleKey.NumPad6:
                    camera.Yaw += 2.0f;                          // Turn Right
                    break;

                case ConsoleKey.NumPad5:
                    camera.Pitch += 2.0f;                        // Turn Up
                    break;

                case ConsoleKey.NumPad8:
                    camera.Pitch -= 2.0f;                        // Turn Down
                    break;
            }

            ManageInput();
        }

        private void SelectOption() {

            switch (cursorY - 1) {

                case 1:
                    camera.Projection = camera.IsOrthographic() ? Projection.Perspective : Projection.Ortographic;
                    break;

                case 2:
                    renderer.RenderMode = RenderMode.Vertices;
                    break;

                case 3:
                    renderer.RenderMode = RenderMode.Mesh;
                    break;

                case 4:
                    renderer.RenderMode = RenderMode.Solid;
                    break;

                case 5:
                    rotate = !rotate;
                    break;

                case 6:
                    rotateX = !rotateX;
                    break;

                case 7:
                    rotateY = !rotateY;
                    break;

                case 8:
                    rotateZ = !rotateZ;
                    break;

                case 9:
                    rObject.Transform.Rotation = new Vector3(); // Reset

                    camera.Position = new Vector3();
                    camera.Yaw = camera.Pitch = camera.Roll = 0;
                    break;

                case 10:
                    loop = false;
                    input.Abort();
                    break;
            }
        }
    }
}
