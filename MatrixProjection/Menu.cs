using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatrixProjection {

    public class Menu {

        private const string PROJECT_TITLE = "MatrixProjection";
        private const string OBJ_DIRECTORY = "Models";

        private readonly string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private List<string> mainOptions;
        private List<string> localObjects;

        public Menu(int width, int height) {

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            Console.CursorVisible = false;

            Console.Title = PROJECT_TITLE;
        }

        // Create menu options
        public void Setup() {

            // The options on the "main menu"
            mainOptions = new List<string> {
                "Local Objects",
                "Import OBJ",
                "Exit"
            };

            // The options on the "local" submenu
            localObjects = new List<string> {
                "Cube",
                "Quad Pyramid",
                "Triangular Pyramid",
                "Engram (Regular Dodecahedron)",
                "Simple Triangle",
                "Back"
            };

            // Create a directory to store external files at startup
            CreateFilesDirectory();
        }

        public void DisplayMainOptions(int cursor = 0) {

            // Clean the console
            Console.Clear();

            // Get the number of options
            int maxOptions = mainOptions.Count;

            // Print every option (+ its index)
            for (int i = 0; i < maxOptions; i++)
                Console.WriteLine($"  {i + 1}." + mainOptions[i]);

            // Print the 'cursor'
            Console.SetCursorPosition(0, cursor);
            Console.Write('►');

            // Read user input
            switch(Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    cursor = cursor > 0 ? cursor - 1 : maxOptions - 1;
                    break;

                case ConsoleKey.DownArrow:
                    cursor = cursor < maxOptions - 1 ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:
                    SelectMainOption(cursor);
                    break;
            }

            DisplayMainOptions(cursor);
        }

        private void SelectMainOption(int cursor) {

            switch (cursor) {

                case 0:
                    DisplayLocal();
                    break;

                case 1:
                    SearchFiles();
                    break;

                case 2:
                    Environment.Exit(0);
                    break;
            }
        }

        // ********** SUBMENUS **********

        private void DisplayLocal(int cursor = 0) {

            // Clean the console
            Console.Clear();

            // Get the number of options
            int maxOptions = localObjects.Count;

            // Print every option (+ its index)
            for (int i = 0; i < maxOptions; i++)
                Console.WriteLine($"  {i + 1}." + localObjects[i]);

            // Print the 'cursor'
            Console.SetCursorPosition(0, cursor);
            Console.Write('►');

            // Read user input
            switch (Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    cursor = cursor > 0 ? cursor - 1 : maxOptions - 1;
                    break;

                case ConsoleKey.DownArrow:
                    cursor = cursor < maxOptions - 1 ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:

                    // Exit this submenu
                    if (cursor == maxOptions - 1)
                        return;

                    StartLocalObject(cursor);

                    break;
            }

            DisplayLocal(cursor);
        }

        private void StartLocalObject(int cursor) {

            Console.Clear();

            Scene scene = null;

            switch (cursor) {

                case 0:
                    scene = new Scene(RenderObject.Create<Cube>());
                    break;

                case 1:
                    scene = new Scene(RenderObject.Create<QuadPyramid>());
                    break;

                case 2:
                    scene = new Scene(RenderObject.Create<TriPyramid>());
                    break;

                case 3:
                    scene = new Scene(RenderObject.Create<Engram>());
                    break;

                case 4:
                    scene = new Scene(RenderObject.Create<SimpleTri>());
                    break;
            }

            scene?.Start();
            scene?.Run();
        }

        // Creates the directory where 'obj' files can be placed
        private void CreateFilesDirectory() {

            // Combine folder names
            string path = Path.Combine(documents, PROJECT_TITLE, OBJ_DIRECTORY);

            // Check if the path provided exists
            if (!Directory.Exists(path)) {

                // Create the 'obj' folder if not found
                Directory.CreateDirectory(path);
            }
        }

        // Searches for 'obj' files
        private void SearchFiles() {

            // Combine folder names
            string path = Path.Combine(documents, PROJECT_TITLE, OBJ_DIRECTORY);

            // Prevent bugs related to directory deletion while the application is running
            CreateFilesDirectory();

            // Get all 'obj' files on the directory
            string[] objPaths = Directory.GetFiles(path, "*.obj", SearchOption.TopDirectoryOnly);

            DisplayFiles(objPaths);
        }

        private void DisplayFiles(string[] paths, int cursor = 0) {

            // Clean the console
            Console.Clear();

            // Get the number of files
            int filesAmount = paths.Length;

            // Check if any files were found
            if (filesAmount != 0) {

                // Print every file name (+ its index)
                for (int i = 0; i < filesAmount; i++)
                    Console.WriteLine($"  {i + 1}." + paths[i].Split('\\').Last()/*.Split('.').First()*/);

            } else {

                // Print a message to the user
                Console.WriteLine($" No files found.");

                // Lock the cursor on 'back'
                cursor = 1;
            }

            // Print the 'back' option
            Console.WriteLine("  Back");

            // Print the 'cursor'
            Console.SetCursorPosition(0, cursor);
            Console.Write('►');

            // Read user input
            switch (Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    cursor = cursor > 0 ? cursor - 1 : filesAmount;
                    break;

                case ConsoleKey.DownArrow:
                    cursor = cursor < filesAmount ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:

                    // Exit this submenu
                    if (filesAmount == 0 || 
                        cursor == filesAmount)
                        return;

                    // Load the selected file
                    for (int i = 0; i < filesAmount; i++) {

                        // Get the file's index
                        if (cursor == i) {

                            SelectFile(paths[i]);
                            break;
                        }
                    }

                    break;
            }

            DisplayFiles(paths, cursor);
        }

        private void SelectFile(string path) {

            // Mesh Importer, scene setter, etc.

            // Clean the console
            Console.Clear();

            // Check if admin exists
            using (TextReader reader = File.OpenText(path)) {

                // Skips the first line
                //string firstLine = reader.ReadLine();

                while (reader.Peek() > -1) {

                    string[] values = reader.ReadLine().Split(' ');

                    if (values[0] == "v") {

                        for (int i = 0; i < values.Length; i++) {

                            // CONVERT TO FLOAT
                            //Console.Write(float.Parse(values[i], CultureInfo.InvariantCulture));

                            Console.Write(values[i] + (i + 1 != values.Length ? ' ' : '\n'));
                        }

                    } else if (values[0] == "f") {

                        for (int i = 0; i < values.Length; i++) {

                            // STORE FIRST VALUE
                            //Console.Write(values[i].Split('/').First() + (i + 1 != values.Length ? ' ' : '\n'));

                            //Console.Write(values[i] + (i + 1 != values.Length ? ' ' : '\n'));
                        }
                    }
                }

                //Console.WriteLine(reader.ReadToEnd());

                reader.Close();
            }

            Console.WriteLine("\nPress any key to go back...");

            Console.ReadKey();

            /* THE PLAN
             * 
             * 1. Save all 'v' into a list (ONLY store the numbers (as floats))
             * 2. Save all 'f' into a list (ONLY store the first value ('a') from each 'a/b/c' ('a' represents a vertex))
             * 3. Build triangles based on the vertices (all 'v') and their order (stored on 'f')
             * 4. Create a mesh with said triangles
             * 
             * Notes: OBJ uses right-hand coordinate system (invert the 'x' to fix)
             *        Vertices are stored in a counter-clockwise order by default (invert triangle's vertices?)
             */
        }
    }
}
