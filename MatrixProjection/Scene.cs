using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MatrixProjection {

    public class Scene {

        private Camera camera;
        private Light light;

        private IRenderer renderer;
        private FrameBuffer frameBuffer;

        private readonly RenderObject rObject;

        private int cursorY = 2;

        private Thread input;
        private Thread inputThread;

        private bool loop = true;

        private bool rotate = true;
        private bool rotateX = false;
        private bool rotateY = true;
        private bool rotateZ = false;

        private readonly Stopwatch timer = new Stopwatch();
        // https://www.youtube.com/watch?v=lW6ZtvQVzyg
        // https://stackoverflow.com/questions/26110228/c-sharp-delta-time-implementation#:~:text=DeltaTime%20like%20in,2493331%7D%0A%20%20%20%20%20%20%20%20%20%20%20%20time1%20%3D%20time2%3B%0A%20%20%20%20%20%20%20%20%7D%0A%20%20%20%20%7D%0A%7D

        public Scene(RenderObject rObject) {

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

        // The method containing the Scene's loop
        // https://gafferongames.com/post/fix_your_timestep/
        public void Run() {

            int fps = 0;                                       // The Frames p/ Second value (only counts display frames)
            int fpsCounter = 0;                                // The Frames counter
            double fpsStart = 0.0d;                            // The time at which fps should start after reset

            int fpsInterval = 1;                               // The interval of time (in seconds) to update fps

            double t = 0.0d;                                   // The total time since the start of the loop
            double dt = 1 / 60.0d;                             // The upper bound for delta time

            Stopwatch timer = Stopwatch.StartNew();            // Start the timer
            double currentTime = timer.Elapsed.TotalSeconds;   // The current (time in seconds)

            // The 'game' loop
            while (loop) {

                double newTime = timer.Elapsed.TotalSeconds;   // The time at the start of the frame
                double frameTime = newTime - currentTime;      // The diff in time between the previous and current frame
                currentTime = newTime;                         // Update the current time

                // Allow multiple updates within 1 frame (if possible)
                while (frameTime > 0.0d) {

                    // Calculate the delta time
                    float deltaTime = (float)Math.Min(frameTime, dt);

                    // Update logic
                    Update(deltaTime);

                    // Decrease frame time
                    frameTime -= deltaTime;

                    // Increase the total time
                    t += deltaTime;
                }

                // Render the scene
                Draw();

                // Increase the fps counter by 1
                fpsCounter++;

                // Calculate if enough time has passed in order to update the fps
                if (t - fpsStart >= fpsInterval) {

                    // Assign the current time as the new "start"
                    fpsStart = t;

                    // Divide the counter by the seconds interval (as we want frames per SECOND)
                    fps = (int)(fpsCounter / (float)fpsInterval);

                    // Reset the fps counter
                    fpsCounter = 0;
                }

                // Display current time and fps
                //Console.Write((int)t + " | " + fps);
            }
        }

        // Process scene objects
        private void Update(float deltaTime) {

            // Rotate the current render object
            if (rotate) {

                Vector3 rotation = Vector3.Zero;

                if (rotateX)
                    rotation += new Vector3(-90f, 0, 0);  // Rotate 90 Deg downwards

                if (rotateY)
                    rotation += new Vector3(0, -90f, 0);  // Rotate 90 Deg rightwards

                if (rotateZ)
                    rotation += new Vector3(0, 0, -90f);  // Rotate 90 Deg rightwards (tilt)

                if (rotation != Vector3.Zero)
                    rObject.Transform.Rotate(rotation * deltaTime);
            }
        }

        // Render scene objects
        private void Draw() {

            // Empty the frame buffer
            frameBuffer.Clear();

            Render3D();
            RenderUI();

            // Draw the scene on the the viewport
            frameBuffer.Draw();
        }

        // Render 3D Objects
        private void Render3D() {

            renderer.Render(rObject);

            frameBuffer.GetFragmentData(renderer.Fragments);
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
                    camera.Pitch -= 2.0f;                        // Turn Up
                    break;

                case ConsoleKey.NumPad8:
                    camera.Pitch += 2.0f;                        // Turn Down
                    break;

                case ConsoleKey.Add:
                    camera.Fov += 1.0f;                          // Zoom Out
                    break;

                case ConsoleKey.Subtract:                        // Zoom In
                    camera.Fov -= 1.0f;
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
                    rObject.Transform.Rotation = Vector3.Zero; // Reset

                    camera.Position = Vector3.Zero;
                    camera.Yaw = camera.Pitch = 0;
                    break;

                case 10:
                    loop = false;
                    input.Abort();
                    break;
            }
        }
    }
}
