using System;

namespace MatrixProjection {

    public struct Fragment {

        public Vector ScreenPos { get; }
        public ShadeChar Symbol { get; }
        public ConsoleColor Color { get; }

        public Fragment(Vector pos, ShadeChar symbol, ConsoleColor color = ConsoleColor.White) {

            ScreenPos = pos;
            Symbol = symbol;
            Color = color;
        }
    }
}
