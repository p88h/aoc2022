using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Viewer {
        int width, height;
        FFWriter ff_writer;
        string title;

        public Viewer(int w, int h, int fps, string t) {
            Console.WriteLine("Initializing viewer: " + w + "x" + h + " @" + fps + "fps");
            width = w; height = h; title = t;
            InitWindow(width, height, "AOC2022 " + title);
            SetTargetFPS(fps);
            ff_writer = new FFWriter(width, height, fps, title);
        }

        public async void loop(Func<int, bool> renderFrame) {
            int cnt = 0;
            bool done = false;
            var ff_task = Task.Run(ff_writer.run);
            while (!WindowShouldClose() && !done) {
                BeginDrawing();
                ClearBackground(BLACK);
                done = renderFrame(cnt++);
                EndDrawing();
                Image screen = LoadImageFromScreen();
                unsafe { 
                    ff_writer.addRawImage(screen.data); 
                    MemFree(screen.data); 
                }
            }
            CloseWindow();
            ff_writer.finish();
            await ff_task;
        }
    }
}
