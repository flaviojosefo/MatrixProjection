using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Draw {

        private const float xOffset = 2.0f;

        private int consoleX, consoleY;

        public Draw(int x, int y, bool cursor) {

            consoleX = x;
            consoleY = y;

            Console.WindowWidth = consoleX;
            Console.WindowHeight = consoleY;

            Console.BufferWidth = consoleX;
            Console.BufferHeight = consoleY;

            Console.CursorVisible = cursor;
        }

        public void DrawPoint(Vector v) {

            Console.SetCursorPosition((int)((v.X * xOffset) + (consoleX / 2.0f)), -(int)(v.Y - (consoleY / 2.0f)));
            Console.Write('·');
        }

        public void DrawLine(Vector from, Vector to) {

            float step = 10.0f;

            Vector line = (to - from) / step;

            DrawPoint(from);

            for (int i = 0; i < step; i++) {

                DrawPoint(from += line);
            }
        }

        private bool OutOfBounds() {

            return false;
        }
    }
}
