using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Numerics;
using System.Net;

namespace aoc2022 {
    public class ASCIIRay {
        int cx = 0, cy = 0;
        long fidx = 0;
        Font font;
        int fsize;
        const string font_name = "Inconsolata-SemiBold.ttf";
        const string font_file = "resources/" + font_name;
        const string font_uri = "https://github.com/googlefonts/Inconsolata/raw/main/fonts/ttf/" + font_name;

        public ASCIIRay(int width, int height, int fps, int size = 24) {
            InitWindow(width, height, "AOC2022");
            SetTargetFPS(fps);
            fsize = size;
            #pragma warning disable SYSLIB0014
            if (!File.Exists(font_file)) using (var client = new WebClient()) client.DownloadFile(font_uri,font_file);
            #pragma warning restore SYSLIB0014
            font = LoadFontEx(font_file, fsize, null, 250);
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
                cx = cy = fsize / 2;
                done = renderFrame(cnt);
                EndDrawing();
                string idxStr = String.Format("{0, 0:D5}", fidx);
                TakeScreenshot("tmp/frame" + idxStr + ".png");
                cnt++; fidx++;
            }
        }

        public void Write(string msg) {
            DrawTextEx(font, msg, new Vector2(cx, cy), fsize, 1, RAYWHITE);
            cx += msg.Length * (fsize / 2);
        }

        public void WriteLine(string msg) {
            Write(msg);
            cx = fsize / 2; cy += fsize;
        }
    }
}
