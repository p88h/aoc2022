namespace aoc2022 {
    public class Day21 : Solution {

        public struct Monkey {
            public long value;
            public string left, right;
            public char op;
            public bool humn;
        }
        private Dictionary<string, Monkey> data = new Dictionary<string, Monkey>();

        public void parse(List<string> input) {
            foreach (var s in input) {
                var s1 = s.Split(": ");
                var s2 = s1[1].Split(' ');
                if (s2.Length == 1) data.Add(s1[0], new Monkey { value = long.Parse(s2[0]), op = 'V', humn = s1[0] == "humn" });
                else data.Add(s1[0], new Monkey { left = s2[0], right = s2[2], op = s2[1][0] });
            }
        }

        List<string> arrange(string root) {
            var stack = new List<string> { root };
            var visited = new HashSet<string> { root };
            var order = new List<string>();
            while (stack.Count > 0) {
                var nstack = new List<string>();
                foreach (var name in stack) {
                    order.Add(name);
                    var node = data[name];
                    if (node.op != 'V') {
                        if (!visited.Contains(node.left)) {
                            nstack.Add(node.left);
                            visited.Add(node.left);
                        }
                        if (!visited.Contains(node.right)) {
                            nstack.Add(node.right);
                            visited.Add(node.right);
                        }
                    }
                }
                stack = nstack;
            }
            return order;
        }

        void compute(List<String> order) {
            order.Reverse();
            foreach (var name in order) {
                var node = data[name];
                switch (data[name].op) {
                    case '+': node.value = data[node.left].value + data[node.right].value; break;
                    case '-': node.value = data[node.left].value - data[node.right].value; break;
                    case '*': node.value = data[node.left].value * data[node.right].value; break;
                    case '/': node.value = data[node.left].value / data[node.right].value; break;
                    default: break;
                }
                if (node.op != 'V') node.humn = data[node.left].humn | data[node.right].humn;
                data[name] = node;
            }
        }

        public string part1() {
            var order = arrange("root");
            compute(order);
            return data["root"].value.ToString();
        }

        public string part2() {
            var order = arrange("root");
            compute(order);
            string key = "root";
            long expected = 0;
            while (key != "humn") {
                bool lefth = data[data[key].left].humn;
                string next = lefth ? data[key].left : data[key].right;
                long otherv = lefth ? data[data[key].right].value : data[data[key].left].value;
                if (key == "root") expected = otherv;
                else switch (data[key].op) {
                        case '+': expected = expected - otherv; break;
                        case '-': expected = lefth ? expected + otherv : otherv - expected; break;
                        case '*': expected = expected / otherv; break;
                        case '/': expected = lefth ? expected * otherv : otherv / expected; break;
                        default: break;
                    }
                key = next;
            }
            return expected.ToString();
        }
    }
}
