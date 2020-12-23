using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Menu {

        private int optionN;

        public Menu(int consoleX, int consoleY) {

            Console.WindowWidth = consoleX;
            Console.WindowHeight = consoleY;

            Console.BufferWidth = consoleX;
            Console.BufferHeight = consoleY;

            Console.CursorVisible = false;
        }

        public void ShowOptions() {

            Console.Clear();

            Console.WriteLine("  1.Cube");
            Console.WriteLine("  2.Quad Pyramid");
            Console.WriteLine("  3.Triangular Pyramid");
            Console.WriteLine("  4.Exit");

            Console.SetCursorPosition(0, optionN);
            Console.Write('►');

            switch(Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    optionN = optionN > 0 ? optionN - 1 : 3;
                    break;

                case ConsoleKey.DownArrow:
                    optionN = optionN < 3 ? optionN + 1 : 0;
                    break;

                case ConsoleKey.Enter:
                    SelectOption();
                    break;
            }

            ShowOptions();
        }

        private void SelectOption() {

            Console.Clear();

            Scene scene = null;

            switch(optionN + 1) {

                case 1:
                    scene = new Scene(60, new Cube());
                    break;

                case 2:
                    scene = new Scene(60, new QuadPyramid());
                    break;

                case 3:
                    scene = new Scene(60, new TriPyramid());
                    break;

                case 4:
                    return;
            }

            scene.Start();
            scene.Update();
        }
    }
}
