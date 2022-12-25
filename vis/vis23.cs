using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace aoc2022 {
    public class Vis23 : Day23 {
        private ASCIIRay renderer = new ASCIIRay(1080, 1080, 60, 24, "Day23");

        public override string part2() {
            var pos = new List<(int x, int y)>(data);
            int xofs = 4, yofs = 4, maxcnt = 1000000;
            foreach (var (x, y) in pos) scratch[x, y] = 3000;
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
                pos = step(pos, 3000 + cnt, out moved);
                if (!moved && cnt + 300 < maxcnt) maxcnt = cnt + 300;
                return cnt > maxcnt;
            });
            return "";
        }
    }
}
