using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day15 : Solution {
        public List<int[]> data = new List<int[]>();
        public void parse(List<string> input) {
            Regex rx = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)", RegexOptions.Compiled);
            foreach (var s in input) {
                int[] pars = new int[4];
                Match m = rx.Match(s);
                for (int i = 1; i <= 4; i++) pars[i - 1] = int.Parse(m.Groups[i].Value);
                data.Add(pars);
            }
        }

        public string part1() {
            List<int> ranges = new List<int>();
            HashSet<int> bcount = new HashSet<int>();
            int target = 2000000;
            foreach (var pars in data) {
                var db = Math.Abs(pars[0] - pars[2]) + Math.Abs(pars[1] - pars[3]);
                var dy = Math.Abs(pars[1] - target);
                if (dy <= db) {
                    ranges.Add(2 * (pars[0] - (db - dy)));
                    ranges.Add(2 * (pars[0] + (db - dy)) + 1);
                }
                if (pars[3] == target) bcount.Add(pars[3]);
            }
            ranges.Sort();
            int count = 0, start = 0, tot = 0;
            foreach (var r in ranges) {
                if (r % 2 == 0 && ++count == 1) start = r / 2;
                if (r % 2 == 1 && --count == 0) tot += r / 2 - start + 1;
            }
            tot -= bcount.Count;
            return tot.ToString();
        }

        public string part2() {
            int range = 4000000;
            var targets = Enumerable.Range(0, range).ToList();
            long result = 0;
            Parallel.ForEach(targets, (target, state) => {
                List<long> ranges = new List<long>();
                foreach (var pars in data) {
                    long db = Math.Abs(pars[0] - pars[2]) + Math.Abs(pars[1] - pars[3]);
                    long dy = Math.Abs(pars[1] - target);
                    long a = pars[0] - (db - dy), b = pars[0] + (db - dy);
                    if (dy <= db && a <= range && b >= 0) {
                        if (a < 0) a = 0;
                        if (b > range) b = range;
                        ranges.Add(2 * a);
                        ranges.Add(2 * b + 1);
                    }
                }
                ranges.Sort();
                long count = 0, start = 0, tot = 0;
                foreach (long r in ranges) {
                    if (r % 2 == 0 && ++count == 1) start = r / 2;
                    if (r % 2 == 1 && --count == 0) tot += r / 2 - start + 1;
                    if (count == 0 && tot < range) {
                        result = (tot * (long) range + target);
                        state.Stop();
                    }
                }
            });
            return result.ToString();
        }
    }
}
