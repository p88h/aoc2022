namespace aoc2022 {
    public class Day06 : Solution {
        private char[] data = { };
        public void parse(List<string> input) {
            data = input[0].ToArray();
        }

        public string part1() {
            for (int i = 3; i < data.Length; i++) {
                if (data[i] != data[i - 1] && data[i] != data[i - 2] && data[i] != data[i - 3] &&
                    data[i - 1] != data[i - 2] && data[i - 1] != data[i - 3] && data[i - 2] != data[i - 3]) {
                    return (i + 1).ToString();
                }
            }
            return "";
        }

        public string part2() {
            for (int i = 13; i < data.Length; i++) {
                bool ok = true;
                for (int k = 1; k < 14; k++) {
                    for (int j = i - k; j > i - 14; j--) {
                        if (data[j] == data[j + k]) {
                            i = j + k + 12;
                            k = 15;
                            ok = false;
                            break;
                        }
                    }
                }
                if (ok) return (i + 1).ToString();
            }
            return "";
        }


        public string part2slower() {
            int[] counts = new int[256];
            int dups = 0;
            for (int i = 0; i < 14; i++) if (++counts[data[i]] == 2) dups++;
            for (int i = 14; i < data.Length; i++) {
                if (dups == 0) return i.ToString();
                if (++counts[data[i]] == 2) dups++;
                if (--counts[data[i - 14]] == 1) dups--;
            }
            return "";
        }
    }
}
