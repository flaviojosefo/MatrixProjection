using System;

namespace MatrixProjection {

    // DEPRECATED

    public class Draw {

        private const float xOffset = 2.0f;

        private readonly int consoleX, consoleY;

        public Draw() {

            consoleX = Console.WindowWidth;
            consoleY = Console.WindowHeight;
        }

        public void DrawPoint(Vector3 v, bool render = true, char symbol = '■') {

            if (!OutOfBounds(v)) {

                Console.SetCursorPosition((int)((v.X * xOffset) + (consoleX / 2.0f)), -(int)(v.Y - (consoleY / 2.0f)));
                Console.Write(render ? symbol : ' ');
            }
        }

        public void DrawLine(Vector3 from, Vector3 to, bool render = true) {

            int step = 10;

            Vector3 line = (to - from) / step;

            DrawPoint(from, render);

            for (int i = 0; i < step - 1; i++) {

                DrawPoint(from += line, render, '·'); 
            }

            DrawPoint(from += line, render);
        }

        private bool OutOfBounds(Vector3 v) {

            if ((int)((v.X * xOffset) + (consoleX / 2.0f)) >= consoleX || (int)((v.X * xOffset) + (consoleX / 2.0f)) < 0 ||
               -(int)(v.Y - (consoleY / 2.0f)) >= (consoleY - 1) || -(int)(v.Y - (consoleY / 2.0f)) < 0) {

                return true;
            }

            return false;
        }
    }
}
