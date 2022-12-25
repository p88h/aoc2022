using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day15 : Solution {

        public struct Sensor { public int sx, sy, bx, by, dist; }

        public List<Sensor> data = new List<Sensor>();
        public void parse(List<string> input) {
            Regex rx = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)", RegexOptions.Compiled);
            foreach (var s in input) {
                int[] pars = new int[4];
                Match m = rx.Match(s);
                for (int i = 1; i <= 4; i++) pars[i - 1] = int.Parse(m.Groups[i].Value);
                int db = Math.Abs(pars[0] - pars[2]) + Math.Abs(pars[1] - pars[3]);
                data.Add(new Sensor { sx = pars[0], sy = pars[1], bx = pars[2], by = pars[3], dist = db });
            }
        }

        public string part1() {
            List<int> ranges = new List<int>();
            HashSet<int> bcount = new HashSet<int>();
            int target = 2000000;
            foreach (var sensor in data) {
                var dy = Math.Abs(sensor.sy - target);
                if (dy <= sensor.dist) {
                    ranges.Add(2 * (sensor.sx - (sensor.dist - dy)));
                    ranges.Add(2 * (sensor.sx + (sensor.dist - dy)) + 1);
                }
                if (sensor.by == target) bcount.Add(sensor.by);
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

        public struct diagonal { public int index, start, width, parity; }
        public string part2() {
            int range = 4000000;
            var diags = new List<diagonal>();
            foreach (var sensor in data) {
                int lp = sensor.sx + sensor.sy - sensor.dist; // left or top most diagonal
                int dp = sensor.sx + sensor.sy + sensor.dist; // bottom or right most diagonal
                int sp = sensor.sx - sensor.sy - sensor.dist; // diag-rotated start point (same for both)
                int pp = (sensor.sx + sensor.sy) % 2; // parity of the center = rows with 'full' diamond.
                diags.Add(new diagonal { index = lp * 2, start = sp, width = sensor.dist, parity = pp });
                diags.Add(new diagonal { index = dp * 2 + 1, start = sp, width = sensor.dist, parity = pp });
            }
            diags.Sort((a, b) => a.index - b.index);
            var ranges = new List<int>();
            foreach (var diag in diags) {
                int di = diag.index / 2;
                if (diag.index % 2 == 0) {
                    ranges.Add(2 * diag.start);
                    ranges.Add(2 * (diag.start + diag.width * 2) + 1);
                    continue;
                } else {
                    ranges.Remove(2 * diag.start);
                    ranges.Remove(2 * (diag.start + diag.width * 2) + 1);
                }
                ranges.Sort();
                int count = 0;
                foreach (int r in ranges) {
                    if (r % 2 == 0) count++;
                    if (r % 2 != 0) count--;
                    int y = (di - (r / 2)) / 2, x = di - y;
                    if (count == 0 && x >= 0 && x < range && y >= 0 && y < range) {
                        long ret = (long)x * 4000000L + (long)y + 1L;
                        return ret.ToString();
                    }
                }
            }
            return "";
        }
    }
}
