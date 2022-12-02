namespace aoc2022 {
    public class Day02 : Solution {
        private List<string> data = new List<string>();
        public void parse(List<string> input) {
            data = input;
        }

        public string part1() {
            return data.Select(s => ((s[2] - s[0] - 1) % 3) * 3 + (s[2] - 'X' + 1)).Sum().ToString();
        }

        public string part2() {
            return data.Select(s => ((s[0] - 'A' + s[2] - 'X' + 2) % 3) + 1 + (s[2] - 'X') * 3).Sum().ToString();
        }
    }
}
