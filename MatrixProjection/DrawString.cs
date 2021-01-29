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

        public void PlotPoint(Vector v, char symbol = '\u2588') { // '\u25A0' -> OLD BLOCK

            if (!OutOfBounds(v)) {

                Vector screenVec = ConvertToScreen(v);

                int index = (int)screenVec.X + (int)(screenVec.Y * width);

                frame[index] = symbol;
            }
        }

        public void PlotMesh(Vector[][] projected) {

            for (int i = 0; i < projected.Length; i++) {

                if (OccludedBackface(projected[i])) continue;

                for (int j = 0; j < projected[i].Length; j++) {

                    // Fills every possible spot between two given points to form a line
                    PlotLine(projected[i][j], projected[i][(j + 1) % projected[i].Length]);
                }
            }
        }

        public void DrawFrame() {

            Console.SetCursorPosition(0, 0);
            Console.Write(frame);
        }

        private bool OutOfBounds(Vector v) {

            if (ConvertToScreen(v).X >= width || ConvertToScreen(v).X < 0 ||
               ConvertToScreen(v).Y >= (height - 1) || ConvertToScreen(v).Y < 0) {

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

        private Vector ConvertToScreen(Vector v) {

            return new Vector((int)((v.X * X_OFFSET) + (width / 2.0f)), -(int)(v.Y - (height / 2.0f)));
        }

        private bool OccludedBackface(Vector[] polygon) {

            // Shoelace formula: https://en.wikipedia.org/wiki/Shoelace_formula#Statement
            // Division by 2 is not necessary, since all we care about is if the value is positive/negative

            float sum = 0.0f;

            for (int i = 0; i < polygon.Length; i++) {

                sum += (polygon[i].X * polygon[(i + 1) % polygon.Length].Y) - (polygon[i].Y * polygon[(i + 1) % polygon.Length].X);
            }

            return sum >= 0;
        }

        #region Bresenham's Line Algorithm

        /* Wiki w/ pseudocode -> https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#All_cases
         * 
         * Note: Better results with 'int' values than with 'float' (less jittery) */

        public void PlotLine(Vector from, Vector to) {

            if (Math.Abs((int)to.Y - (int)from.Y) < Math.Abs((int)to.X - (int)from.X)) {

                if ((int)from.X > (int)to.X) {

                    PlotLineLow(to, from);

                } else {

                    PlotLineLow(from, to);
                }

            } else {

                if ((int)from.Y > (int)to.Y) {

                    PlotLineHigh(to, from);

                } else {

                    PlotLineHigh(from, to);
                }
            }
        }

        private void PlotLineLow(Vector from, Vector to) {

            int dx = (int)to.X - (int)from.X;
            int dy = (int)to.Y - (int)from.Y;

            int yi = 1;

            if (dy < 0) {

                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = (int)from.Y;

            for (int i = (int)from.X; i <= (int)to.X; i++) {

                PlotPoint(new Vector(i, y));

                if (D > 0) {

                    y += yi;
                    D += 2 * (dy - dx);

                } else {

                    D += 2 * dy;
                }
            }
        }

        private void PlotLineHigh(Vector from, Vector to) {

            int dx = (int)to.X - (int)from.X;
            int dy = (int)to.Y - (int)from.Y;

            int xi = 1;

            if (dx < 0) {

                xi = -1;
                dx = -dx;
            }

            int D = (2 * dx) - dy;
            int x = (int)from.X;

            for (int i = (int)from.Y; i < (int)to.Y; i++) {

                PlotPoint(new Vector(x, i));

                if (D > 0) {

                    x += xi;
                    D += 2 * (dx - dy);

                } else {

                    D += 2 * dx;
                }
            }
        }

        #endregion
    }
}
