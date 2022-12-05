using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Numerics;

namespace aoc2022 {

    public class Vis04 : Solution {

        public class Block {
            public int px, py;
            public float alpha = 0.0f;
            public Block(int idx) {
                px = ((idx - 1) % 11) * 100 + 90;
                py = ((idx - 1) / 11) * 82 + 120;
            }

            public void render(int frame) {
                DrawRectangle(px, py, 100, 20, ColorAlpha(WHITE, alpha));
            }
        }

        private class Elf {
            public int thick;
            public int px, py, dx, dy, start, end, dir;
            public int cur = -1;
            public bool cleaning = true;

            public Elf(int w, int x, int y, int s, int e, int d) {
                thick = w;
                px = dx = x;
                py = dy = y;
                start = s;
                end = e;
                dir = d;
            }

            public bool render(int frame) {
                // ended cleaning in the current sector and not in the last sector. move to the next sector's beginning.
                if (px == dx && py == dy && cleaning && cur != end + dir) {
                    if (cur < 0) cur = start; else cur += dir;
                    if (cur == 0) {
                        dx = 140; dy = 80;
                    } else if (cur == 100) {
                        dx = 1240; dy = 680;
                    } else {
                        dx = ((cur - 1) % 11) * 100 + 140 - dir * 50;
                        dy = ((cur - 1) / 11) * 82 + 80;
                    }
                    if (cur == end + dir) {
                        dx = dir > 0 ? 1400 : -100;
                    }
                    cleaning = false;
                }
                // ended moving to the start position other than the last
                if (px == dx && py == dy && cur != end + dir && !cleaning) {
                    dx = dx + dir * 100;
                    cleaning = true;
                }

                bool moving = true;
                int speed = cleaning ? 3 : 5;
                if (py != dy) {
                    if (py + 3 < dy) py += 3; else if (py - 3 > dy) py -= 3; else py = dy;
                } else if (px != dx) {
                    if (px + speed < dx) px += speed; else if (px - speed > dx) px -= speed; else px = dx;
                } else moving = false;

                if (px <= -20 || px >= 1300) return false;

                int width = thick + 4;
                int height = 20 - thick;
                int cx = px;
                int cy = py;

                DrawEllipse(cx, cy, width, height, WHITE);
                int fy = cy - height - 10;
                DrawCircle(cx, fy, 10, WHITE);
                DrawCircle(cx - 4, fy - 2, 2, BLACK);
                DrawCircle(cx + 4, fy - 2, 2, BLACK);
                int hy = fy - 10;
                DrawTriangle(new Vector2(cx - 5, hy), new Vector2(cx + 5, hy), new Vector2(cx, hy - 10), GREEN);

                int ly = cy + height - 2;
                int ly2 = ly + height / 2;
                int ly3 = ly2 + 10;
                int lfx = cx - 8, rfx = cx + 8;
                if (moving) {
                    if ((frame / 5) % 2 == 0) rfx += 3; else lfx += 3;
                }
                DrawLine(lfx - 2, ly3, lfx, ly3, WHITE);
                DrawLine(lfx, ly3, cx - 8, ly2, WHITE);
                DrawLine(cx - 8, ly2, cx, ly, WHITE);

                DrawLine(rfx, ly3, rfx + 2, ly3, WHITE);
                DrawLine(rfx, ly3, cx + 8, ly2, WHITE);
                DrawLine(cx + 8, ly2, cx, ly, WHITE);

                int bmx = ((frame / 7) % 2 == 0) ? -2 : 2;
                int lx = cx - width + 2;
                if (px > dx && cleaning) {
                    DrawLine(lx, cy, lx - 10, cy, WHITE);
                    DrawLine(lx - 10, cy, lx - 12, cy + 2, WHITE);
                    // broom
                    DrawLine(lx - 12, cy + 2, lx - 12 + bmx, ly3 - 3, BLUE);
                    DrawRectangle(lx - 18 + bmx, ly3 - 3, 12, 3, BLUE);
                } else {
                    DrawLine(lx, cy, lx - 4, cy + 6, WHITE);
                    DrawLine(lx - 4, cy + 6, lx - 4, cy + 10, WHITE);
                }

                int rx = cx + width - 2;
                if (px < dx && cleaning) {
                    DrawLine(rx, cy, rx + 10, cy, WHITE);
                    DrawLine(rx + 10, cy, rx + 12, cy + 2, WHITE);
                    // broom
                    DrawLine(rx + 12, cy + 2, rx + 12 + bmx, ly3 - 3, BLUE);
                    DrawRectangle(rx + 6 + bmx, ly3 - 3, 12, 3, BLUE);
                } else {
                    DrawLine(rx, cy, rx + 4, cy + 6, WHITE);
                    DrawLine(rx + 4, cy + 6, rx + 4, cy + 10, WHITE);
                }
                return true;
            }
        }

        private Day04 solver = new Day04();
        private List<Elf> elves = new List<Elf>();
        private List<Block> blocks = new List<Block>();
        public void parse(List<string> input) {
            solver.parse(input);
            for (int i = 1; i <= 99; i++) blocks.Add(new Block(i));
        }

        public string part1() {
            return solver.part1();
        }

        public string part2() {
            const int screenWidth = 1280;
            const int screenHeight = 800;
            Random rnd = new Random();
            InitWindow(screenWidth, screenHeight, "AOC2022 DAY 4");
            SetTargetFPS(60);
            int cnt = 0;
            while (!WindowShouldClose()) {
                BeginDrawing();
                ClearBackground(BLACK);
                int p = cnt / 20;
                if (cnt % 20 == 0 && p < solver.data.Count) {
                    elves.Add(new Elf(rnd.Next(16), 40, 0, solver.data[p][0], solver.data[p][1], 1));
                    elves.Add(new Elf(rnd.Next(16), 1240, 820, solver.data[p][3], solver.data[p][2], -1));
                }
                foreach (var blk in blocks) blk.render(cnt);
                bool anymoving = false;
                foreach (var elf in elves) {
                    anymoving = anymoving | elf.render(cnt);
                    if (elf.cleaning) blocks[elf.cur - 1].alpha += 0.001f;
                }
                EndDrawing();
                string countStr = String.Format("{0, 0:D5}", cnt);
                if (anymoving) TakeScreenshot("tmp/frame" + countStr + ".png");
                cnt++;
            }
            CloseWindow();        // Close window and OpenGL context            
            return "";
        }
    }
}
