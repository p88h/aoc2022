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
                        if (mapp[y, (x + cnt) % hsize] == '>') renderer.WriteXY(x + 1, y + 1, "<");
                        if (mapp[y, (x + crx) % hsize] == '>') renderer.WriteXY(x + 1, y + 1, ">");
                        if (mapp[(y + cnt) % vsize, x] == '^') renderer.WriteXY(x + 1, y + 1, "^");
                        if (mapp[(y + cry) % vsize, x] == 'v') renderer.WriteXY(x + 1, y + 1, "v");
                    }
                }
                return cnt > 3600;
            });
            return "";
        }
    }
}
