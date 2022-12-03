using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class ASCIIRay {
        int cx = 0, cy = 0;
        long fidx = 0;

        public ASCIIRay(int width, int height, int fps) {
            InitWindow(width, height, "AOC2022");
            SetTargetFPS(fps);
        }

        ~ASCIIRay() {
            CloseWindow();
        }

        public void loop(int fps, Func<int, bool> renderFrame) {
            int cnt = 0;
            bool done = false; ;
            while (!WindowShouldClose() && !done) {
                BeginDrawing();
                ClearBackground(BLACK);
                cx = cy = 8;
                done = renderFrame(cnt);
                EndDrawing();
                string idxStr = String.Format("{0, 0:D5}", fidx);
                TakeScreenshot("tmp/frame" + idxStr + ".png");
                cnt++;fidx++;
            }
        }

        public void Write(string msg) {
            DrawText(msg, cx, cy, 32, RAYWHITE);
            cx += msg.Length * 16;
        }

        public void WriteLine(string msg) {
            Write(msg);
            cx = 8; cy += 32;
        }
    }
}
