namespace aoc2022 {
    public class Day11 : Solution {

        public class Monkey {
            public long divisor, a, b, c, count;
            public int left, right;
            public List<long> items = new List<long>(), init;
            public Monkey(List<string> block) {
                var parts = block[1].Split(':');
                init = parts[1].Split(',').Select(x => long.Parse(x)).ToList();
                parts = block[2].Split(' ');
                if (parts[7] == "old") {
                    a = 1;
                } else {
                    long v = long.Parse(parts[7]);
                    if (parts[6] == "*") b = v; else c = v;
                }
                divisor = long.Parse(block[3].Split(' ')[5]);
                left = int.Parse(block[4].Split(' ')[9]);
                right = int.Parse(block[5].Split(' ')[9]);
            }
            public void Reset() { items.Clear(); items.AddRange(init); count = 0; }
        }

        public List<Monkey> monkeys = new List<Monkey>();
        public long modulus = 1;

        public void parse(List<string> input) {
            for (int b = 0; b < input.Count; b += 7) monkeys.Add(new Monkey(input.GetRange(b, 6)));
            foreach (var m in monkeys) modulus *= m.divisor;
        }

        public string part1() {
            foreach (var m in monkeys) m.Reset();
            for (int round = 0; round < 20; round++) {
                foreach (var m in monkeys) {
                    foreach (long v in m.items) {
                        m.count++;
                        long w = (m.a * v * v + m.b * v + m.c) / 3;
                        monkeys[(w % m.divisor == 0) ? m.left : m.right].items.Add(w);
                    }
                    m.items.Clear();
                }
            }
            var top = monkeys.OrderByDescending(m => m.count).ToList();
            return (top[0].count * top[1].count).ToString();
        }

        public string part2() {
            foreach (var m in monkeys) m.Reset();
            for (int round = 0; round < 10000; round++) {
                foreach (var m in monkeys) {
                    foreach (long v in m.items) {
                        m.count++;
                        long w = (m.a * v * v + m.b * v + m.c) % modulus;
                        monkeys[(w % m.divisor == 0) ? m.left : m.right].items.Add(w);
                    }
                    m.items.Clear();
                }
            }
            var top = monkeys.OrderByDescending(m => m.count).ToList();
            return (top[0].count * top[1].count).ToString();
        }
    }
}
