using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day24 : Solution {

        protected char[,] mapp = { };
        protected int hsize, vsize;

        public void parse(List<string> input) {
            hsize = input[0].Length - 2;
            vsize = input.Count - 2;
            mapp = new char[vsize, hsize];
            for (int y = 0; y < vsize; y++) for (int x = 0; x < hsize; x++) mapp[y, x] = input[y + 1][x + 1];
        }

        protected int trip(int sx, int sy, int ex, int ey, int st, List<(int, int, int)>? trace = null) {
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
                    if (trace != null) trace.Add((t-1, x, y));
                    if ((x, y) == startpos && tm != st) nstack.Add(startpos);
                    foreach (var (dx, dy) in moves) {
                        var (nx, ny) = (x + dx, y + dy);
                        if ((nx, ny) == endpos) return t;
                        if (nx < 0 || ny < 0 || nx >= hsize || ny >= vsize) continue;
                        int code = (tm << 16) + (ny << 8) + nx;
                        if (visited.Contains(code)) continue;
                        if (mapp[ny, (nx + t) % hsize] != '<' && mapp[ny, (nx + tx) % hsize] != '>' &&
                            mapp[(ny + t) % vsize, nx] != '^' && mapp[(ny + ty) % vsize, nx] != 'v') {
                            visited.Add(code);
                            nstack.Add((nx, ny));
                        }
                    }
                }
                stack = nstack;
            }
            return -1;
        }

        public virtual string part1() { return trip(0, -1, hsize - 1, vsize, 0).ToString(); }

        public virtual string part2() {
            int a = trip(0, -1, hsize - 1, vsize, 0);
            int b = trip(hsize - 1, vsize, 0, -1, a);
            int c = trip(0, -1, hsize - 1, vsize, b);
            return c.ToString();
        }
    }
}
