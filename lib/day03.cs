namespace aoc2022 {
    public class Day03 : Solution {
        private List<string> data = new List<string>();
        public void parse(List<string> input) {
            data = input;
        }

        public string part1() {
            int sum = 0;            
            foreach (var n in data) {
                char p = ' ';
                HashSet<char> left = new HashSet<char>();
                for (int j = 0; j < n.Length/2; j++) left.Add(n[j]);
                for (int j = n.Length/2; j < n.Length; j++) if (left.Contains(n[j])) p = n[j];
                if (p > 'a') sum += p - 'a' + 1; else sum += p - 'A' + 27;
            }
            return sum.ToString();
        }

        public string part2() {
            int sum = 0;            
            for (int i = 0; i < data.Count; i+=3) {
                char p = ' ';
                Dictionary<char, int> counts = new Dictionary<char, int>();
                foreach (char c in data[i]) { counts[c]=counts.GetValueOrDefault(c)|1; }
                foreach (char c in data[i+1]) { counts[c]=counts.GetValueOrDefault(c)|2; }
                foreach (char c in data[i+2]) { counts[c]=counts.GetValueOrDefault(c)|4; if (counts[c]==7) p =c; }
                if (p > 'a') sum += p - 'a' + 1; else sum += p - 'A' + 27;
            }
            return sum.ToString();
        }
    }
}
