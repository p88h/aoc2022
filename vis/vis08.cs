using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;


namespace aoc2022 {
    public class Vis08 : Solution {
        private Day08 solver = new Day08();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            return solver.part1();
        }


        public string part2() {
            ASCIIRay renderer = new ASCIIRay(1280, 720, 30, 24, "Day08");
            Camera3D camera = new Camera3D();
            camera.target = new Vector3(50.0f, 0.0f, 50.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;
            List<int[]> data = solver.data;
            List<Color[]> colors = new List<Color[]>();
            int dim = data.Count;
            for (int i = 0; i < dim; i++) {
                colors.Add(new Color[dim]);
                for (int j = 0; j < dim; j++) colors[i][j] = new Color(80, 50 + data[i][j] * 20, 80, 255);
            }

            renderer.loop(cnt => {
                camera.position = new Vector3((float)Math.Cos(cnt / 300.0f) * 90.0f + 50, 
                                              (float)Math.Cos(cnt / 150.0f) * 10.0f + 10.0f,
                                              (float)Math.Sin(cnt / 300.0f) * 90.0f + 50.0f);
                BeginMode3D(camera);
                float maxh = (cnt / 30.0f) + 1;
                for (int i = 0; i < dim; i++) {
                    for (int j = 0; j < dim; j++) {
                        float h = data[i][j];
                        if (h > maxh) h = maxh;
                        float stump = 1 + (h - 1) / 3;
                        float b = stump;
                        h -= stump;
                        float m = 1 + h / 3;
                        while (h > 0.1) {
                            float r = h;
                            if (r > m) r = m;
                            m -= 0.5f;
                            float d = r / 2;
                            if (d > 1.0f) d = 1.0f;
                            if (d < 0.3f) d = 0.3f;
                            DrawCylinder(new Vector3(i, b, j), 0.1f, d, r, 6, colors[i][j]);
                            DrawCylinderWires(new Vector3(i, b, j), 0.1f, d, r, 6, DARKGRAY);
                            b += r;
                            h -= r;
                        }
                        DrawCylinder(new Vector3(i, 0, j), 0.2f, 0.2f, stump, 8, BROWN);
                        DrawCylinderWires(new Vector3(i, 0, j), 0.2f, 0.2f, stump, 8, BLACK);
                    }
                }
                if (cnt >= 300 && cnt < 300 + dim) {
                    int i = cnt - 300;
                    int maxlr = 0, maxrl = 0, maxtb = 0, maxbt = 0;
                    for (int j = 0; j < dim; j++) {
                        if (data[i][j] > maxlr) {
                            maxlr = data[i][j];
                            colors[i][j] = new Color(50 + data[i][j] * 20, 50 + data[i][j] * 20, 80, 255);
                        }
                        if (data[i][dim - j - 1] > maxrl) {
                            maxrl = data[i][dim - j - 1];
                            colors[i][dim - j - 1] = new Color(50 + data[i][dim - j - 1] * 20, 50 + data[i][dim - j - 1] * 20, 80, 255);
                        }
                        if (data[j][i] > maxtb) {
                            maxtb = data[j][i];
                            colors[j][i] = new Color(50 + data[j][i] * 20, 50 + data[j][i] * 20, 80, 255);
                        }
                        if (data[dim - j - 1][i] > maxbt) {
                            maxbt = data[dim - j - 1][i];
                            colors[dim - j - 1][i] = new Color(50 + data[dim - j - 1][i] * 20, 50 + data[dim - j - 1][i] * 20, 80, 255);
                        }
                    }
                }
                EndMode3D();
                return cnt > 900;
            });
            return solver.part2();
        }
    }
}
