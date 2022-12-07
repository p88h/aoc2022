using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis00 : Solution {
        private Day00 solver = new Day00();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            return solver.part1();
        }

        public string part2() {
            ASCIIRay renderer = new ASCIIRay(800, 600, 30);
            renderer.loop(cnt => { 
                renderer.WriteXY(25,12, "Advent of Code"); 
                renderer.WriteXY(25,14, solver.part1()); 
                renderer.WriteXY(34,14, solver.part2()); 
                return cnt >= 300; 
            });
            return solver.part2();
        }
    }
}
