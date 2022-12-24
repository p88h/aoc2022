using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day24 : Solution {

        public char[,] lmap = { }, rmap = { }, umap = { }, dmap = { };
        public int hsize, vsize;

        public void parse(List<string> input) {
            hsize = input[0].Length - 2;
            vsize = input.Count - 2;
            lmap = new char[vsize, hsize]; rmap = new char[vsize, hsize];
            umap = new char[vsize, hsize]; dmap = new char[vsize, hsize];
            for (int y = 1; y < input.Count - 1; y++) {
                for (int x = 1; x < input[y].Length - 1; x++) {
                    lmap[y - 1, x - 1] = input[y][x] == '<' ? '<' : '.';
                    rmap[y - 1, x - 1] = input[y][x] == '>' ? '>' : '.';
                    umap[y - 1, x - 1] = input[y][x] == '^' ? '^' : '.';
                    dmap[y - 1, x - 1] = input[y][x] == 'v' ? 'v' : '.';
                }
            }
        }

        int trip(int sx, int sy, int ex, int ey, int st) {
            (int, int) startpos = (sx, sy);
            (int, int) endpos = (ex, ey);
            List<(int, int)> stack = new List<(int, int)> { startpos };
            HashSet<int> visited = new HashSet<int>();
            List<(int, int)> moves = new List<(int, int)> { (-1, 0), (1, 0), (0, -1), (0, 1), (0, 0) };
            int t = st, tm;
            while (stack.Count > 0) {
                List<(int, int)> nstack = new List<(int, int)>();
                t += 1; tm = t % (hsize * vsize);
                int tx = hsize - (t % hsize);
                int ty = vsize - (t % vsize);
                foreach (var (x, y) in stack) {
                    if ((x, y) == startpos && tm != st) nstack.Add(startpos);
                    foreach (var (dx, dy) in moves) {
                        var (nx, ny) = (x + dx, y + dy);
                        if ((nx, ny) == endpos) return t;
                        if (nx < 0 || ny < 0 || nx >= hsize || ny >= vsize) continue;
                        int code = (tm << 16) + (ny << 8) + nx;
                        if (visited.Contains(code)) continue;
                        if (lmap[ny, (nx + t) % hsize] == '.' && rmap[ny, (nx + tx) % hsize] == '.' &&
                            umap[(ny + t) % vsize, nx] == '.' && dmap[(ny + ty) % vsize, nx] == '.') {
                            visited.Add(code);
                            nstack.Add((nx, ny));
                        }
                    }
                }
                stack = nstack;
            }
            return -1;
        }

        public virtual string part1() {
            return trip(0, -1, hsize - 1, vsize, 0).ToString();
        }

        public virtual string part2() {
            int a = trip(0, -1, hsize - 1, vsize, 0);
            int b = trip(hsize - 1, vsize, 0, -1, a);
            int c = trip(0, -1, hsize - 1, vsize, b);
            Console.WriteLine("A: " + a + " B: " + b + " C: " + c);
            return c.ToString();
        }
    }
}
