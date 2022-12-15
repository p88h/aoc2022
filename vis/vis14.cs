using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;


namespace aoc2022 {
    public class Vis14 : Solution {
        private Day14 solver = new Day14();
        public void parse(List<string> input) {
            solver.parse(input);
        }
        int tot1 = 0, tot2 = 0;

        public string part1() {
            ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 24, "Day14");
            List<(int, int)> flow = new List<(int, int)> { (500 - solver.ofs, 0) };
            List<(int, int)> sand = new List<(int, int)>();
            int maxcnt = 1000000;
            int S = 6;
            Color sandcol = YELLOW;
            int speed = 0;
            bool done = false;
            Console.WriteLine(solver.maxy);
            var mapp = solver.draw();
            for (int ax = 0; ax < 320; ax++) mapp[ax, solver.maxy + 2] = '#';
            RenderTexture2D background = LoadRenderTexture(1920, 1080);
            BeginTextureMode(background);
            foreach (var path in solver.paths) {
                for (int i = 1; i < path.Count; i++) {
                    (int sx, int sy) = path[i - 1];
                    (int ex, int ey) = path[i];
                    int x = Math.Min(sx, ex);
                    int y = Math.Min(sy, ey);
                    int w = (Math.Abs(ex - sx) + 1);
                    int h = (Math.Abs(ey - sy) + 1);
                    DrawRectangle(x * S, 1080 - y * S - h * S, w * S, h * S, DARKGRAY);
                }
            }
            DrawRectangle(0, 1080 - (solver.maxy + 2) * S - 64, 1920, 64, DARKBROWN);
            EndTextureMode();
            renderer.loop(cnt => {
                if (cnt % 60 == 0 && speed < 100) speed++;
                if (!done) for (int rep = 0; rep < speed; rep++) {
                        (int x, int y) = solver.drop(flow, mapp);
                        sand.Add((x, y));
                        mapp[x, y] = 'o';
                        if (y > solver.maxy && tot1 == 0) {
                            tot1 = tot2;
                            sandcol = ORANGE;
                        }
                        tot2++;
                        BeginTextureMode(background);
                        DrawCircle(x * S + S / 2, 1080 - y * S - S / 2, S / 2, tot1 > 0 ? ORANGE : YELLOW);
                        EndTextureMode();
                        if (flow.Count == 0) {
                            done = true;
                            maxcnt = cnt + 180;
                            break;
                        }
                    }
                DrawTexture(background.texture, 0, 0, WHITE);
                foreach ((var fx, var fy) in flow) DrawRectangleLines(fx * S + S / 4, fy * S + S / 4, S / 2, S / 2, DARKGREEN);
                renderer.WriteXY(1, 1, "Grains: " + tot2);
                if (tot1 > 0) renderer.WriteXY(1, 2, "Part 1: " + tot1);
                return cnt > maxcnt;
            });
            return tot1.ToString();
        }

        public string part2() {
            return tot2.ToString();
        }
    }
}
