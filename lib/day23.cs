using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day23 : Solution {

        protected List<int> data = new List<int>();
        protected int[] scratch = { }, tmp = {};

        public void parse(List<string> input) {
            scratch = new int[input[0].Length * 512];
            for (int y = 0; y < input.Count; y++)
                for (int x = 0; x < input[y].Length; x++)
                    if (input[y][x] == '#') data.Add(((y + 16) << 8) + x + 16);
            tmp = new int[data.Count];
        }

        protected bool step(int[] work, int round) {
            (int dx, int dy)[] moves = { (0, -1), (0, 1), (-1, 0), (1, 0) };
            int[] scan = { -257, -256, -255, 1, 257, 256, 255, -1 };
            int[] move2scan = { 0, 4, 6, 2 };
            int[] cnt = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            bool moved = false;
            for (int p = 0; p < work.Length; p++) { // part 1
                int code = work[p], x = code & 0xFF, nx = x, y = code >> 8, ny = y, tot = 0;
                for (int s = 0; s < 8; s++) {
                    cnt[s] = ((scratch[code + scan[s]] & 0xFFFF) == round) ? 1 : 0;
                    tot += cnt[s];
                }
                if (tot != 0) {
                    cnt[8] = cnt[0];
                    for (int i = 0; i < 4; i++) {
                        int idx = (i + round) % 4;
                        int spos = move2scan[idx];
                        tot = cnt[spos] + cnt[spos + 1] + cnt[spos + 2];
                        if (tot == 0) {
                            (nx, ny) = (x + moves[idx].dx, y + moves[idx].dy);
                            break;
                        }
                    }
                    moved = true;
                }
                tmp[p] = (ny << 8) + nx;
                scratch[tmp[p]] += 1 << 16;
            }
            for (int i = 0; i < work.Length; i++) {
                int ncode = tmp[i], nx = ncode & 0xFF, ny = ncode >> 8;
                if (scratch[ncode] >> 16 == 1) {
                    scratch[work[i]] = 0;
                    scratch[ncode] = round + 1;                    
                    work[i] = ncode;
                } else {
                    scratch[ncode] = 0;
                    scratch[work[i]] = round + 1;
                }
            }
            return moved;
        }

        public virtual string part1() {
            int[] pos = data.ToArray();
            foreach (var code in pos) scratch[code] = 1000;
            for (int i = 0; i < 10; i++) step(pos, 1000 + i);
            foreach (var code in pos) scratch[code] = 0;
            var xlist = pos.Select(p => p & 0xFF).ToList();
            var ylist = pos.Select(p => p >> 8).ToList();
            var rsize = (xlist.Max() - xlist.Min() + 1) * (ylist.Max() - ylist.Min() + 1);
            return (rsize - pos.Length).ToString();
        }

        public virtual string part2() {
            var pos = data.ToArray();
            int round = 0;
            foreach (var code in pos) scratch[code] = 2000;
            while (step(pos, 2000 + round)) round++;
            foreach (var code in pos) scratch[code] = 0;
            return (round+1).ToString();
        }
    }
}
