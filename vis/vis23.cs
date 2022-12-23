using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis23 : Day23 {
        private ASCIIRay renderer = new ASCIIRay(1080, 1080, 30, 24, "Day22");

        public override string part2() {
            HashSet<(int x, int y)> pos = new HashSet<(int, int)>(data);
            int xofs = 20, yofs = 20, maxcnt = 1000000;
            renderer.loop(cnt => {
                renderer.WriteXY(1, 1, "Round: " + cnt);
                foreach (var elf in pos) {
                    DrawTriangle(new Vector2((elf.x + xofs) * 7 - 4, (elf.y + yofs) * 7 - 3),
                                 new Vector2((elf.x + xofs) * 7 + 4, (elf.y + yofs) * 7 - 3),
                                 new Vector2((elf.x + xofs) * 7, (elf.y + yofs) * 7 - 9),
                                 GREEN);
                    DrawRectangle((elf.x + xofs) * 7 - 1, (elf.y + yofs) * 7 - 10, 2, 2, RED);
                    DrawCircle((elf.x + xofs) * 7, (elf.y + yofs) * 7, 4, YELLOW);
                    DrawRectangle((elf.x + xofs) * 7 - 3, (elf.y + yofs) * 7 - 1, 2, 2, BLUE);
                    DrawRectangle((elf.x + xofs) * 7 + 1, (elf.y + yofs) * 7 - 1, 2, 2, BLUE);

                }
                bool moved;
                pos = step(pos, cnt, out moved);
                if (!moved && cnt + 300 < maxcnt) maxcnt = cnt + 300;
                return cnt > maxcnt;
            });
            return "";
        }
    }
}
