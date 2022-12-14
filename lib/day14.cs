using System;
using System.Text;

namespace aoc2022 {
    public class Day14 : Solution {

        public List<List<(int, int)>> paths = new List<List<(int, int)>>();
        public int maxy = 0;
        public int ofs = 340;

        public void parse(List<string> input) {
            foreach (var s in input) {
                var path = s.Split(" -> ").Select(s => s.Split(',')).Select(p => (int.Parse(p[0]) - ofs, int.Parse(p[1]))).ToList();
                foreach ((int x, int y) in path)  maxy = Math.Max(maxy, y);
                paths.Add(path);
            }
        }

        public char[,] draw() {
            var ret = new char[320, 160];
            for (int cx = 0; cx < 320; cx++) for (int cy = 0; cy < 160; cy++) ret[cx, cy] = ' ';
            foreach (var path in paths) {
                for (int i = 1; i < path.Count; i++) {
                    (int sx, int sy) = path[i - 1];
                    (int ex, int ey) = path[i];
                    (int dx, int dy) = (Math.Sign(ex - sx), Math.Sign(ey - sy));
                    while (sx != ex || sy != ey) {
                        ret[sx, sy] = '#';
                        sx += dx; sy += dy;
                    }
                    ret[ex, ey] = '#';
                }
            }
            return ret;
        }

        public (int, int) drop(List<(int, int)> flow, char[,] mapp) {
            (int x, int y) = flow[flow.Count - 1];
            while (y < 159) {
                if (mapp[x, y + 1] == ' ') {
                    y++;
                } else if (mapp[x - 1, y + 1] == ' ') {
                    x--; y++;
                } else if (mapp[x + 1, y + 1] == ' ') {
                    x++; y++;
                } else break;
                flow.Add((x, y));
            }
            if (y < 159) flow.RemoveAt(flow.Count - 1);
            return (x, y);
        }

        public string solve(int limit, char[,] mapp) {
            List<(int, int)> flow = new List<(int, int)> { (500-ofs, 0) };
            int tot = 0;
            while (flow.Count > 0) {
                (int x, int y) = drop(flow, mapp);
                if (y > limit) break;
                mapp[x, y] = 'o';
                tot++;
            }
            return tot.ToString();
        }

        public string part1() {
            return solve(maxy, draw());
        }
        public string part2() {
            char[,] mapp = draw();
            for (int ax = 0; ax < 320; ax++) mapp[ax, maxy + 2] = '#';
            return solve(maxy + 3, mapp);
        }
    }
}
