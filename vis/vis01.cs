using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace aoc2022 {

    public class Vis01 : Solution {

        private class Sugar {
            int px = 800;
            int py = 100;
            public int weight;
            public bool done = false;
            public int size;

            public Sugar(int w, int x) {
                weight = w;
                px = x;
                if (weight > 0) {
                    size = (int)Math.Round(Math.Sqrt(weight) / 10);
                } else {
                    size = 32;
                }
            }

            public void render() {
                if (weight > 0 && px < 1000) {
                    DrawRectangle(px - size / 2, py - size, size, size, Color.Blue);
                }
                if (px > 120) px -= 4;
                if (px <= 120 && py < 200) py += 4;
                if (py >= 200) done = true;
            }
        }

        private class Elf {
            public int weight = 0;
            public int px, py, dx, dy;

            public Elf(int x, int y) {
                px = dx = x;
                py = dy = y;
            }

            public void render(int frame) {
                bool moving = true;
                if (px != dx) {
                    if (px + 3 < dx) px += 3; else if (px - 3 > dx) px -= 3; else px = dx;
                } else if (py != dy) {
                    if (py + 3 < dy) py += 3; else if (py - 3 > dy) py -= 3; else py = dy;
                } else moving = false;
                if (px == 0 || px > 1000) return;

                int thick = weight < 200000 ? (weight * 16) / 200000 : 16;
                int width = thick + 4;
                int height = 20 - thick;
                int cx = px;
                int cy = py;

                DrawEllipse(cx, cy, width, height, Color.White);
                int fy = cy - height - 10;
                DrawCircle(cx, fy, 10, Color.White);
                DrawCircle(cx - 4, fy - 2, 2, Color.Black);
                DrawCircle(cx + 4, fy - 2, 2, Color.Black);
                int hy = fy - 10;
                DrawTriangle(new Vector2(cx - 5, hy), new Vector2(cx + 5, hy), new Vector2(cx, hy - 10), Color.Green);

                int ly = cy + height - 2;
                int ly2 = ly + height / 2;
                int ly3 = ly2 + 10;
                int lfx = cx - 8, rfx = cx + 8;
                if (moving) {
                    if ((frame / 5) % 2 == 0) rfx += 3; else lfx += 3;
                }
                DrawLine(lfx - 2, ly3, lfx, ly3, Color.White);
                DrawLine(lfx, ly3, cx - 8, ly2, Color.White);
                DrawLine(cx - 8, ly2, cx, ly, Color.White);

                DrawLine(rfx, ly3, rfx + 2, ly3, Color.White);
                DrawLine(rfx, ly3, cx + 8, ly2, Color.White);
                DrawLine(cx + 8, ly2, cx, ly, Color.White);

                int lx = cx - width + 2;
                DrawLine(lx, cy, lx - 4, cy + 6, Color.White);
                DrawLine(lx - 4, cy + 6, lx - 4, cy + 10, Color.White);

                int rx = cx + width - 2;
                DrawLine(rx, cy, rx + 10, cy, Color.White);
                DrawLine(rx + 10, cy, rx + 12, cy - 2, Color.White);

                DrawLine(rx + 4, cy - 4, rx + 6, cy - 2, Color.Red);
                DrawLine(rx + 6, cy - 2, rx + 50, cy - 2, Color.Red);
                DrawLine(rx + 50, cy - 2, rx + 52, cy - 4, Color.Red);
                int tx = rx + 25;
                int tw = (int)Math.Round(Math.Sqrt(weight) / 10);
                DrawTriangle(new Vector2(tx - tw / 2, cy - 2), new Vector2(tx + tw / 2, cy - 2), new Vector2(tx, cy - tw), Color.Blue);
                DrawText(weight.ToString(), rx + 12, cy + 4, 10, Color.Maroon);
            }

            void add(int s) { weight += s; }
        }
        private Day01 solver = new Day01();
        private List<Elf> elves = new List<Elf>();
        private List<Sugar> sugars = new List<Sugar>();
        int ofs = 0, cur = 0;
        public void parse(List<string> input) {
            solver.parse(input);
            int tot = 0;
            for (int i = 0; i < solver.data.Count(); i++) {
                sugars.Add(new Sugar(solver.data[i], 200 + tot));
                tot += sugars[i].size + 8;
            }
            for (int i = 0; i < 5; i++) {
                elves.Add(new Elf(100 - i * 20, 200));
            }
        }

        public string part1() {
            return solver.part1();
        }

        int maxframe = 1000000;

        public bool render(int cnt) {
            for (int i = 0; i < 10; i++) {
                int px = 132 + i * 84;
                DrawCircle(px, 110, 10, Color.DarkGray);
                switch ((i + (cnt / 5)) % 4) {
                    case 0: DrawLine(px, 102, px, 118, Color.LightGray); break;
                    case 1: DrawLine(px - 6, 104, px + 6, 116, Color.LightGray); break;
                    case 2: DrawLine(px - 8, 110, px + 8, 110, Color.LightGray); break;
                    case 3: DrawLine(px - 6, 116, px + 6, 104, Color.LightGray); break;
                }
            }
            DrawLine(132, 100, 1000, 100, Color.DarkGray);
            DrawLine(132, 120, 1000, 120, Color.DarkGray);
            Elf current = elves[cur];
            for (int i = ofs; i < sugars.Count(); i++) {
                if (sugars[i].done) {
                    current.weight += sugars[i].weight;
                    ofs++;
                    if (sugars[i].weight == 0) {
                        for (int j = 0; j < cur; j++) {
                            if (elves[j].weight < current.weight) {
                                elves[j].dx += 80;
                            } else {
                                current.dx += 80;
                            }
                        }
                        current.dx += 20;
                        current.dy += 128;
                        cur++;
                        for (int j = cur; j < elves.Count(); j++) {
                            elves[j].dx += 20;
                        }
                        elves.Add(new Elf(0, 200));
                    }
                    if (ofs == sugars.Count()) {
                        for (int j = cur + 1; j < elves.Count(); j++) {
                            elves[j].dx = 0;
                        }
                        maxframe = cnt + 120;
                    }
                } else {
                    sugars[i].render();
                }
            }
            foreach (var elf in elves) {
                elf.render(cnt);
            }
            return cnt > maxframe;
        }

        public string part2() {
            Viewer view = new Viewer(960, 540, 120, "Day01");
            view.loop(render);
            return solver.part2();
        }

    }
}
