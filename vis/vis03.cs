namespace aoc2022 {
    public class Vis03 : Solution {
        private List<string> data = new List<string>();
        private string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private ASCIIRay renderer = new ASCIIRay(1280, 720, 15, 22, "Day03");
        private int tot1 = 0, tot2 = 0;
        public void parse(List<string> input) {
            data = input;
        }

        public bool renderPart1(int idx) {
            if (idx >= data.Count) return true;
            string line = data[idx];
            renderer.WriteLine(line);
            renderer.Write("       ");
            HashSet<char> left = new HashSet<char>(), right = new HashSet<char>();
            for (int i = 0; i < line.Length / 2; i++) left.Add(line[i]);
            for (int i = line.Length / 2; i < line.Length; i++) right.Add(line[i]);
            foreach (char c in alphabet) renderer.Write(" " + c);
            renderer.WriteLine("");
            renderer.Write(" left: ");
            foreach (char c in alphabet) renderer.Write("|" + (left.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write("right: ");
            foreach (char c in alphabet) renderer.Write("|" + (right.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write(" same: ");
            char p = ' ';
            foreach (char c in alphabet) {
                if (right.Contains(c) && left.Contains(c)) {
                    renderer.Write("|x");
                    p = c;
                } else {
                    renderer.Write("| ");
                }
            }
            renderer.WriteLine("|");
            renderer.WriteLine("");
            int s = p > 'a' ? p - 'a' + 1 : p - 'A' + 27;
            tot1 += s;
            renderer.WriteLine("priority: " + s);
            renderer.WriteLine(" TOTAL 1: " + tot1);
            renderer.WriteLine("");
            renderPart2(idx);
            return false;
        }

        public bool renderPart2(int idx) {
            int i = idx / 3;
            i *= 3;
            if (i >= data.Count) return true;
            renderer.WriteLine(data[i]);
            renderer.WriteLine(data[i + 1]);
            renderer.WriteLine(data[i + 2]);
            renderer.Write("       ");
            HashSet<char> x = new HashSet<char>(), y = new HashSet<char>(), z = new HashSet<char>();
            foreach (char c in data[i]) x.Add(c);
            foreach (char c in data[i + 1]) y.Add(c);
            foreach (char c in data[i + 2]) z.Add(c);
            foreach (char c in alphabet) renderer.Write(" " + c);
            renderer.WriteLine("");
            renderer.Write("elf 1: ");
            foreach (char c in alphabet) renderer.Write("|" + (x.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write("elf 2: ");
            foreach (char c in alphabet) renderer.Write("|" + (y.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write("elf 3: ");
            foreach (char c in alphabet) renderer.Write("|" + (z.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write(" same: ");
            char p = ' ';
            foreach (char c in alphabet) {
                if (x.Contains(c) && y.Contains(c) && z.Contains(c)) {
                    renderer.Write("|x");
                    p = c;
                } else {
                    renderer.Write("| ");
                }
            }
            renderer.WriteLine("|");
            renderer.WriteLine("");
            int s = p > 'a' ? p - 'a' + 1 : p - 'A' + 27;
            if (idx % 3 == 0) tot2 += s;
            renderer.WriteLine("priority: " + s);
            renderer.WriteLine(" TOTAL 2: " + tot2);
            return false;
        }

        public string part1() {
            renderer.loop(x => renderPart1(x));
            return tot1.ToString();
        }

        public string part2() {
            return tot2.ToString();
        }
    }
}
