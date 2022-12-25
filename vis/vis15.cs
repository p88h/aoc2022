using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Numerics;

namespace aoc2022 {
    public class Vis15 : Solution {
        private Day15 solver = new Day15();
        public void parse(List<string> input) {
            solver.parse(input);
        }
        static int scale = 4000;
        static int ofsx = 220 * scale;
        static int ofsy = 40 * scale;

        class Scanner {
            int sx, sy, bx, by, bd, cd = 0, cs = 0, alpha = 255, ring = 0;
            public Scanner(int sx, int sy, int bx, int by) {
                this.sx = sx; this.sy = sy; this.bx = bx; this.by = by;
                this.bd = Math.Abs(sx - bx) + Math.Abs(sy - by);
            }
            public void step() {
                cd += cs;
                if (cs < scale * 4) cs += 10;
                if (cd >= bd) {
                    DrawCircle(bx / scale, by / scale, 5, new Color(255, 255, 160, alpha));
                    DrawPoly(new Vector2(sx / scale, sy / scale), 4, (bd / scale), 0, new Color(255, 200, 255, alpha / 2));
                    DrawPolyLines(new Vector2(sx / scale, sy / scale), 4, (bd / scale), 0, new Color(255, 160, 255, alpha));
                    if (alpha > 64) alpha -= 4;
                    if (ring < 20) {
                        ring++;
                        DrawCircleLines(bx / scale, by / scale, 5 + ring, new Color(255, 255, 160, 64 + alpha / 2));
                    }
                } else {
                    DrawPolyLines(new Vector2(sx / scale, sy / scale), 4, (cd / scale), 0, new Color(255, 200, 255, 255));
                    DrawPoly(new Vector2(sx / scale, sy / scale), 4, (cd / scale), 0, new Color(255, 200, 255, 128));
                }
            }
        }

        public string part1() {
            ASCIIRay renderer = new ASCIIRay(1440, 1080, 30, 24, "Day15");
            List<Scanner> scanners = new List<Scanner>();
            foreach (var p in solver.data) scanners.Add(new Scanner(p.sx + ofsx, p.sy + ofsy, p.bx + ofsx, p.by + ofsy));
            renderer.loop(cnt => {
                foreach (var s in scanners) s.step();
                DrawRectangleLines(ofsx / scale, ofsy / scale, 1000, 1000, GREEN);
                return cnt > 1000;
            });
            return "";
        }

        public string part2() {
            return "";
        }
    }
}
