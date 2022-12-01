namespace aoc2022 {
    public class Day01 : Solution {
        public List<int> data = new List<int>();
        public List<int> sums = new List<int>();
        public void parse(List<string> input) {
            foreach (var s in input) {
                if (s.Length > 0) {
                    data.Add(int.Parse(s));
                } else {
                    data.Add(0);
                }
            }
            data.Add(0);
        }

        public string part1() {
            int sum = 0;
            int max = 0;
            foreach (var n in data) {
                if (n > 0) {
                    sum += n;
                } else {
                    if (sum > max) max = sum;
                    sums.Add(sum);
                    sum = 0;
                }
            }
            return max.ToString();
        }

        public string part2() {
            int tot3 = sums.OrderByDescending(n => n).Take(3).Sum();
            return tot3.ToString();
        }
    }
}
