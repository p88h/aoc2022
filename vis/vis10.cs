using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace aoc2022 {
    public class Vis10 : Solution {
        private Day10 solver = new Day10();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            return solver.part1();
        }

        public class CPUX : Day10.CPU {
            public int cycle = 0;
            int sy = 0;
            public List<int[]> spl = new List<int[]>();
            public List<char> disp = new List<char>();
            public override void tick() {
                if (cycle % 40 == 0) sy++;
                int posx = (cycle++ % 40);
                spl.Add(new int[] { sy, x - 1, x, (x + 1) % 40 });
                if (posx == x || posx == x - 1 || posx == (x + 1) % 40) {
                    disp.Add('#');
                } else {
                    disp.Add('.');
                }
            }
        }

        public string part2() {
            ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 24, "Day10");
            Camera3D camera = new Camera3D();
            camera.Target = new Vector3(19.0f, 0.0f, 4.0f);
            camera.Position = new Vector3(19.0f, 25.0f, 16.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 45.0f;
            camera.Projection = CameraProjection.Perspective;
            // generate sprite position list
            CPUX cpu = new CPUX();
            foreach (var ins in solver.program) cpu.exec(ins);
            Console.WriteLine(cpu.spl.Count);
            renderer.loop(cnt => {
                int cycle = cnt / 10;
                int idx = cnt % 10;
                int px = cycle % 40;
                int py = 1 + cycle / 40;
                float rx = (float)px + idx / 10.0f;
                float rz = 0.5f + (float)Math.Sin(Math.PI * idx / 10.0f);
                BeginMode3D(camera);
                if (cycle < cpu.spl.Count) {
                    float sy = cpu.spl[cycle][0];
                    float[] sx = new float[3] { cpu.spl[cycle][1], cpu.spl[cycle][2], cpu.spl[cycle][3] };
                    if (idx != 0 && cycle + 1 < cpu.spl.Count) {
                        float[] sx2 = new float[3] { cpu.spl[cycle + 1][1], cpu.spl[cycle + 1][2], cpu.spl[cycle + 1][3] };
                        sx[0] = (sx[0] * (10.0f - idx) + sx2[0] * idx) / 10.0f;
                        sx[1] = (sx[1] * (10.0f - idx) + sx2[1] * idx) / 10.0f;
                        sx[2] = (sx[2] * (10.0f - idx) + sx2[2] * idx) / 10.0f;
                    }
                    DrawCube(new Vector3(sx[0], 0, sy), 1, 0.1f, 1, Color.Blue);
                    DrawCube(new Vector3(sx[1], 0, sy), 1, 0.1f, 1, Color.Blue);
                    DrawCube(new Vector3(sx[2], 0, sy), 1, 0.1f, 1, Color.Blue);
                    DrawCube(new Vector3(rx, rz, py), 1, 1, 1, Color.Red);
                    DrawCubeWires(new Vector3(rx, rz, py), 1, 1, 1, Color.Black);
                }
                for (int d = 0; d <= cycle && d < cpu.disp.Count; d++) {
                    int dx = d % 40;
                    int dy = 1 + d / 40;
                    if (cpu.disp[d] == '#') {
                        DrawCube(new Vector3(dx, 0, dy), 0.8f, 0.2f, 0.8f, Color.Green);
                        DrawCubeWires(new Vector3(dx, 0, dy), 0.8f, 0.2f, 0.8f, Color.Brown);
                    }
                }
                EndMode3D();
                return cnt > 2500;
            });
            return solver.part2();
        }
    }
}
