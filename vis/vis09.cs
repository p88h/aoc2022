using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis09 : Solution {
        private Day09 solver = new Day09();
        private ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 24, "Day09");
        public void parse(List<string> input) {
            solver.parse(input);
        }

        HashSet<int> visited1 = new HashSet<int>();
        HashSet<int> visited2 = new HashSet<int>();
        List<Day09.Snake> segments = new List<Day09.Snake>();
        int pos = 0;
        int speed = 1;

        public bool render(int idx) {
            if (speed < 5 && speed <= (idx / 1000)) speed++;
            for (int k = 0; pos < solver.data.Length && k < speed; k++, pos += 2) segments[0].move(solver.data[pos + 1], solver.data[pos]);
            foreach (int p in visited1) {
                int cy = p / 1000, cx = p % 1000;
                DrawRectangle(cx * 3 - 500, cy * 3 - 100, 4, 4, DARKGRAY);
            }
            foreach (int p in visited2) {
                int cy = p / 1000, cx = p % 1000;
                DrawRectangle(cx * 3 - 500, cy * 3 - 100, 4, 4, LIGHTGRAY);
            }
            foreach (var seg in segments) DrawCircle(seg.cx * 3 - 500, seg.cy * 3 - 100, 3, RAYWHITE);
            renderer.WriteXY(2,1,"Visited [1]: " + visited1.Count);
            renderer.WriteXY(2,2,"Visited [2]: " + visited2.Count);
            renderer.WriteXY(2,3,"Speed: " + speed + " cells/frame");
            renderer.WriteXY(2,4,"Moves: " + pos + " / " + solver.data.Length);
            return (idx > solver.data.Length / 5);
        }

        public string part1() {
            return "";
        }

        public string part2() {
            Console.WriteLine(solver.data.Length);
            segments.Add(new Day09.Snake(null, (x, y) => visited2.Add(y * 1000 + x)));
            for (int i = 0; i < 7; i++) segments.Add(new Day09.Snake(segments[segments.Count - 1]));
            segments.Add(new Day09.Snake(segments[segments.Count - 1], (x, y) => visited1.Add(y * 1000 + x)));
            segments.Add(new Day09.Snake(segments[segments.Count - 1]));
            segments.Reverse();
            renderer.loop(x => render(x));
            return solver.part2();
        }
    }
}
