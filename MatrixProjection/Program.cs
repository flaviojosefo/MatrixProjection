using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            // Adapted to Squared font (8x8 pixels)
            // If ratio between pixel size changes, change 'X_OFFSET' in 'DrawString' class
            Menu menu = new Menu(120, 80);
            menu.ShowOptions();
        }
    }
}
