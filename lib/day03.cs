namespace aoc2022 {
    public class Day03 : Solution {
        private List<string> data = new List<string>();
        public void parse(List<string> input) {
            data = input;
        }

        public string part1() {
            int sum = 0;            
            foreach (var n in data) {
                ulong mask = 0;
                for (int j = 0; j < n.Length/2; j++) mask |= 1UL << (n[j]-'@');
                for (int j = n.Length/2; j < n.Length; j++) if ((mask & (1UL << (n[j]-'@'))) != 0) {
                    if (n[j] > 'a') sum += n[j] - 'a' + 1; else sum += n[j] - 'A' + 27;
                    break;
                }
            }
            return sum.ToString();
        }

        public string part2() {
            int sum = 0;            
            for (int i = 0; i < data.Count; i+=3) {
                ulong a = 0, b = 0;
                foreach (char c in data[i]) a |= 1UL << (c-'@');
                foreach (char c in data[i+1]) b |= 1UL << (c-'@');
                ulong d = a & b;
                foreach (char c in data[i+2]) if ((d & (1UL << (c-'@'))) != 0) {
                    if (c > 'a') sum += c - 'a' + 1; else sum += c - 'A' + 27;
                    break;
                }
            }
            return sum.ToString();
        }
    }
}
