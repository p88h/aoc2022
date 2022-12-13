namespace aoc2022 {
    public class Day11 : Solution {
        public class Monkey {
            public long divisor, a, b, c;
            public int left, right;
            public Monkey(long pa, long pb, long pc, long d, int l, int r) {
                a = pa; b = pb; c = pc; divisor = d; left = l; right = r;
            }
        }

        public List<Monkey> monkeys = new List<Monkey>();
        public List<(int, long)> items = new List<(int, long)>();
        public long modulus = 1;

        public void parse(List<string> input) {
            for (int i = 0; i < input.Count; i += 7) {
                var block = input.GetRange(i, 6);
                var parts = block[1].Split(':');
                var init = parts[1].Split(',').Select(x => long.Parse(x)).ToList();
                parts = block[2].Split(' ');
                long a = 0, b = 0, c = 0;
                if (parts[7] == "old") {
                    a = 1;
                } else {
                    long v = long.Parse(parts[7]);
                    if (parts[6] == "*") b = v; else { b = 1; c = v; }
                }
                long d = long.Parse(block[3].Split(' ')[5]);
                int l = int.Parse(block[4].Split(' ')[9]);
                int r = int.Parse(block[5].Split(' ')[9]);
                monkeys.Add(new Monkey(a, b, c, d, l, r));
                foreach (var v in init) items.Add((i / 7, v));
                modulus *= d;
            }
        }

        public string run(int rounds, Func<long, int, int, long> worry) {
            long[] count = new long[monkeys.Count];
            for (int i = 0; i < items.Count; i++) {
                (int c, long x) = items[i];
                for (int r = 0; r < rounds;) {
                    var m = monkeys[c];
                    count[c]++;
                    x = worry(m.a * x * x + m.b * x + m.c, c, i);
                    int d = (x % m.divisor == 0) ? m.left : m.right;
                    if (d < c) r++;
                    c = d;
                }
            }
            var top = count.OrderByDescending(n => n).ToList();
            return (top[0] * top[1]).ToString();
        }

        public string part1() { return run(20, (x, y, z) => x / 3); }

        public string part2() { return run(10000, (x, y, z) => x % modulus); }
    }
}
