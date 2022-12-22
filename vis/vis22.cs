using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.ShaderLocationIndex;

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

        float shift(float s, float d) {
            float delta = Math.Abs(s - d);
            float sign = Math.Abs(s) > Math.Abs(d) || delta >= 180 ? -1 : 1;
            float diff = (180 - Math.Abs(delta - 180)) * sign;
            if (s != d - diff) s = d - diff;
            if (d < s) {
                if (s - 5 <= d) return d;
                return s - 3f;
            } else {
                if (s + 5 >= d) return d;
                return s + 3f;
            }
        }

        (float, float) side_angle(int side, float current_phi = 355) {
            switch (side) {
                case 0: return (current_phi, 5);
                case 1: return (355, 85);
                case 2: return (85, 85);
                case 3: return (175, 85);
                case 4: return (current_phi, 175);
                case 5: return (265, 85);
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
            string faces = "⬅⬆⮕⬇";

            Camera3D camera = new Camera3D();
            List<Model> sides = new List<Model>();

            camera.target = new Vector3(5, 5, 5);
            camera.position = new Vector3(5, 22, 5.001f);
            camera.up = new Vector3(0, 1, 0);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;
            SetCameraMode(camera, CameraMode.CAMERA_CUSTOM);
            List<RenderTexture2D> textures = new List<RenderTexture2D>();
            for (int i = 0; i < 6; i++) {
                RenderTexture2D rtex = LoadRenderTexture(50 * 24, 50 * 24);
                textures.Add(rtex);
                Model model = LoadModelFromMesh(GenMeshPlane(10, 10, 5, 5));
                SetMaterialTexture(ref model, 0, MATERIAL_MAP_DIFFUSE, ref rtex.texture);
                sides.Add(model);
            }
            var shader = LoadShader("resources/lighting.vs", "resources/lighting.fs");
            unsafe {
                shader.locs[(int)SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
            }
            int ambientLoc = GetShaderLocation(shader, "ambient");
            float[] ambient = new[] { 0.4f, 0.4f, 0.4f, 1.0f };
            Raylib.SetShaderValue(shader, ambientLoc, ambient, ShaderUniformDataType.SHADER_UNIFORM_VEC4);
            Light[] lights = new Light[6];
            for (int i = 0; i < 6; i++) {
                var (phi, theta) = side_angle(i);
                var lightPos = orbital(phi, theta, 5, 5, 5, 30);
                var targetPos = orbital(phi, theta, 5, 5, 5, 10);
                lights[i] = Rlights.CreateLight(i, LightType.LIGHT_POINT, lightPos, targetPos, RAYWHITE, shader);
            }
            unsafe {
                foreach (var model in sides) model.materials[0].shader = shader;
            }
            List<(int, int)> texpos = new List<(int, int)> { (50, 0), (100, 0), (50, 50), (0, 100), (50, 100), (0, 150) };
            List<(bool, bool)> flip = new List<(bool, bool)> { (true, false), (true, false), (true, false), (false, true), (true, false), (true, true) };
            int last_side = 0, current_side = 0;
            (float, float) target_angle = (85, 5), current_angle = target_angle; ;
            int sleep = 0, maxsleep = 10;
            renderer.SetColor(160,160,160,255);
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
                    ClearBackground(BLACK);
                    DrawRectangleLines(0, 0, 50 * 24, 50 * 24, WHITE);
                    for (int ty = 0; ty < 50; ty++) for (int tx = 0; tx < 50; tx++) {
                            char ch = mapp[ty + texpos[i].Item2, tx + texpos[i].Item1];
                            int dx = flip[i].Item1 ? tx : 49 - tx;
                            int dy = flip[i].Item2 ? ty : 49 - ty;
                            if (flip[i].Item1 && ch == '⮕') ch = '⬅';
                            else if (flip[i].Item1 && ch == '⬅') ch = '⮕';
                            if (flip[i].Item2 && ch == '⬇') ch = '⬆';
                            else if (flip[i].Item2 && ch == '⬆') ch = '⬇';
                            if (i == 5) {
                                (dx, dy) = (dy, dx);
                                switch (ch) {
                                    case '⮕': ch = '⬇'; break;
                                    case '⬅': ch = '⬆'; break;
                                    case '⬆': ch = '⬅'; break;
                                    case '⬇': ch = '⮕'; break;
                                }
                            }
                            if ((ty + texpos[i].Item2, tx + texpos[i].Item1) == (y,x)) {
                                renderer.SetColor(200,250,200,255);
                                renderer.WriteXY(2 * dx, dy, ch.ToString());
                                renderer.SetColor(160,160,160,255);
                            } else if (ch != '.') renderer.WriteXY(2 * dx, dy, ch.ToString());
                        }
                    EndTextureMode();
                }
                if (current_angle != target_angle) {
                    current_angle.Item1 = shift(current_angle.Item1, target_angle.Item1);
                    current_angle.Item2 = shift(current_angle.Item2, target_angle.Item2);
                    camera.position = orbital(current_angle.Item1, current_angle.Item2, 5, 5, 5, 20);
                    UpdateCamera(ref camera);
                }
                BeginMode3D(camera);
                DrawModelEx(sides[0], new Vector3(5, 10, 5), new Vector3(0, 1, 0), 0, new Vector3(1, 1, 1), WHITE);
                DrawModelEx(sides[1], new Vector3(10, 5, 5), new Vector3(0, 0, 1), 270, new Vector3(1, 1, 1), WHITE);
                DrawModelEx(sides[2], new Vector3(5, 5, 10), new Vector3(1, 0, 0), 90, new Vector3(1, 1, 1), WHITE);
                DrawModelEx(sides[3], new Vector3(0, 5, 5), new Vector3(0, 0, 1), 90, new Vector3(1, 1, 1), WHITE);
                DrawModelEx(sides[4], new Vector3(5, 0, 5), new Vector3(1, 0, 0), 180, new Vector3(1, 1, 1), WHITE);
                DrawModelEx(sides[5], new Vector3(5, 5, 0), new Vector3(1, 0, 0), 270, new Vector3(1, 1, 1), WHITE);
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
                return cnt > 3600;
            });
            return "";
        }

    }
}
