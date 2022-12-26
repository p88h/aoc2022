using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day23 : Solution {

        protected List<(int, int)> data = new List<(int, int)>();
        protected int[,] scratch = { };
        (int x, int y)[] tmp = { };

        public void parse(List<string> input) {
            scratch = new int[input[0].Length * 2, input.Count * 2];
            for (int y = 0; y < input.Count; y++)
                for (int x = 0; x < input[y].Length; x++)
                    if (input[y][x] == '#') data.Add((x + 16, y + 16));
            tmp = new (int x, int y)[data.Count];
        }

        protected bool step((int x, int y)[] work, int round) {
            (int dx, int dy)[] moves = { (0, -1), (0, 1), (-1, 0), (1, 0) };
            (int dx, int dy)[] scan = { (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0) };
            int[] move2scan = { 0, 4, 6, 2 };
            int[] cnt = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            bool moved = false;
            for (int p = 0; p < work.Length; p++) { // part 1
                (int x, int y) = work[p];
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
                tmp[p] = npos;
                scratch[npos.x, npos.y] += 1 << 16;
            }
            for (int i = 0; i < work.Length; i++) {
                (int x, int y) npos = tmp[i];
                if (scratch[npos.x, npos.y] >> 16 == 1) {
                    scratch[work[i].x, work[i].y] = 0;
                    scratch[npos.x, npos.y] = round + 1;
                    work[i] = npos;
                } else {
                    scratch[npos.x, npos.y] = 0;
                    scratch[work[i].x, work[i].y] = round + 1;
                }
            }
            return moved;
        }

        public virtual string part1() {
            (int x, int y)[] pos = data.ToArray();
            foreach (var (x, y) in pos) scratch[x, y] = 1000;
            for (int i = 0; i < 10; i++) step(pos, 1000 + i);
            foreach (var (x, y) in pos) scratch[x, y] = 0;
            var xlist = pos.Select(p => p.x).ToList();
            var ylist = pos.Select(p => p.y).ToList();
            var rsize = (xlist.Max() - xlist.Min() + 1) * (ylist.Max() - ylist.Min() + 1);
            return (rsize - pos.Length).ToString();
        }

        public virtual string part2() {
            var pos = data.ToArray();
            int round = 0;
            foreach (var (x, y) in pos) scratch[x, y] = 2000;
            while (step(pos, 2000 + round)) round++;
            foreach (var (x, y) in pos) scratch[x, y] = 0;
            return (round+1).ToString();
        }
    }
}
