namespace aoc2022 {
    public class Vis06 : Solution {
        private string data = "";
        private int maxpos = 0;
        private int skip = 0;
        private ASCIIRay renderer = new ASCIIRay(1280, 720, 5, 24);
        public void parse(List<string> input) {
            data = input[0];
            maxpos = data.Length;
        }

        public bool render(int idx) {
            idx += skip;
            if (idx >= maxpos + 30) return true;
            if (idx > maxpos) idx = maxpos;
            renderer.SetColor(80,80,80,255);
            HashSet<char> cs = new HashSet<char>();
            int[] counts = new int[256];
            int p = 1;
            for (int i = idx; i < idx+14; i++) p*=++counts[data[i]];
            if (p==1) maxpos = idx;
            for (int i = 0; i< data.Length; i++) {
                if (i >= idx && i < idx + 14) {
                    if (counts[data[i]] == 1) renderer.SetColor(160,250,160,255); 
                    else renderer.SetColor(240,160,160,255); 
                }
                if (i == idx + 14) {
                    renderer.SetColor(80,80,160,255);
                }
                renderer.WriteXY(i%106,i/106,data[i].ToString());
            }
            for (int i = idx; i < idx+14; i++) { p/=counts[data[i]]; counts[data[i]]--; if (p==1) { skip+=(i-idx); break; } }
            return false;
        }

        public string part1() {
            return "";
        }

        public string part2() {
            renderer.loop(x => render(x));
            return (maxpos+14).ToString();
        }
    }
}
