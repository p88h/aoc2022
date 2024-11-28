using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace aoc2022 {
    public class Vis21 : Solution {
        private ASCIIRay renderer = new ASCIIRay(1920, 1080, 30, 24, "Day21");

        Day21 solver = new Day21();

        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            return "";
        }

        public class Simulator {
            public string start, tag;
            List<string> order = new List<string>();
            public List<string> stack = new List<string>();
            Dictionary<string, (double, double)> precomputed = new Dictionary<string, (double, double)>();
            private Dictionary<string, Day21.Monkey> data;
            ASCIIRay renderer;
            public int ymax = 0, xmax = 156;


            public Simulator(string from, Day21 solver, ASCIIRay renderer, string tag, int ypos) {
                start = from;
                solver.compute(start, order);
                foreach (var key in order) precomputed[key] = solver.compute(key);
                order.Reverse();
                data = solver.data;
                this.renderer = renderer;
                this.tag = tag;
            }

            public bool step(int cnt, int ypos) {
                int px = tag.Length + 8, py = ypos;
                renderer.WriteXY(0, ypos, tag + " STACK: ");
                for (int i = 0; i < stack.Count; i++) {
                    var (h, v) = precomputed[stack[i]];
                    string label = (h != 0) ? "(H*" + h.ToString() + " + " + v.ToString() + ")" : "(" + v.ToString() + ")";
                    if (px + label.Length > xmax) { py++; px = 4; }
                    renderer.WriteXY(px, py, label);
                    px += label.Length + 1;
                }
                px = tag.Length + 8; py = ymax = ypos + 4;
                if (cnt >= order.Count) return true;
                renderer.WriteXY(0, py, tag + " INPUT: ");
                for (int i = cnt; i < order.Count; i++) {
                    var key = order[i];
                    string label = data[key].op;
                    if (label[0] == 'V') label = data[key].value.ToString();
                    if (key == "humn") label = "H";
                    if (px + label.Length > xmax) { py++; px = 4; ymax = py; }
                    renderer.WriteXY(px, py, label);
                    px += label.Length + 1;
                }
                string current = order[cnt];
                if (data[current].op[0] != 'V') {
                    stack.RemoveAt(stack.Count - 1);
                    stack.RemoveAt(stack.Count - 1);
                }
                stack.Add(current);
                return false;
            }
        }

        public string part2() {
            Simulator left = new Simulator(solver.data["root"].left, solver, renderer, "LEFT", 0);
            Simulator right = new Simulator(solver.data["root"].right, solver, renderer, "RIGHT", 24);
            int maxcnt = 10000000;
            Camera3D camera = new Camera3D();
            Model model = LoadModel("resources/CartoonMonkeyModel.obj");
            renderer.viewer.setupLights(10, 4, 4, new List<Model> { model });
            Texture2D texture = LoadTexture("resources/Monkey_Diffuse.png");
            SetMaterialTexture(ref model, 0, MaterialMapIndex.Diffuse, ref texture);

            camera.Target = new Vector3(14.5f, 4.0f, 0.0f);
            camera.Position = new Vector3(15.0f, 12.0f, 19.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 45.0f;
            camera.Projection = CameraProjection.Perspective;
            UpdateCamera(ref camera, CameraMode.Free);
            int factor = 60;
            renderer.loop(cnt => {
                float pz0 = (float)Math.Sin(Math.PI * (cnt % factor) / factor);
                BeginMode3D(camera);
                DrawModelEx(model, new Vector3((cnt / 100.0f) - 2, pz0 - 6.0f, 0), new Vector3(0, 1, 0), (float)cnt, new Vector3(0.16f, 0.16f, 0.16f), Color.White);
                EndMode3D();
                bool done1 = left.step(cnt, 0);
                bool done2 = right.step(cnt, left.ymax + 2);
                if (done1 && done2) {
                    var (lh, lv) = solver.compute(solver.data["root"].left);
                    var (rh, rv) = solver.compute(solver.data["root"].right);
                    string ll = (lh != 0) ? "H*" + lh.ToString() + " + " + lv.ToString() : lv.ToString();
                    string rl = (rh != 0) ? "H*" + rh.ToString() + " + " + rv.ToString() : rv.ToString();
                    renderer.WriteXY(0, right.ymax, ll + " == " + rl);
                    if (cnt > maxcnt - 240) {
                        if (rh != 0) (lv, rv) = (rv, lv);
                        renderer.WriteXY(0, right.ymax + 1, "H == (" + rv.ToString() + " - " + lv.ToString() + ") / " + (lh + rh).ToString());
                    }
                    if (cnt > maxcnt - 180) {
                        var ret = (lh != 0) ? ((rv - lv) / lh) : ((lv - rv) / rh);
                        renderer.WriteXY(0, right.ymax + 2, "H == " + Math.Round(ret).ToString());
                    }
                    if (maxcnt > cnt + 300) maxcnt = cnt + 300;
                }
                return cnt >= maxcnt;
            });
            return "";
        }
    }
}
