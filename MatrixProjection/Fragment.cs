using System;

namespace MatrixProjection {

    public struct Fragment {

        public Vector3 ScreenPos { get; }
        public ShadeChar Symbol { get; }
        public ConsoleColor Color { get; }

        public Fragment(Vector3 pos, ShadeChar symbol, ConsoleColor color = ConsoleColor.White) {

            ScreenPos = pos;
            Symbol = symbol;
            Color = color;
        }
    }
}
