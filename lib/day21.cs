namespace aoc2022 {
    public class Day21 : Solution {

        public struct Monkey {
            public double value;
            public string left, right, op;
        }
        public Dictionary<string, Monkey> data = new Dictionary<string, Monkey>();

        public void parse(List<string> input) {
            foreach (var s in input) {
                var s1 = s.Split(": ");
                var s2 = s1[1].Split(' ');
                if (s2.Length == 1) data.Add(s1[0], new Monkey { value = double.Parse(s2[0]), op = "V" });
                else data.Add(s1[0], new Monkey { left = s2[0], right = s2[2], op = s2[1] });
            }
        }

        public (double, double) compute(string key, List<string>? order = null) {
            if (order != null) order.Add(key);
            if (key == "humn") return (1, 0);
            if (data[key].op[0] == 'V') return (0, data[key].value);
            var (lh, lv) = compute(data[key].left, order);
            var (rh, rv) = compute(data[key].right, order);
            switch (data[key].op[0]) {
                case '+': return (lh + rh, lv + rv);
                case '-': return (lh - rh, lv - rv);
                case '*': return (lv * rh + lh * rv, lv * rv);
                case '/': return (lh / rv, lv / rv);
                default: break;
            }
            return (0, 0);
        }

        public string part1() {
            var (h, v) = compute("root");
            return (data["humn"].value * h + v).ToString();
        }

        public string part2() {
            var (lh, lv) = compute(data["root"].left);
            var (rh, rv) = compute(data["root"].right);
            var ret = (lh != 0) ? ((rv - lv) / lh) : ((lv - rv) / rh);
            return Math.Round(ret).ToString();
        }
    }
}
