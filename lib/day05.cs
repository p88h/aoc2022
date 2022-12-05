namespace aoc2022 {
    public class Day05 : Solution {
        public List<int[]> moves = new List<int[]>();

        public class State {
            public List<char[]> stacks = new List<char[]>();
            public int[] sizes;
            public State(int nstacks, int maxn) {
                sizes = new int[nstacks];
                for (int k = 0; k < nstacks; k++) {
                    stacks.Add(new char[nstacks * maxn]);
                    sizes[k] = 0;
                }
            }
            public State(State other) {
                sizes = (int[])other.sizes.Clone();
                foreach (var stack in other.stacks) stacks.Add((char[])stack.Clone());
            }

            public void move1(int x, int a, int b) {
                int l = sizes[a];
                for (int i = 0; i < x; i++) stacks[b][sizes[b]++] = stacks[a][l - i - 1];
                sizes[a] -= x;
            }

            public void move2(int x, int a, int b) {
                int l = sizes[a];
                Array.Copy(stacks[a], sizes[a] - x, stacks[b], sizes[b], x);
                sizes[a] -= x; sizes[b] += x;
            }

            public String top() {
                string ret = "";
                for (int i = 0; i < stacks.Count(); i++) ret += stacks[i][sizes[i] - 1];
                return ret;
            }
        }

        private State? initial;

        public void parse(List<string> input) {
            int i;
            for (i = 0; i < input.Count(); i++) if (input[i].Length < 2) break;
            int nstacks = (input[i - 1].Length + 1) / 4;
            initial = new State(nstacks, i - 1);
            for (int j = i - 2; j >= 0; j--) {
                for (int k = 0; k < nstacks; k++) {
                    if (input[j][k * 4 + 1] != ' ') initial.stacks[k][initial.sizes[k]++] = input[j][k * 4 + 1];
                }
            }
            for (int j = i + 1; j < input.Count(); j++) {
                var parts = input[j].Split(' ');
                moves.Add(new int[] { int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1 });
            }
        }


        public string part1() {
            State tmp = new State(initial!);
            foreach (var move in moves) tmp.move1(move[0], move[1], move[2]);
            return tmp.top();
        }

        public string part2() {
            State tmp = new State(initial!);
            foreach (var move in moves) tmp.move2(move[0], move[1], move[2]);
            return tmp.top();
        }
    }
}
