using System;

namespace MatrixProjection {

    public struct ConsoleData {

        public (int, int) XInterval { get; }
        public int Y { get; }
        public ConsoleColor Color { get; }

        public ConsoleData((int, int) xInterval, int y, ConsoleColor color) {

            XInterval = xInterval;
            Y = y;
            Color = color;
        }

        public int GetInterval() {

            return XInterval.Item2 - XInterval.Item1;
        }
    }
}
