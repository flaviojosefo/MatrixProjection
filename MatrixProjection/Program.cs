using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MatrixProjection {

    class Program {

        static void Main(string[] args) {

            Menu menu = new Menu(120, 50);
            menu.ShowOptions();
        }
    }
}
