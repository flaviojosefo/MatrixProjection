using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixProjection {

    public class FrameBuffer {

        private readonly int width, height;

        private readonly int totalPixels;

        private readonly StringBuilder buffer;

        public FrameBuffer() {

            width = Console.WindowWidth;
            height = Console.WindowHeight - 1;

            totalPixels = width * height;

            buffer = new StringBuilder(totalPixels);
        }

        public void NewFrame() {

            buffer.Clear();

            for (int i = 0; i < totalPixels; i++) {

                buffer.Append(' ');
            }
        }

        public void DrawFrame() {

            Console.SetCursorPosition(0, 0);
            Console.Write(buffer);
        }

        public void AddText(Vector3 windowCoord, string text) {

            int index = (int)(windowCoord.X + (windowCoord.Y * width));

            for (int i = 0; i < text.Length; i++) {

                buffer[index + i] = text[i];
            }
        }

        public void AddText(Vector3 windowCoord, char character) {

            int index = (int)(windowCoord.X + (windowCoord.Y * width));

            buffer[index] = character;
        }

        // Only receives relevant fragments to be drawn
        public void GetFragmentData(List<Fragment> frags) {

            for (int i = 0; i < frags.Count; i++) {

                if (!OutOfBounds(frags[i].ScreenPos)) {

                    // 2D to 1D
                    int index = (int)frags[i].ScreenPos.X + (int)(frags[i].ScreenPos.Y * width);

                    buffer[index] = (char)frags[i].Symbol;
                }
            }
        }

        // When clipping is implemented this SHOULD be discarded
        private bool OutOfBounds(Vector3 screenV) {

            if (screenV.X >= width || screenV.X < 0 ||
                screenV.Y >= height || screenV.Y < 0) {

                return true;
            }

            return false;
        }
    }
}
