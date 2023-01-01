using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis23 : Day23 {
        private ASCIIRay renderer = new ASCIIRay(1080, 1080, 60, 24, "Day23");

        public override string part2() {
            int[] pos = data.ToArray();
            int xofs = 4, yofs = 4, maxcnt = 1000000;
            foreach (var code in pos) scratch[code] = 3000;
            renderer.loop(cnt => {
                renderer.WriteXY(1, 1, "Round: " + cnt);
                foreach (var code in pos) {
                    int x = code & 0xFF, y = code >> 8;
                    DrawTriangle(new Vector2((x + xofs) * 7 - 4, (y + yofs) * 7 - 3),
                                 new Vector2((x + xofs) * 7 + 4, (y + yofs) * 7 - 3),
                                 new Vector2((x + xofs) * 7, (y + yofs) * 7 - 9),
                                 GREEN);
                    DrawRectangle((x + xofs) * 7 - 1, (y + yofs) * 7 - 10, 2, 2, RED);
                    DrawCircle((x + xofs) * 7, (y + yofs) * 7, 4, YELLOW);
                    DrawRectangle((x + xofs) * 7 - 3, (y + yofs) * 7 - 1, 2, 2, BLUE);
                    DrawRectangle((x + xofs) * 7 + 1, (y + yofs) * 7 - 1, 2, 2, BLUE);

                }
                bool moved = step(pos, 3000 + cnt);
                if (!moved && cnt + 300 < maxcnt) maxcnt = cnt + 300;
                return cnt > maxcnt;
            });
            return "";
        }
    }
}
