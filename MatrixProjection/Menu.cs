using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Menu {

        private const string CONSOLE_TITLE = "MatrixProjection";

        private List<string> options;
        private int optionN;

        public Menu(int width, int height) {

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            Console.CursorVisible = false;

            Console.Title = CONSOLE_TITLE;
        }

        // Create menu options
        public void Setup() {

            options = new List<string> {
                "Cube",
                "Quad Pyramid",
                "Triangular Pyramid",
                "Engram (Regular Dodecahedron)",
                "Simple Triangle",
                "Exit"
            };
        }

        public void ShowOptions() {

            // Clean the console
            Console.Clear();

            // Get the number of options
            int maxOptions = options.Count;

            // Print every option (+ its index)
            for (int i = 0; i < maxOptions; i++)
                Console.WriteLine($"  {i + 1}." + options[i]);

            // Print 'cursor'
            Console.SetCursorPosition(0, optionN);
            Console.Write('►');

            // Read user input
            switch(Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    optionN = optionN > 0 ? optionN - 1 : maxOptions - 1;
                    break;

                case ConsoleKey.DownArrow:
                    optionN = optionN < maxOptions - 1 ? optionN + 1 : 0;
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
                    scene = new Scene(RenderObject.Create<Cube>());
                    break;

                case 2:
                    scene = new Scene(RenderObject.Create<QuadPyramid>());
                    break;

                case 3:
                    scene = new Scene(RenderObject.Create<TriPyramid>());
                    break;

                case 4:
                    scene = new Scene(RenderObject.Create<Engram>());
                    break;

                case 5:
                    scene = new Scene(RenderObject.Create<SimpleTri>());
                    break;

                case 6:
                    Environment.Exit(0);
                    return;
            }

            scene?.Start();
            scene?.Run();
        }
    }
}
