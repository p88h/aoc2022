using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace aoc2022 {
    public class Vis11 : Solution {
        private Day11 solver = new Day11();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            // record all 'flight paths' first
            List<List<(int, long)>> paths = new List<List<(int, long)>>();
            for (int i = 0; i < solver.items.Count; i++) paths.Add(new List<(int, long)>());
            solver.run(20, (x, c, i) => { paths[i].Add((c, x)); return x / 3; });
            int maxcnt = paths.Select(x => x.Count).Max();
            Console.WriteLine(maxcnt);
            Viewer viewer = new Viewer(1920, 1080, 60, "Day11");
            Camera3D camera = new Camera3D();
            Model model = LoadModel("resources/CartoonMonkeyModel.obj");
            Model cube = LoadModelFromMesh(GenMeshCube(1.0f, 1.0f, 1.0f));
            viewer.setupLights(solver.monkeys.Count * 4.0f + 2, 4, 16, new List<Model> { model, cube });
            Texture2D texture = LoadTexture("resources/Monkey_Diffuse.png");
            SetMaterialTexture(ref model, 0, MaterialMapIndex.Diffuse, ref texture);

            camera.Target = new Vector3(14.5f, 4.0f, 0.0f);
            camera.Position = new Vector3(15.0f, 12.0f, 19.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 45.0f;
            camera.Projection = CameraProjection.Perspective;
            UpdateCamera(ref camera, CameraMode.Free);
            // generate sprite position list
            int factor = 60;
            viewer.loop(cnt => {
                BeginMode3D(camera);
                float pz0 = (float)Math.Sin(Math.PI * (cnt % factor) / factor);
                int[] stacks = new int[solver.monkeys.Count];
                for (int i = 0; i < paths.Count; i++) {
                    int idx = (cnt + i) / factor;
                    int ofs = (cnt + i) % factor;
                    if (idx + 1 >= paths[i].Count) {
                        (int fx, _) = paths[i][paths[i].Count - 1];
                        stacks[fx]++;
                        continue;
                    }
                    (float px2, _) = paths[i][idx + 1];
                    (float px1, long w) = paths[i][idx];
                    float px = 4.0f * (px1 * (factor - ofs) + px2 * ofs) / (float)factor;
                    float pz = (float)Math.Sin(Math.PI * ofs / factor) * (float)Math.Log2(w);
                    if (idx + 1 == paths[i].Count) pz = pz0;
                    DrawModel(cube, new Vector3(px, pz, 0), 1f, Color.Blue);
                }
                for (int i = 0; i < solver.monkeys.Count; i++) {
                    DrawModel(model, new Vector3(i * 4.0f, pz0 - 4.0f, 0), 0.16f, Color.White);
                    for (int j = 0; j < stacks[i]; j++)
                        DrawModel(cube, new Vector3(i * 4.0f + (j % 2) * 0.8f, -4.0f + (j / 2) * 0.8f, -2), 0.8f, Color.Blue);
                }
                EndMode3D();
                return cnt > (maxcnt * factor + 300);
            });
            UnloadModel(model);
            UnloadModel(cube);
            return solver.part1();
        }

        public string part2() {
            return solver.part2();
        }
    }
}
