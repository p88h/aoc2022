using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;


namespace aoc2022 {
    public class Vis18 : Solution {
        private Day18 solver = new Day18();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            return solver.part1();
        }

        public string part2() {
            var ret = solver.part2();
            ASCIIRay renderer = new ASCIIRay(1920, 1080, 30, 24, "Day18");
            Camera3D camera = new Camera3D();
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.target = new Vector3(solver.max / 2, solver.max / 2, solver.max / 2);            
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;
            renderer.loop(cnt => {
                camera.position = new Vector3((float)Math.Cos(cnt / 300.0f) * solver.max + solver.max / 2, 
                                              solver.max * 2,
                                              (float)Math.Sin(cnt / 300.0f) * solver.max + solver.max / 2);
                BeginMode3D(camera);
                for (int idx = cnt * 8; idx < solver.trace.Count; idx++) {
                    var (x, y, z) = solver.trace[idx];
                    DrawCube(new Vector3(x, z, y), 1f, 1f, 1f, LIGHTGRAY);
                    DrawCubeWires(new Vector3(x, z, y), 1f, 1f, 1f, DARKGRAY);
                }
                for (int idx = 0; idx < solver.cubes.Count; idx++) {
                    var (x, y, z) = (solver.cubes[idx][0],solver.cubes[idx][1],solver.cubes[idx][2]) ;
                    DrawCube(new Vector3(x, z, y), 1f, 1f, 1f, BROWN);
                    DrawCubeWires(new Vector3(x, z, y), 1f, 1f, 1f, DARKGRAY);
                }
                EndMode3D();
                return cnt > solver.trace.Count/8 + 60;
            });
            return ret;
        }
    }
}
