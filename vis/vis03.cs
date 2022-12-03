namespace aoc2022 {
    public class Vis03 : Solution {
        private List<string> data = new List<string>();
        private string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private ASCIIRay renderer = new ASCIIRay(1920, 540, 15);
        private int tot = 0;
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
            renderer.Write("left:  ");
            foreach (char c in alphabet) renderer.Write("|" + (left.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write("right: ");
            foreach (char c in alphabet) renderer.Write("|" + (right.Contains(c) ? 'x' : ' '));
            renderer.WriteLine("|");
            renderer.Write("same:  ");
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
            tot += s;
            renderer.WriteLine("priority: " + s);
            renderer.WriteLine("TOTAL:    " + tot);
            return false;
        }

        public string part1() {
            tot = 0;
            renderer.loop(10, x => renderPart1(x));
            return tot.ToString();
        }

        public bool renderPart2(int i) {
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
            renderer.Write("same:  ");
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
            tot += s;
            renderer.WriteLine("priority: " + s);
            renderer.WriteLine("TOTAL:    " + tot);
            return false;
        }

        public string part2() {
            tot = 0;
            renderer.loop(10, x => renderPart2(x));
            return tot.ToString();
        }
    }
}
