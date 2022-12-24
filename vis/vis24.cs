namespace aoc2022 {
    public class Vis24 : Day24 {
        private ASCIIRay renderer = new ASCIIRay(1280, 720, 10, 16, "Day24");
        public override string part2() {
            List<(int t, int x, int y)> trace = new List<(int, int, int)>();
            int ofsx = 4, ofsy = 10, tpos = 0, ret = trip(0, -1, hsize - 1, vsize, 0, trace);
            int ret2 = trip(hsize - 1, vsize, 0, -1, ret, trace);
            int ret3 = trip(0, -1, hsize - 1, vsize, ret2, trace);
            int lastnum = 0, lost = 0;
            renderer.loop(cnt => {
                if (cnt < 100) return false;
                cnt -= 100;
                if (cnt > ret3 + 100) return true;
                if (cnt > ret3) cnt = ret3;
                renderer.SetColor(160, 160, 160, 255);
                for (int y = 0; y < vsize + 2; y++) {
                    renderer.WriteXY(ofsx, y + ofsy, "#");
                    renderer.WriteXY(hsize + 1 + ofsx, y + ofsy, "#");
                }
                for (int x = 2; x < hsize + 2; x++) renderer.WriteXY(x + ofsx, ofsy, "#");
                for (int x = 0; x < hsize; x++) renderer.WriteXY(x + ofsx, vsize + 1 + ofsy, "#");
                int crx = hsize - (cnt % hsize);
                int cry = vsize - (cnt % vsize);
                renderer.SetColor(100, 100, 180, 255);
                for (int y = 0; y < vsize; y++) {
                    for (int x = 0; x < hsize; x++) {
                        if (mapp[y, (x + cnt) % hsize] == '>') renderer.WriteXY(x + 1 + ofsx, y + 1 + ofsy, "<");
                        if (mapp[y, (x + crx) % hsize] == '>') renderer.WriteXY(x + 1 + ofsx, y + 1 + ofsy, ">");
                        if (mapp[(y + cnt) % vsize, x] == '^') renderer.WriteXY(x + 1 + ofsx, y + 1 + ofsy, "^");
                        if (mapp[(y + cry) % vsize, x] == 'v') renderer.WriteXY(x + 1 + ofsx, y + 1 + ofsy, "v");
                    }
                }
                renderer.SetColor(250, 250, 200, 255);
                int num = 0;
                while (tpos < trace.Count && trace[tpos].t == cnt) {
                    renderer.WriteXY(trace[tpos].x + 1 + ofsx, trace[tpos].y + 1 + ofsy, "@");
                    tpos++; num++;
                }
                if (num < lastnum) lost+=lastnum-num;
                lastnum = num;
                renderer.WriteXY(ofsx, ofsy - 2, "TIME: " + cnt);
                renderer.WriteXY(ofsx, vsize + ofsy + 3, "@ ACTIVE: " + num);
                if (lost>0) renderer.WriteXY(ofsx, vsize + ofsy + 4, "@ LOST IN THE BLIZZARD: " + lost);
                if (cnt > ret) renderer.WriteXY(ofsx, vsize + ofsy + 6, "SNACKS LOST: YES"); else
                if (cnt > ret2) renderer.WriteXY(ofsx, vsize + ofsy + 6, "SNACKS FOUND: YES");
                return false;
            });
            return "";
        }
    }
}
