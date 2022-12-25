namespace aoc2022 {
    public class Day25 : Solution {
        private List<string> data = new List<string>();
        public void parse(List<string> input) { data = input; }

        long snafudec(string number) {
            long r = 0, p = 1;
            for (int i = 1; i <= number.Length; i++) {
                char c = number[number.Length - i];
                long v;
                switch (c) {
                    case '-': v = -1; break;
                    case '=': v = -2; break;
                    default: v = c - '0'; break;
                }
                r += p * v; p *= 5;
            }
            return r;
        }

        string snafuenc(long value) {
            List<char> arr = new List<char>();
            while (value > 0) {
                switch (value % 5) {
                    case 3: arr.Add('='); value += 5; break;
                    case 4: arr.Add('-'); value += 5; break;
                    default: arr.Add((char)((value % 5) + '0')); break;
                }
                value /= 5;
            }
            arr.Reverse();
            return new string(arr.ToArray());
        }

        public string part1() {
            long sum = 0;
            foreach (var n in data) sum += snafudec(n);
            return snafuenc(sum);
        }

        public string part2() {
            long prod = 1;
            return prod.ToString();
        }
    }
}
