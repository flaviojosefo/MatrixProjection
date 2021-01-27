using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProjection {

    public class DrawString {

        // Ratio between charaters' width and height (in pixels)
        private const float X_OFFSET = 1.0f;

        private readonly int width, height;

        private readonly int totalPixels;

        private readonly StringBuilder frame;

        public DrawString() {

            width = Console.WindowWidth;
            height = Console.WindowHeight;

            totalPixels = width * (height - 1);

            frame = new StringBuilder(totalPixels);
        }

        public void NewFrame() {

            frame.Clear();

            for (int i = 0; i < totalPixels; i++) {

                frame.Append(' ');
            }
        }

        public void AddPoint(Vector v, char symbol = '■') {

            if (!OutOfBounds(v)) {

                int x = (int)((v.X * X_OFFSET) + (width / 2.0f));
                int y = -(int)(v.Y - (height / 2.0f));

                int index = x + (y * width);

                frame[index] = symbol;
            }
        }

        public void AddLine(Vector from, Vector to) {

            int step = 10;

            Vector line = (to - from) / step;

            AddPoint(from);

            for (int i = 0; i < step - 1; i++) {

                AddPoint(from += line, '·');
            }

            AddPoint(from += line);
        }

        public void AddMesh(Vector[][] projected) {

            for (int i = 0; i < projected.Length; i++) {

                for (int j = 0; j < projected[i].Length; j++) {

                    // Connects each point to the next (connects the last to the first)
                    AddLine(projected[i][j], projected[i][(j + 1) % projected[i].Length]);
                }
            }
        }

        public void DrawFrame() {

            Console.SetCursorPosition(0, 0);
            Console.Write(frame);
        }

        private bool OutOfBounds(Vector v) {

            if ((int)((v.X * X_OFFSET) + (width / 2.0f)) >= width || (int)((v.X * X_OFFSET) + (width / 2.0f)) < 0 ||
               -(int)(v.Y - (height / 2.0f)) >= (height - 1) || -(int)(v.Y - (height / 2.0f)) < 0) {

                return true;
            }

            return false;
        }

        public void AddText(Vector windowCoord, string text) {

            int index = (int)(windowCoord.X + (windowCoord.Y * width));

            for (int i = 0; i < text.Length; i++) {

                frame[index + i] = text[i];
            }
        }

        public void AddText(Vector windowCoord, char character) {

            int index = (int)(windowCoord.X + (windowCoord.Y * width));

            frame[index] = character;
        }
    }
}
