 using static Raylib_cs.Raylib;

namespace aoc2022 {
    public class Vis17 : Solution {
        private Day17 solver = new Day17();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        string[] pieces = { "    ", "    ", "    ", "@   ", "    ",
                            "    ", " @  ", "  @ ", "@   ", "    ",
                            "    ", "@@@ ", "  @ ", "@   ", "@@  ",
                            "@@@@", " @  ", "@@@ ", "@   ", "@@  " };

        string[] elephant = { "         _",
                              "      /-/ `\\",
                              ">\\   |@@    |-_______",
                              "\\ \\__/      |        `\\",
                              " \\____/^\\__/         | |",
                              "        |        \\   | h",
                              "        | |\\______'\\  |",
                              "        |_|        |_|"};

        public string part1() {
            ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 24, "Day17");
            Day17.Tetris tetris = new Day17.Tetris(4000);
            Day17.Cyclotron ctron = new Day17.Cyclotron(solver.size);
            int round = 0, w = 0, px = 2, py = tetris.h + 6, startw = 0;
            List<(int, int)> history = new List<(int, int)>();
            long cycle_answer = 0;
            List<(int, int)> epos = new List<(int, int)>();
            renderer.loop(cnt => {
                int p = round % 5;
                int screenh = 43;
                int minh = Math.Max(0, (tetris.h + 7) - screenh);
                renderer.WriteXY(12, screenh - (py - minh) + 3, (cnt % 2 == 0) ? solver.data[w].ToString() : "V");
                for (int i = 0; i < 4; i++) renderer.WriteXY(px + 2, screenh - (py - minh) + i, pieces[i * 5 + p]);
                renderer.WriteXY(1, screenh + 1, "+-------+");
                for (int l = minh; l <= tetris.h + 7; l++) {
                    renderer.WriteXY(1, screenh - (l - minh), "|");
                    for (int j = 0; j < 7; j++) renderer.WriteXY(2 + j, screenh - (l - minh), tetris.tower[l, j].ToString());
                    renderer.WriteXY(9, screenh - (l - minh), "|");
                }
                renderer.WriteXY(1, screenh - (tetris.h - minh), "=");
                renderer.WriteXY(9, screenh - (tetris.h - minh), "=");
                int h, starth = Math.Max(0, history.Count - 20);
                for (h = starth; h < history.Count; h++) {
                    renderer.WriteXY(16, 20 + h - starth, String.Format("Round {0} Start P: {1} Start W: {2} Tower height: {3}",
                                                            h + 1, (h + 1) % 5, history[h].Item1, history[h].Item2));
                }
                renderer.WriteXY(16, 42, String.Format("Cycle length: {0}, matching prefix: {1}", ctron.last_diff, ctron.cycle_len));
                if (cycle_answer > 0) renderer.WriteXY(16, 43, String.Format("Computed asnwer: {0}", cycle_answer));

                if (cnt % 357 == 1) epos.Add((100 + GetRandomValue(0, 13), GetRandomValue(54, 84)));
                for (int e = 0; e < epos.Count; e++) {
                    var (ex, ey) = epos[e];
                    if (cnt % 7 == 0) ey--;
                    if (ey < -10) ey = 100 + GetRandomValue(0, 13);
                    if (cnt % 11 == 0) ex += GetRandomValue(-1, 1);
                    if (ex < 50) ex = 50;
                    if (ex > 100) ex = 100;
                    for (int i = 0; i < elephant.Length; i++) renderer.WriteXY(ex, ey + i, elephant[i]);
                    epos[e] = (ex, ey);
                }
                if (epos.Count > 30) {
                    renderer.WriteXY(16, 41, String.Format("Too many elephants: YES"));
                } else if (epos.Count > 3) {
                    renderer.WriteXY(16, 41, String.Format("Elephants controlling the simulation: {0}", epos.Count));
                }


                renderer.WriteXY(20, 2, "ELFTRIS v0.2022");
                renderer.WriteXY(20, 3, "Expanded Elephant Edition");
                renderer.WriteXY(20, 6, "+-NEXT-+");
                renderer.WriteXY(20, 7, "|      |");
                for (int l = 0; l < 4; l++) {
                    renderer.WriteXY(20, 8 + l, "|      |");
                    renderer.WriteXY(22, 8 + l, pieces[l * 5 + ((p + 1) % 5)]);
                }
                renderer.WriteXY(20, 12, "|      |");
                renderer.WriteXY(20, 13, "+-BLCK-+");

                if (round == 2023) return true;

                // move with the wind
                if (cnt % 2 == 0) {
                    if (solver.data[w] == '<' && !tetris.checkCollisionsX(p, px - 1, py, tetris.ltab)) px--;
                    if (solver.data[w] == '>' && !tetris.checkCollisionsX(p, px + 1, py, tetris.rtab)) px++;
                    w = (w + 1) % solver.size;
                    // drop with gravity
                } else {
                    if (tetris.checkCollisionsY(p, px, py)) {
                        tetris.drop(p, px, py);
                        px = 2;
                        py = tetris.h + 6;
                        history.Add((startw, tetris.h));
                        if (ctron.ready(round, 2022, p, w, tetris.h) && cycle_answer == 0)
                            cycle_answer = tetris.h + ctron.cycle_height;
                        startw = w;
                        round++;
                    } else py--;
                }
                return false;
            });
            return "";
        }

        public string part2() {
            return "";
        }
    }
}
