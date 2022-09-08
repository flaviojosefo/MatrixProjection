using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        // Displays the options on the main menu
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
                    // Move the cursor up
                    cursor = cursor > 0 ? cursor - 1 : maxOptions - 1;
                    break;

                case ConsoleKey.DownArrow:
                    // Move the cursor down
                    cursor = cursor < maxOptions - 1 ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:
                    SelectMainOption(cursor);
                    break;
            }

            DisplayMainOptions(cursor);
        }

        // Select an option on the main menu
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
                    // Move the cursor up
                    cursor = cursor > 0 ? cursor - 1 : maxOptions - 1;
                    break;

                case ConsoleKey.DownArrow:
                    // Move the cursor down
                    cursor = cursor < maxOptions - 1 ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:

                    // Exit this submenu
                    if (cursor == maxOptions - 1)
                        return;

                    LoadLocalObject(cursor);

                    break;
            }

            DisplayLocal(cursor);
        }

        // Loads a new Scene with objects from this project
        private void LoadLocalObject(int cursor) {

            // Clean the console
            Console.Clear();

            // The scene to be displayed
            Scene scene = null;

            // Check where the cursor is pointing
            switch (cursor) {

                case 0:
                    // Create a simple cube
                    scene = new Scene(RenderObject.Create<Cube>());
                    break;

                case 1:
                    // Create a pyramid with a quad base
                    scene = new Scene(RenderObject.Create<QuadPyramid>());
                    break;

                case 2:
                    // Create a pyramid with a triangular base
                    scene = new Scene(RenderObject.Create<TriPyramid>());
                    break;

                case 3:
                    // Create a dodecahedron - the word 'Engram' comes from Destiny 2 ;)
                    scene = new Scene(RenderObject.Create<Engram>());
                    break;

                case 4:
                    // Create a simple triangle (mostly used for baseline testing)
                    scene = new Scene(RenderObject.Create<SimpleTri>());
                    break;
            }

            // Initiate the scene
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
                    Console.WriteLine($"  {i + 1}." + paths[i].Split('\\').Last());

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
                    // Move the cursor up
                    cursor = cursor > 0 ? cursor - 1 : filesAmount;
                    break;

                case ConsoleKey.DownArrow:
                    // Move the cursor down
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

                            LoadFile(paths[i]);
                            break;
                        }
                    }

                    break;
            }

            DisplayFiles(paths, cursor);
        }

        // Loads a new Scene with objects generated from an external file
        private void LoadFile(string path) {

            // Clean the console
            Console.Clear();

            // Generate a mesh by reading the supplied '.obj' file
            Mesh generated = CustomMesh.GenerateFromOBJ(path);

            // Create a RenderObject with the prior mesh
            RenderObject rObject = RenderObject.Create(generated);

            // The scene to be displayed
            Scene scene = new Scene(rObject);

            // Initiate the scene
            scene.Start();
            scene.Run();
        }
    }
}
