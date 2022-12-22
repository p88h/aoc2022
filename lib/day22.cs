using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day22 : Solution {
        public char[,] mapp = new char[0, 0];
        public int[] xmax = { }, xmin = { }, ymax = { }, ymin = { }, moves = { };
        public char[] turns = { };
        public int dimx, dimy, dimc;

        public void parse(List<string> input) {
            var directions = input[input.Count - 1];
            dimy = input.Count - 3;
            dimx = input.GetRange(0, input.Count - 2).Select(s => s.Length).Max() - 1;
            dimc = (dimy + 1) / 4;
            mapp = new char[dimy + 1, dimx + 1];
            xmax = new int[dimy + 1]; ymax = new int[dimx + 1]; xmin = new int[dimy + 1]; ymin = new int[dimx + 1];
            for (int y = 0; y <= dimy; y++) for (int x = 0; x <= dimx; x++) mapp[y, x] = x < input[y].Length ? input[y][x] : ' ';
            for (int y = 0; y <= dimy; y++) for (int x = 0; x <= dimx; x++) if (mapp[y, x] != ' ') { xmin[y] = x; break; }
            for (int y = 0; y <= dimy; y++) for (int x = 0; x <= dimx; x++) if (mapp[y, dimx - x] != ' ') { xmax[y] = dimx - x; break; }
            for (int x = 0; x <= dimx; x++) for (int y = 0; y <= dimy; y++) if (mapp[y, x] != ' ') { ymin[x] = y; break; }
            for (int x = 0; x <= dimx; x++) for (int y = 0; y <= dimy; y++) if (mapp[dimy - y, x] != ' ') { ymax[x] = dimy - y; break; }
            moves = Regex.Split(directions, "[LR]").Select(n => int.Parse(n)).ToArray();
            turns = new char[moves.Length - 1];
            for (int i = 0, j = 0; i < directions.Length; i++) if (directions[i] == 'L' || directions[i] == 'R') turns[j++] = directions[i];
        }

        protected (int, int) move1(int x, int y, int dx, int dy) {
            int nx = x + dx, ny = y + dy;
            // wrap around
            if (dx != 0) {
                if (nx > xmax[ny]) nx = xmin[ny];
                if (nx < xmin[ny]) nx = xmax[ny];
            } else {
                if (ny > ymax[nx]) ny = ymin[nx];
                if (ny < ymin[nx]) ny = ymax[nx];
            }
            return (nx, ny);
        }

        public virtual string part1() {
            int x = xmin[0], y = 0, dx = 1, dy = 0;
            for (int i = 0; i < moves.Length; i++) {
                for (int j = 0; j < moves[i]; j++) {
                    var (nx, ny) = move1(x, y, dx, dy);
                    if (mapp[ny, nx] == '#') break;
                    (x, y) = (nx, ny);
                }
                if (i < turns.Length) (dx, dy) = (turns[i] == 'R') ? (-dy, dx) : (dy, -dx);
            }
            int face = dx != 0 ? -(dx - 1) : (2 - dy);
            int ret = 1000 * (y + 1) + 4 * (x + 1) + face;
            return ret.ToString();
        }

        protected (int, int, int, int) move2(int x, int y, int dx, int dy) {
            int nx = x + dx, ny = y + dy, ndx = dx, ndy = dy;
            // wrap around
            if (dx != 0) {
                if (nx > xmax[ny]) {
                    if (ny < dimc) { // B->E+RR
                        nx = 2 * dimc - 1;
                        ny = 3 * dimc - 1 - ny;
                        ndx = -dx;
                    } else if (ny < 2 * dimc) { // C->B+L
                        nx = ny + dimc;
                        ny = dimc - 1;
                        (ndx, ndy) = (dy, -dx);
                    } else if (ny < 3 * dimc) { // E->B+RR
                        nx = 3 * dimc - 1;
                        ny = 3 * dimc - 1 - ny;
                        ndx = -dx;
                    } else { // F->E+L
                        nx = ny - 2 * dimc;
                        ny = 3 * dimc - 1;
                        (ndx, ndy) = (dy, -dx);
                    }
                } else if (nx < xmin[ny]) {
                    if (ny < dimc) { // A->D+RR
                        nx = 0;
                        ny = 3 * dimc - 1 - ny;
                        ndx = -dx;
                    } else if (ny < 2 * dimc) { // C->D+L
                        nx = ny - dimc;
                        ny = 2 * dimc;
                        (ndx, ndy) = (dy, -dx);
                    } else if (ny < 3 * dimc) { // D->A+RR
                        nx = dimc;
                        ny = 3 * dimc - 1 - ny;
                        ndx = -dx;
                    } else { // F->A+L
                        nx = ny - 2 * dimc;
                        ny = 0;
                        (ndx, ndy) = (dy, -dx);
                    }
                }
            } else {
                if (ny > ymax[nx]) {
                    if (nx < dimc) { // F->B
                        ny = 0;
                        nx = nx + 2 * dimc;
                    } else if (nx < 2 * dimc) { // E->F+R
                        ny = nx + 2 * dimc;
                        nx = dimc - 1;
                        (ndx, ndy) = (-dy, dx);
                    } else { // B->C+R
                        ny = nx - dimc;
                        nx = 2 * dimc - 1;
                        (ndx, ndy) = (-dy, dx);
                    }
                } else if (ny < ymin[nx]) {
                    if (nx < dimc) { // D->C+R
                        ny = nx + dimc;
                        nx = dimc;
                        (ndx, ndy) = (-dy, dx);
                    } else if (nx < 2 * dimc) { // A->F+R
                        ny = nx + 2 * dimc;
                        nx = 0;
                        (ndx, ndy) = (-dy, dx);
                    } else { // B->F
                        ny = 4 * dimc - 1;
                        nx = nx - 2 * dimc;
                    }
                }
            }
            return (nx, ny, ndx, ndy);
        }

        public virtual string part2() {
            int x = xmin[0], y = 0, dx = 1, dy = 0;
            for (int i = 0; i < moves.Length; i++) {
                for (int j = 0; j < moves[i]; j++) {
                    var (nx, ny, ndx, ndy) = move2(x, y, dx, dy);
                    if (mapp[ny, nx] == '#') break;
                    (x, y, dx, dy) = (nx, ny, ndx, ndy);
                }
                if (i < turns.Length) (dx, dy) = (turns[i] == 'R') ? (-dy, dx) : (dy, -dx);
            }
            int face = dx != 0 ? -(dx - 1) : (2 - dy);
            int ret = 1000 * (y + 1) + 4 * (x + 1) + face;
            return ret.ToString();
        }
    }
}
