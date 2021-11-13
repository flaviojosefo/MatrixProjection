using System;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            // Adapted to Squared font (8x8 pixels)
            // If ratio between pixel size changes, change 'ASPECT_RATIO' in 'Scene' class
            //Menu menu = new Menu(120, 80); // Console Window starts at [30,25]
            Menu menu = new Menu(Console.LargestWindowWidth, Console.LargestWindowHeight);
            menu.ShowOptions();
        }
    }
}
