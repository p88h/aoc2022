using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace aoc2022 {
    public class Vis22 : Day22 {
        private ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 24, "Day22");

        public string part1off() {
            int winx = 0, winy = 0, winw = Math.Min(dimx, 1920 / 12), winh = Math.Min(dimy, 1080 / 24);
            int x = xmin[0], y = 0, dx = 1, dy = 0, idx = 0, dist = moves[0], face = 0;
            string faces = ">v<^";
            renderer.loop(cnt => {
                if (y < winy + 4 && winy > 0) winy--;
                if (x < winx + 4 && winx > 0) winx--;
                if (y > winy + winh - 4 && winy + winh < dimy) winy++;
                if (x > winx + winw - 4 && winx + winw < dimx) winx++;
                mapp[y, x] = faces[face];
                for (int sy = 0; sy <= winh; sy++) for (int sx = 0; sx <= winw; sx++) if (mapp[sy + winy, sx + winx] != ' ')
                            renderer.WriteXY(sx, sy, mapp[sy + winy, sx + winx].ToString());
                if (dist > 0) {
                    var (nx, ny) = move1(x, y, dx, dy);
                    dist--;
                    if (mapp[ny, nx] == '#') dist = 0; else (x, y) = (nx, ny);
                } else if (idx < moves.Length - 1) {
                    if (idx < turns.Length) (dx, dy) = (turns[idx] == 'R') ? (-dy, dx) : (dy, -dx);
                    face = dx != 0 ? -(dx - 1) : (2 - dy);
                    dist = moves[++idx];
                } else return true;
                return cnt > 3600;
            });
            return "";
        }

        float turn(float s, float d) {
            float delta = Math.Abs(s - d);
            float sign = Math.Abs(s) > Math.Abs(d) || delta >= 180 ? -1 : 1;
            float diff = (180 - Math.Abs(delta - 180)) * sign;
            if (diff > 3) diff = 3; if (diff < -3) diff = -3;
            return s + diff;
        }

        (float, float) side_angle(int side, float current_phi = 355) {
            switch (side) {
                case 0: return (85, 85);
                case 1: return (355, 85);
                case 2: return (current_phi, 175);
                case 3: return (175, 85);
                case 4: return (265, 85);
                case 5: return (current_phi, 5);
            }
            return (current_phi, current_phi);
        }

        Vector3 orbital(float phiDeg, float thetaDeg, float cx, float cy, float cz, float radius) {
            double phiRad = Math.PI * phiDeg / 180.0;
            double thetaRad = Math.PI * thetaDeg / 180.0;
            double x = radius * Math.Sin(thetaRad) * Math.Cos(phiRad) + cx;
            double y = radius * Math.Sin(thetaRad) * Math.Sin(phiRad) + cy;
            double z = radius * Math.Cos(thetaRad) + cz;
            return new Vector3((float)x, (float)z, (float)y);
        }

        public override string part2() {
            int x = xmin[0], y = 0, dx = 1, dy = 0, idx = 0, dist = moves[0], face = 0;
            string faces = "⮕⬇⬅⬆";

            Camera3D camera = new Camera3D();
            List<Model> sides = new List<Model>();

            camera.Target = new Vector3(0, 0, 0);
            camera.Position = orbital(85, 85, 0, 0, 0, 20);
            camera.Up = new Vector3(0, 1, 0);
            camera.FovY = 45.0f;
            camera.Projection = CameraProjection.Perspective;
            UpdateCamera(ref camera, CameraMode.Custom);
            List<RenderTexture2D> textures = new List<RenderTexture2D>();
            Model model = LoadModel("resources/cube.obj");
            for (int i = 0; i < 6; i++) {
                RenderTexture2D rtex = LoadRenderTexture(50 * 24, 50 * 24);
                textures.Add(rtex);
                SetMaterialTexture(ref model, i, MaterialMapIndex.Diffuse, ref rtex.Texture);
            }
            sides.Add(model);
            var shader = LoadShader("resources/lighting.vs", "resources/lighting.fs");
            unsafe {
                shader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(shader, "viewPos");
            }
            int ambientLoc = GetShaderLocation(shader, "ambient");
            float[] ambient = new[] { 0.4f, 0.4f, 0.4f, 1.0f };
            Raylib.SetShaderValue(shader, ambientLoc, ambient, ShaderUniformDataType.Vec4);
            Light[] lights = new Light[6];
            for (int i = 0; i < 6; i++) {
                var (phi, theta) = side_angle(i);
                var lightPos = orbital(phi, theta, 0, 0, 0, 30);
                var targetPos = orbital(phi, theta, 0, 0, 0, 5);
                lights[i] = Rlights.CreateLight(i, LightType.LIGHT_POINT, lightPos, targetPos, Color.RayWhite, shader);
            }
            unsafe {
                for (int i = 0; i < 6; i++) model.Materials[i].Shader = shader;
            }
            List<(int, int)> texpos = new List<(int, int)> { (50, 0), (100, 0), (50, 50), (0, 100), (50, 100), (0, 150) };
            int last_side = 0, current_side = 0;
            (float, float) target_angle = (85, 85), current_angle = target_angle; ;
            int sleep = 0, maxsleep = 10;
            renderer.SetColor(160, 160, 160, 255);
            renderer.loop(cnt => {
                mapp[y, x] = faces[face];
                for (int i = 0; i < 6; i++) {
                    if (y >= texpos[i].Item2 && y < texpos[i].Item2 + dimc &&
                        x >= texpos[i].Item1 && x < texpos[i].Item1 + dimc) {
                        current_side = i;
                        if (last_side != current_side) {
                            last_side = current_side;
                            target_angle = side_angle(current_side, target_angle.Item1);
                            Console.WriteLine(String.Format("Current side: {0} angle: {1},{2} => {3},{4}", current_side,
                                              current_angle.Item1, current_angle.Item2, target_angle.Item1, target_angle.Item2));
                        }
                    } else if (cnt > 0) continue;
                    BeginTextureMode(textures[i]);
                    ClearBackground(Color.Black);
                    DrawRectangleLines(0, 0, 50 * 24, 50 * 24, Color.White);
                    for (int ty = 0; ty < 50; ty++) for (int tx = 0; tx < 50; tx++) {
                            char ch = mapp[ty + texpos[i].Item2, tx + texpos[i].Item1];
                            if ((ty + texpos[i].Item2, tx + texpos[i].Item1) == (y, x)) {
                                renderer.SetColor(200, 250, 200, 255);
                                renderer.WriteXY(2 * tx, ty, ch.ToString());
                                renderer.SetColor(160, 160, 160, 255);
                            } else if (ch != '.') renderer.WriteXY(2 * tx, ty, ch.ToString());
                        }
                    EndTextureMode();
                }
                if (current_angle != target_angle) {
                    current_angle.Item1 = turn(current_angle.Item1, target_angle.Item1);
                    current_angle.Item2 = turn(current_angle.Item2, target_angle.Item2);
                    camera.Position = orbital(current_angle.Item1, current_angle.Item2, 0, 0, 0, 20);
                    UpdateCamera(ref camera, CameraMode.Custom);
                }
                BeginMode3D(camera);
                DrawModelEx(sides[0], new Vector3(0, 0, 0), new Vector3(0, 1, 0), 0, new Vector3(1, 1, 1), Color.White);
                EndMode3D();
                if (sleep <= maxsleep) {
                    sleep++;
                    return false;
                }
                sleep = 0;
                if (dist > 0) {
                    var (nx, ny, ndx, ndy) = move2(x, y, dx, dy);
                    dist--;
                    if (mapp[ny, nx] == '#') dist = 0; else (x, y, dx, dy) = (nx, ny, ndx, ndy);
                    face = dx != 0 ? -(dx - 1) : (2 - dy);
                } else if (idx < moves.Length - 1) {
                    if (idx < turns.Length) (dx, dy) = (turns[idx] == 'R') ? (-dy, dx) : (dy, -dx);
                    face = dx != 0 ? -(dx - 1) : (2 - dy);
                    dist = moves[++idx];
                    if (maxsleep > 0) maxsleep--;
                } else return true;
                return false;
            });
            return "";
        }

    }
}
