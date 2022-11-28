using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis00 : Solution {
        private Day00 solver = new Day00();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            Console.Clear();
            for (int y = 0; y < Console.WindowHeight / 2; y++) {
                Console.WriteLine();
            }
            for (int x = 0; x < Console.WindowWidth / 2 - 1; x++) {
                Console.Write(" ");
            }
            Console.WriteLine(solver.part1());
            Console.ReadLine();
            return "";
        }

        public string part2() {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 600;

            InitWindow(screenWidth, screenHeight, "AOC2022");

            SetTargetFPS(30);
            //--------------------------------------------------------------------------------------
            int cnt = 0;
            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText(solver.part2(), 190, 200, 20, MAROON);

                EndDrawing();
                //----------------------------------------------------------------------------------
                string countStr = String.Format("{0, 0:D5}", cnt);
                TakeScreenshot("tmp/frame" + countStr + ".png");
                cnt++;
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------
            
            return "";
        }
    }
}
