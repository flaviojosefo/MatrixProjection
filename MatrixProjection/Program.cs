using System;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            // If ratio between pixel size changes, change 'ASPECT_RATIO' in 'Camera' class
            //Menu menu = new Menu(120, 80);
            Menu menu = new Menu(Console.LargestWindowWidth, Console.LargestWindowHeight);
            menu.ShowOptions();
        }
    }
}
