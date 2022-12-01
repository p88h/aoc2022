namespace aoc2022 {
    public class Day01 : Solution {
        public List<int> data = new List<int>() { 0 };
        public List<int> sums = new List<int>();
        public void parse(List<string> input) {
            data = input.Select(s => (s.Length > 0) ? int.Parse(s) : 0).ToList();
        }

        public string part1() {
            sums = new List<int>(data.Count) { 0 };
            foreach (var n in data) if (n == 0) sums.Add(0); else sums[sums.Count - 1] += n;
            return sums.Max().ToString();
        }

        public string part2() {
            return sums.OrderByDescending(n => n).Take(3).Sum().ToString();
        }
    }
}
