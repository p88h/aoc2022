namespace aoc2022 {
    public class Vis02 : Solution {
        Day02 solver = new Day02();
        List<string> combs = new List<string>();
        public void parse(List<string> input) {
            solver.parse(input);
        }        

        public string part1() {
            return solver.part1();
        }

        public string part2() {
            ASCIIRay renderer = new ASCIIRay(720, 480, 30, 22, "Day02");
            Dictionary<string, int> scores1 = new Dictionary<string, int>();
            Dictionary<string, int> scores2 = new Dictionary<string, int>();
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (var c1 in "ABC") {
                foreach (var c2 in "XYZ") {
                    string s = c1 + " " + c2;
                    scores1.Add(s, ((c2 - c1 - 1) % 3) * 3 + (c2 - 'X' + 1));
                    scores2.Add(s, ((c1 - 'A' + c2 - 'X' + 2) % 3) + 1 + (c2 - 'X') * 3);
                    counts.Add(s, 0);
                }
            }                        
            string active = "";
            int pos = 0;
            int lag = 0;
            int lagd = 30;
            renderer.loop(cnt => { 
                if (lag==0 && pos < solver.data.Count) {
                    counts[solver.data[pos]]++;
                    active = solver.data[pos];
                    lag = lagd;
                    if (lagd > 0) lagd--;
                    pos++;
                } else if (lag>0) lag--; else active="";
                renderer.WriteXY(6,1,"/-------v---------v---------v-------v---------v---------\\");
                renderer.WriteXY(6,2,"| Input | Value 1 | Value 2 | Count | Total 1 | Total 2 |");
                renderer.WriteXY(6,3,">-------+---------+---------+-------+---------+---------<");
                int y = 3;
                int tot1 = 0, tot2 = 0;
                foreach (var s in scores1.Keys) {
                    renderer.WriteXY(6, ++y,"|       |         |         |       |         |         |");
                    if (s == active) {
                        renderer.SetColor(180,240,180,255);
                        renderer.WriteXY(8, y, s);
                        renderer.SetColor(180,180,180,255);
                    } else {
                        renderer.WriteXY(8, y, s);
                    }
                    renderer.WriteXY(16, y, scores1[s].ToString());
                    renderer.WriteXY(26, y, scores2[s].ToString());
                    renderer.WriteXY(36, y, counts[s].ToString());
                    int v1 = scores1[s]*counts[s];
                    int v2 = scores2[s]*counts[s];
                    renderer.WriteXY(44, y, v1.ToString());
                    renderer.WriteXY(56, y, v2.ToString());
                    tot1 += v1;
                    tot2 += v2;                                        
                }
                renderer.WriteXY(6,++y,"\\-------^---------^---------^-------^---------^---------/");
                renderer.WriteXY(36,++y,"TOTAL");
                renderer.WriteXY(44, y, tot1.ToString());
                renderer.WriteXY(56, y, tot2.ToString());
                renderer.WriteXY(0, 7, "+---+");
                renderer.WriteXY(0, 8, "|   ]>");
                renderer.WriteXY(0, 9, "+---+");
                renderer.SetColor(160,240,160,255);
                renderer.WriteXY(1, 8, active);
                renderer.SetColor(180,180,180,255);
                y = 9;
                for (int j = pos; j < solver.data.Count && j-pos < 12; j++) renderer.WriteXY(1, ++y, solver.data[j]);
                return (cnt > solver.data.Count + 500);
            });
            return solver.part2();
        }
    }
}
