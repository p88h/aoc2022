using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.MaterialMapIndex;


namespace aoc2022 {
    public class Vis12 : Solution {
        private Day12 solver = new Day12();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 24, "Day12");
            Camera3D camera = new Camera3D();
            string ret = solver.part1();
            int cx = solver.ex, cy = solver.ey;
            List<(int, int)> path = new List<(int, int)> { (cx, cy) };
            while ((cx, cy) != (solver.sx, solver.ey)) {
                int t = solver.prev[cx, cy];
                cx = t % 1000;
                cy = t / 1000;
                path.Add((cx, cy));
            }
            path.Reverse();
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;
            int factor = 10;
            int maxcnt = path.Count * factor + 400;
            float rot = 0.0f;
            renderer.loop(cnt => {
                int idx = cnt / factor, nidx = idx + 1;
                int ofs = cnt % factor;
                if (idx >= path.Count - 1) idx = nidx = path.Count - 1;
                (int px1, int py1) = path[idx];
                (int px2, int py2) = path[nidx];
                int pz1 = (solver.map[px1, py1] - 'a');
                int pz2 = (solver.map[px2, py2] - 'a');
                float px = (px1 * (factor - ofs) + px2 * ofs) / (float)factor;
                float py = (py1 * (factor - ofs) + py2 * ofs) / (float)factor;
                float pz = 0.5f * (pz1 * (factor - ofs) + pz2 * ofs) / (float)factor;
                camera.target = new Vector3(px, pz, py);
                if (idx < path.Count - 1) {
                    camera.position = new Vector3(px - 2, 62.0f, py + solver.h);
                    pz += (float)Math.Sin(Math.PI * ofs / factor);
                } else {
                    camera.position = new Vector3(px - 2 + rot, 62.0f - rot, py + solver.h - rot);
                    if (rot < 30.0f) {
                        rot += 1.0f / (float) factor;
                    } else if (rot < 33.0f) {
                        rot += 0.5f / (float) factor;
                    }
                }
                BeginMode3D(camera);
                DrawSphereEx(new Vector3(px, pz + 0.5f, py), 0.5f, 6, 6, BLUE);                
                for (int x = 0; x < solver.w; x++) {
                    for (int y = 0; y < solver.h; y++) {
                        int elev = solver.map[x, y] - 'a'; // 0-25
                        DrawCube(new Vector3(x, elev / 4.0f, y), 1f, elev / 2.0f, 1f, new Color(elev * 10, 250 - elev * 10, 140, 255));
                        DrawCubeWires(new Vector3(x, elev / 4.0f, y), 1f, elev / 2.0f, 1f, BLACK);
                    }
                }
                EndMode3D();
                renderer.WriteXY(1,0,"Distance: " + idx);
                renderer.WriteXY(1,1,"Position: " + px1 + "," + py1);
                renderer.WriteXY(1,2,"Elevation: " + pz1);
                return cnt > maxcnt;
            });
            return solver.part1();
        }

        public string part2() {
            return solver.part2();
        }
    }
}
