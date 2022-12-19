using System.Collections.Concurrent;

namespace aoc2022 {
    public class Day19 : Solution {
        private List<int[]> data = new List<int[]>();
        public void parse(List<string> input) {
            int[] idxs = { 6, 12, 18, 21, 27, 30 };
            foreach (var s in input) {
                var ss = s.Split(' ');
                var t = new int[idxs.Length];
                for (int i = 0; i < idxs.Length; i++) t[i] = int.Parse(ss[idxs[i]]);
                data.Add(t);
            }
        }

        (int[], int[], int) nextState(int[] rs, int[] cs, int p, int r1, int c1, int r2, int c2) {
            int w = Math.Max((c1 - cs[r1] + rs[r1] - 1) / rs[r1], (c2 - cs[r2] + rs[r2] - 1) / rs[r2]);
            if (w < 0) w = 0; w++;
            int[] ncs = new int[4] { cs[0] + rs[0] * w, cs[1] + rs[1] * w, cs[2] + rs[2] * w, cs[3] + rs[3] * w };
            ncs[r1] -= c1; ncs[r2] -= c2;
            int[] nrs = new int[4] { rs[0], rs[1], rs[2], rs[3] }; nrs[p]++;
            return (nrs, ncs, w);
        }

        int maxsim(int[] rs, int[] cs, int[] blueprint, int r, int maxr) {
            int max = 0, w;
            int[] nrs, ncs;
            if (r > maxr) return 0;
            if (r == maxr) return cs[3];                        
            // check if we can build a geode robot first. No max.
            if (rs[2] > 0) {
                (nrs, ncs, w) = nextState(rs, cs, 3, 0, blueprint[4], 2, blueprint[5]);
                int tmp = cs[3] + rs[3] * (maxr - r);
                if (r + w <= maxr) tmp = maxsim(nrs, ncs, blueprint, r + w, maxr);
                max = Math.Max(max, tmp);
                if (w == 1) return max;
            }
            // build ore robot next, up to 4 max.
            if (rs[0] < 4) {
                (nrs, ncs, w) = nextState(rs, cs, 0, 0, blueprint[0], 0, 0);
                max = Math.Max(max, maxsim(nrs, ncs, blueprint, r + w, maxr));
            }
            // same for a clay robot, up to 8 max.
            if (rs[1] < 8) {
                (nrs, ncs, w) = nextState(rs, cs, 1, 0, blueprint[1], 0, 0);
                max = Math.Max(max, maxsim(nrs, ncs, blueprint, r + w, maxr));
            }
            // same for an obsidian robot, up to 8 max.
            if (rs[1] > 0 && rs[2] < 8) {
                (nrs, ncs, w) = nextState(rs, cs, 2, 0, blueprint[2], 1, blueprint[3]);
                max = Math.Max(max, maxsim(nrs, ncs, blueprint, r + w, maxr));
            }
            return max;
        }

        public string part1() {
            return Enumerable.Range(0, data.Count).AsParallel()
                .Select(idx => (idx + 1) * maxsim(new int[] { 1, 0, 0, 0 }, new int[4] { 1, 0, 0, 0 }, data[idx], 1, 24) )
                .Sum().ToString();
        }

        public string part2() {
            return Enumerable.Range(0, 3).AsParallel()
                .Select(idx => maxsim(new int[] { 1, 0, 0, 0 }, new int[4] { 1, 0, 0, 0 }, data[idx], 1, 32) )
                .Aggregate((t,n) => t*n).ToString();
        }
    }
}
