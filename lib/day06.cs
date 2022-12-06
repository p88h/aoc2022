namespace aoc2022 {
    public class Day06 : Solution {
        private string data = "";
        public void parse(List<string> input) {
            data = input[0];
        }

        public string part1() {
            for (int i = 3; i < data.Length; i++) {
                if (data[i] != data[i - 1] && data[i] != data[i - 2] && data[i] != data[i - 3] &&
                    data[i - 1] != data[i - 2] && data[i - 1] != data[i - 3] && data[i - 2] != data[i - 3]) {
                    return (i+1).ToString();
                }
            }
            return "";
        }

        public string part2() {
            int[] counts = new int[256];
            int unique = 0;
            for (int i = 0; i < 14; i++) {
                int x = ++counts[data[i]];
                if (x == 1) unique++;
                if (x == 2) unique--;
            }
            for (int i = 14; i < data.Length; i++) {
                if (unique == 14) return i.ToString();
                int x = ++counts[data[i]];
                if (x == 1) unique++;
                if (x == 2) unique--;
                x = --counts[data[i-14]];
                if (x == 1) unique++;
                if (x == 0) unique--;
            }
            return "";
        }
    }
}
