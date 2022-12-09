namespace aoc2022 {
    public class Day09 : Solution {
        public int[] data = { };
        public void parse(List<string> input) {
            List<int> values = input.Select(s => int.Parse(s.Substring(2))).ToList();
            data = new int[values.Sum() * 2];
            int k = 0;
            for (int i = 0; i < input.Count; i++) {
                for (int j = 0; j < values[i]; j++) {
                    switch (input[i][0]) {
                        case 'L': data[k] = -1; break;
                        case 'R': data[k] = 1; break;
                        case 'U': data[k + 1] = -1; break;
                        case 'D': data[k + 1] = 1; break;
                        default: break;
                    }
                    k += 2;
                }
            }
        }

        public class Snake {
            public int cx=200, cy=200;
            Snake? next;
            Action<int, int>? trace;
            public Snake(Snake? n = null, Action<int, int>? t = null) {
                next = n;
                trace =t;
                if (trace != null) trace(cx, cy);
            }

            public void move(int dx, int dy) {
                cx += dx; cy += dy;
                if (next != null) next.chase(cx, cy);
            }

            void chase(int px, int py) {
                int dx = px - cx, dy = py - cy, ax = Math.Abs(dx), ay = Math.Abs(dy);
                if (ax > 1 || ay > 1) {
                    if (ax > 0) cx += dx < 0 ? -1 : 1; 
                    if (ay > 0) cy += dy < 0 ? -1 : 1; 
                    if (trace != null) trace(cx, cy);
                    if (next != null) next.chase(cx, cy);
                }
            }
        }

        public string part1() {            
            HashSet<int> visited = new HashSet<int>();
            Snake tail = new Snake(null, (x,y) => visited.Add(y*1000+x));
            Snake head = new Snake(tail);
            for (int i = 0; i < data.Length; i += 2) head.move(data[i], data[i+1]);
            return visited.Count.ToString();
        }


        public string part2() {
            HashSet<int> visited = new HashSet<int>();
            Snake head = new Snake(null, (x,y) => visited.Add(y*1000+x));
            for (int i = 0; i < 9; i++) head = new Snake(head);
            for (int i = 0; i < data.Length; i += 2) head.move(data[i], data[i+1]);
            return visited.Count.ToString();
        }
    }
}
