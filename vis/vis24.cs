using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.ShaderLocationIndex;

namespace aoc2022 {
    public class Vis24 : Day24 {
        private ASCIIRay renderer = new ASCIIRay(1280,720, 3, 16, "Day24");
        public override string part1() {
            renderer.loop(cnt => {
                for (int y = 0; y < vsize + 2; y++) {
                    renderer.WriteXY(0, y, "#"); 
                    renderer.WriteXY(hsize + 1, y, "#");
                }
                for (int x = 2; x < hsize + 2; x++) renderer.WriteXY(x, 0, "#");
                for (int x = 0; x < hsize; x++) renderer.WriteXY(x, vsize+1, "#");
                int crx = hsize - (cnt % hsize);
                int cry = vsize - (cnt % vsize);
                for (int y = 0; y < vsize; y++) {
                    for (int x = 0; x < hsize; x++) {
                        if (lmap[y, (x + cnt) % hsize] != '.') renderer.WriteXY(x + 1, y + 1, "<");
                        if (rmap[y, (x + crx) % hsize] != '.') renderer.WriteXY(x + 1, y + 1, ">");
                        if (umap[(y + cnt) % vsize, x] != '.') renderer.WriteXY(x + 1, y + 1, "^");
                        if (rmap[(y + cry) % vsize, x] != '.') renderer.WriteXY(x + 1, y + 1, "v");
                    }
                }
                return cnt > 3600;
            });
            return "";
        }
    }
}
