namespace aoc2022 {
    public class Day02 : Solution {
        public List<string> data = new List<string>();
        public void parse(List<string> input) {
            data = input;
        }

        public string part1() {
            int t = 0;
            foreach (var s in data) { t += ((s[2] - s[0] - 1) % 3) * 3 + (s[2] - 'X' + 1); }
            return t.ToString();
        }

        public string part2() {
            int t = 0;
            foreach (var s in data) { t += ((s[0] - 'A' + s[2] - 'X' + 2) % 3) + 1 + (s[2] - 'X') * 3; }
            return t.ToString();
        }
    }
}
