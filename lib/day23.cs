using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day23 : Solution {

        protected List<(int, int)> data = new List<(int, int)>();
        protected int[,] scratch = { }, dedup = { };

        public void parse(List<string> input) {
            scratch = new int[input[0].Length * 2, input.Count * 2];
            for (int y = 0; y < input.Count; y++)
                for (int x = 0; x < input[y].Length; x++)
                    if (input[y][x] == '#') data.Add((x + 16, y + 16));
        }

        protected List<(int, int)> step(List<(int x, int y)> start, int round, out bool moved) {
            List<(int x, int y)> tmplan = new List<(int, int)>(start.Count);
            List<(int dx, int dy)> moves = new List<(int, int)> { (0, -1), (0, 1), (-1, 0), (1, 0) };
            List<(int dx, int dy)> scan = new List<(int, int)> { (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0) };
            int[] move2scan = { 0, 4, 6, 2 };
            int[] cnt = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            moved = false;
            foreach (var (x, y) in start) { // part 1
                (int x, int y) npos = (x, y);
                int tot = 0;
                for (int s = 0; s < 8; s++) {
                    cnt[s] = ((scratch[x + scan[s].dx, y + scan[s].dy] & 0xFFFF) == round) ? 1 : 0;
                    tot += cnt[s];
                }
                if (tot != 0) {
                    cnt[8] = cnt[0];
                    for (int i = 0; i < 4; i++) {
                        int idx = (i + round) % 4;
                        int spos = move2scan[idx];
                        tot = cnt[spos] + cnt[spos + 1] + cnt[spos + 2];
                        if (tot == 0) {
                            npos = (x + moves[idx].dx, y + moves[idx].dy);
                            break;
                        }
                    }
                    moved = true;
                }
                tmplan.Add(npos);
                scratch[npos.x, npos.y] += 1 << 16;
            }
            for (int i = 0; i < start.Count; i++) {
                (int x, int y) npos = tmplan[i];
                if (scratch[npos.x, npos.y] >> 16 == 1) {
                    scratch[start[i].x, start[i].y] = 0;
                    scratch[npos.x, npos.y] = round + 1;
                } else {
                    scratch[npos.x, npos.y] = 0;
                    scratch[start[i].x, start[i].y] = round + 1;
                    tmplan[i] = start[i];
                }
            }
            return tmplan;
        }

        public virtual string part1() {
            var pos = new List<(int x, int y)>(data);
            bool moved;
            foreach (var (x, y) in pos) scratch[x, y] = 1000;
            for (int i = 0; i < 10; i++) pos = step(pos, 1000 + i, out moved);
            var xlist = pos.Select(p => p.x).ToList();
            var ylist = pos.Select(p => p.y).ToList();
            var rsize = (xlist.Max() - xlist.Min() + 1) * (ylist.Max() - ylist.Min() + 1);
            return (rsize - pos.Count).ToString();
        }

        public virtual string part2() {
            var pos = new List<(int x, int y)>(data);
            bool moved = true;
            int round = 0;
            foreach (var (x, y) in pos) scratch[x, y] = 2000;
            while (moved) pos = step(pos, 2000 + (round++), out moved);
            return round.ToString();
        }
    }
}
