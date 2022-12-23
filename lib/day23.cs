using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day23 : Solution {

        public List<(int, int)> data = new List<(int, int)>();

        public void parse(List<string> input) {
            for (int y = 0; y < input.Count; y++) for (int x = 0; x < input[y].Length; x++) if (input[y][x] == '#') data.Add((x, y));
        }

        public HashSet<(int, int)> step(HashSet<(int x, int y)> start, int round, out bool moved) {
            HashSet<(int, int)> tmp = new HashSet<(int, int)>();
            Dictionary<(int, int), (int, int)> plan = new Dictionary<(int, int), (int, int)>();
            Dictionary<(int, int), int> dedup = new Dictionary<(int, int), int>();
            List<(int dx, int dy)> moves = new List<(int, int)> { (0, -1), (0, 1), (-1, 0), (1, 0) };
            List<(int dx, int dy)> scan = new List<(int, int)> { (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0) };
            int[] move2scan = { 0, 4, 6, 2 };
            moved = false;
            foreach (var (x, y) in start) { // part 1
                (int, int) npos = (x, y);
                List<int> cnt = new List<int>();
                int tot = 0;
                foreach (var (dx, dy) in scan) { cnt.Add(start.Contains((x + dx, y + dy)) ? 1 : 0); tot+=cnt[cnt.Count-1]; }                
                if (tot != 0) {
                    cnt.Add(cnt[0]);
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
                plan[(x,y)] = npos;
                if (dedup.ContainsKey(npos)) dedup[npos] += 1; else dedup[npos] = 1;
            }
            foreach (var (cpos, npos) in plan) if (dedup[npos] == 1) tmp.Add(npos); else tmp.Add(cpos);
            return tmp;
        }

        public virtual string part1() {
            HashSet<(int x, int y)> pos = new HashSet<(int, int)>(data);
            bool moved;
            for (int i = 0; i < 10; i++) pos = step(pos, i, out moved);
            var xlist = pos.Select(p => p.x).ToList();
            var ylist = pos.Select(p => p.y).ToList();
            var rsize = (xlist.Max() - xlist.Min() + 1) * (ylist.Max() - ylist.Min() + 1);
            return (rsize - pos.Count).ToString();
        }

        public virtual string part2() {
            HashSet<(int x, int y)> pos = new HashSet<(int, int)>(data);
            bool moved = true;
            int round = 0;
            while (moved) pos = step(pos, round++, out moved);
            return round.ToString();
        }
    }
}
