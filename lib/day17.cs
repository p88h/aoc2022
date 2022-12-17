using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day17 : Solution {

        public string data = "";
        public int size;

        public void parse(List<string> input) {
            data = input[0];
            size = data.Length;
        }

        public class Tetris {
            public char[,] tower;
            public int h = 0;

            public int[] wtab = { 4, 3, 3, 1, 2 };
            public int[,] btab = { { 4, 4, 4, 4 }, { 3, 4, 3, 0 }, { 4, 4, 4, 0 }, { 4, 0, 0, 0 }, { 4, 4, 0, 0 } };
            public int[,] rtab = { { 0, 0, 0, 4 }, { 0, 2, 3, 2 }, { 0, 3, 3, 3 }, { 1, 1, 1, 1 }, { 0, 0, 2, 2 } };
            public int[,] ltab = { { 0, 0, 0, 1 }, { 0, 2, 1, 2 }, { 0, 3, 3, 1 }, { 1, 1, 1, 1 }, { 0, 0, 1, 1 } };
            public int[,] htab = { { 4, 4, 4, 4 }, { 3, 2, 3, 0 }, { 4, 4, 2, 0 }, { 1, 0, 0, 0 }, { 3, 3, 0, 0 } };

            public Tetris(int maxh) {
                tower = new char[maxh, 7];
                for (int y = 0; y < maxh; y++) for (int x = 0; x < 7; x++) tower[y, x] = ' ';
            }

            public bool simpleCheckX(int px, int py, int p) { return (px < 0 || px > 7 - wtab[p]); }

            public bool checkCollisionsY(int p, int px, int py) {
                if (py == 3) return true;
                for (int i = 0; i < 4; i++) if (btab[p, i] > 0 && tower[py - btab[p, i], px + i] == '#') return true;
                return false;
            }
            public bool checkCollisionsX(int p, int px, int py, int[,] xtab) {
                if (px < 0 || px > 7 - wtab[p]) return true;
                for (int i = 0; i < 4; i++) if (xtab[p, i] > 0 && tower[py - i, px + xtab[p, i] - 1] == '#') return true;
                return false;
            }
            public void drop(int p, int px, int py) {
                for (int col = 0; col < 4; col++) {
                    if (htab[p, col] > 0) {
                        if (py - htab[p, col] + 2 >= h) h = py - htab[p, col] + 2;
                        for (int row = htab[p, col]; row <= btab[p, col]; row++) tower[py - row + 1, px + col] = '#';
                    }
                }
            }
        }

        public class Cyclotron {
            int[] last_seen, last_height;
            public long cycle_len = 0, last_diff = 0, cycle_height = 0;
            public Cyclotron(int size) {
                last_seen = new int[size * 32];
                last_height = new int[size * 32];
            }
            public bool ready(int round, long rounds, int p, int w, int h) {
                int code = (w << 5) + p;
                if (last_seen[code] != 0) {
                    long dt = round - last_seen[code];
                    long dh = h - last_height[code];
                    if (last_diff == 0) last_diff = dt;
                    if (last_diff == dt) cycle_len++;
                    if (cycle_len > 10 && (round % last_diff) == ((rounds - 1) % last_diff)) {
                        long nrounds = (rounds - round) / last_diff;
                        cycle_height = nrounds * dh;
                        return true;
                    }
                } else cycle_len = last_diff = 0;
                last_seen[code] = round;
                last_height[code] = h;
                return false;
            }
        }

        long play(long rounds) {
            int w = 0;
            Tetris tetris = new Tetris(8000);
            Cyclotron ctron = new Cyclotron(size);
            for (int round = 0; round < rounds; round++) {
                int p = round % 5;
                // top-left char of new piece should now be at x=2 y=6 + 1 to start from falling.
                int px = 2, py = tetris.h + 7;
                // we can fall 4 times and shift 4 times without 'complicated' checks
                for (int i = 0; i < 4; i++) {
                    py--;
                    if (data[w] == '<' && !tetris.simpleCheckX(px - 1, py, p)) px--;
                    if (data[w] == '>' && !tetris.simpleCheckX(px + 1, py, p)) px++;
                    w = (w + 1) % size;
                }
                while (!tetris.checkCollisionsY(p, px, py)) {
                    py--;
                    if (data[w] == '<' && !tetris.checkCollisionsX(p, px - 1, py, tetris.ltab)) px--;
                    if (data[w] == '>' && !tetris.checkCollisionsX(p, px + 1, py, tetris.rtab)) px++;
                    w = (w + 1) % size;
                }
                tetris.drop(p, px, py);
                if (ctron.ready(round, rounds, p, w, tetris.h)) break;
            }
            return tetris.h + ctron.cycle_height;
        }

        public string part1() {
            return play(2022).ToString();
        }

        public string part2() {
            return play(1000000000000).ToString();
        }
    }
}
