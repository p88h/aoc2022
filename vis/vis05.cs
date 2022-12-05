namespace aoc2022 {
    public class Vis05 : Solution {
        private Day05 solver = new Day05();
        private Day05.State? s1, s2;
        private List<int[]> single_moves = new List<int[]>();
        private ASCIIRay renderer = new ASCIIRay(1280, 800, 30, 22);
        public void parse(List<string> input) {
            solver.parse(input);
            foreach (var move in solver.moves) for (int i = 0; i < move[0]; i++) single_moves.Add(new int[] { move[1], move[2] });
        }

        public bool render(int idx) {
            int n = (idx / 2);
            if (n > single_moves.Count + 60) return true;
            for (int s = 0; s < s1!.stacks.Count; s++) {
                for (int i = 0; i < s1.sizes[s]; i++) {
                    renderer.WriteXY(5 + s * 4, 34 - i, "[" + s1.stacks[s][i] + "] ");
                }
                if (n < single_moves.Count && s == single_moves[n][idx % 2]) {
                    for (int i = s1.sizes[s]; i < 34; i++) renderer.WriteXY(6 + s * 4, 34 - i, "|");
                    for (int j = s; j < 10; j++) renderer.WriteXY(6 + j * 4, 0, "####");
                }
            }
            for (int j = 0; j < 34; j++) renderer.WriteXY(44, j, "#");
            renderer.WriteXY(45, 32, ".--*.");
            renderer.WriteXY(45, 33, "| /\\|");
            if ((idx / 3) % 2 == 0) renderer.WriteXY(45, 34, "|>@@|"); else renderer.WriteXY(45, 34, "|=@@|");
            renderer.WriteXY(45, 35, "*****");
            for (int s = 0; s < s2!.stacks.Count; s++) {
                for (int i = 0; i < s2.sizes[s]; i++) {
                    renderer.WriteXY(70 + s * 4, 34 - i, "[" + s2.stacks[s][i] + "] ");
                }
                if (n < solver.moves.Count && s == solver.moves[n][1 + idx % 2]) {
                    for (int i = s2.sizes[s]; i < 34; i++) renderer.WriteXY(71 + s * 4, 34 - i, "|");
                    for (int j = 0; j <= s + 1; j++) renderer.WriteXY(64 + j * 4, 0, "####");
                }
            }
            for (int j = 0; j < 34; j++) renderer.WriteXY(65, j, "#");
            renderer.WriteXY(60, 32, ".-*-.");
            renderer.WriteXY(60, 33, "|/\\ |");
            if (n < solver.moves.Count && (idx / 3) % 2 == 1) renderer.WriteXY(60, 34, "|@@<|"); else renderer.WriteXY(60, 34, "|@@=|");
            renderer.WriteXY(60, 35, "*****");
            renderer.WriteXY(5, 35, "CrateMover 9000");
            renderer.WriteXY(70, 35, "CrateMover 9001");
            if (idx % 2 == 0) {
                if (n < single_moves.Count) s1!.move1(1, single_moves[n][0], single_moves[n][1]);
                if (n < solver.moves.Count) s2!.move2(solver.moves[n][0], solver.moves[n][1], solver.moves[n][2]);
            }
            return false;
        }


        public string part1() {
            s1 = new Day05.State(solver.initial!);
            s2 = new Day05.State(solver.initial!);
            renderer.loop(x => render(x));
            return solver.part1();
        }

        public string part2() {
            return solver.part2();
        }
    }
}
