using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Diagnostics;

namespace aoc2022 {
    public class Viewer {
        int width, height;
        FFWriter ff_writer;
        string title;
        Stopwatch stopwatch = new Stopwatch();

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
            stopwatch.Start();
            long lastts = 0;
            long lastcnt = 0;
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
                long ts = stopwatch.ElapsedMilliseconds;
                if (ts > lastts + 4999) {
                    Console.WriteLine("Rendered " + (cnt-lastcnt) + " frames in " + (ts-lastts) + " ms. " + " AFPS=" + ((cnt * 1000 + 500) / ts));
                    lastcnt = cnt;
                    lastts = ts;
                }
            }
            CloseWindow();
            ff_writer.finish();
            await ff_task;
        }
    }
}
