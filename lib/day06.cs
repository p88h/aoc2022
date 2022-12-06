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
            int dups = 0;
            for (int i = 0; i < 14; i++) if (++counts[data[i]] == 2) dups++;
            for (int i = 14; i < data.Length; i++) {                
               if (dups == 0) return i.ToString();
               if (++counts[data[i]] == 2) dups++;
               if (--counts[data[i-14]] == 1) dups--;
            }
            return "";
        }
    }
}
