using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis07 : Solution {
        private Day07 solver = new Day07();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            return solver.part1();
        }

        public string path(Day07.Node node) {
            if (node.parent == null) return "";
            return path(node.parent) + "/" + node.name;
        }

        public void getAllFiles(Day07.Node node, List<int> collect) {
            foreach (var child in node.entries.Values) getAllFiles(child, collect);
            collect.Add(node.index);
        }

        public string part2() {
            ASCIIRay renderer = new ASCIIRay(1280, 720, 30, 24, "Day07");
            int[] start = new int[solver.allNodes.Count];
            int[] end = new int[solver.allNodes.Count];
            Color[] colors = new Color[solver.allNodes.Count];
            Random rnd = new Random();
            foreach (var node in solver.allNodes) {
                if (node.parent == null) {
                    start[0] = end[0] = 0;
                } else {
                    end[node.index] = start[node.index] = end[node.parent.index];                    
                    end[node.parent.index] += (node.size + 1599) / 1600;
                }
                colors[node.index] = new Color(rnd.Next(256),rnd.Next(256),rnd.Next(256),255);
            }
            long freeSpace = 70000000 - solver.allNodes[0].size;
            long minNeeded = 30000000 - freeSpace;
            Day07.Node min = solver.allNodes[0];
            foreach (Day07.Node n in solver.allNodes) if (n.size < min.size && n.size > minNeeded) min = n;
            List<int> toRemove = new List<int>();
            getAllFiles(min, toRemove);
            int startRemoveAt = solver.allNodes.Count + 30;

            renderer.loop(cnt => { 
                for (int i = 0; i < cnt && i < solver.allNodes.Count; i ++) {
                    Day07.Node node = solver.allNodes[i];
                    if (node.entries.Count > 0) continue;
                    for (int j = 0; j < (node.size + 1599)/1600; j++) {
                        long cx = (start[node.index] + j) % 256;
                        long cy = (start[node.index] + j) / 256;
                        DrawRectangle((int)cx*5,40+(int)cy*5,4,4,colors[node.index]);
                    }
                }
                if (cnt < solver.allNodes.Count) {
                    renderer.WriteXY(0,0, "ls " + path(solver.allNodes[cnt]) + " : " + solver.allNodes[cnt].size);
                } else if (cnt > startRemoveAt && cnt < startRemoveAt + toRemove.Count) {
                    int idx = toRemove[cnt - startRemoveAt];
                    renderer.WriteXY(0,0, "rm " + path(solver.allNodes[idx]));
                    colors[idx] = BLACK;
                }
                return cnt > startRemoveAt + toRemove.Count + 30;
            });
            return solver.part2();
        }
    }
}
