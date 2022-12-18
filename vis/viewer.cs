using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.CameraMode;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.ShaderLocationIndex;

namespace aoc2022 {

    static class ViewerOptions {
        public static bool recordVideo = false;
    }

    public class Viewer {
        int width, height;
        FFWriter ff_writer;
        string title;
        Stopwatch stopwatch = new Stopwatch();
        public Shader shader;
        public Light[] lights = new Light[4];

        public Viewer(int w, int h, int fps, string t) {
            Console.WriteLine("Initializing viewer: " + w + "x" + h + " @" + fps + "fps");
            width = w; height = h; title = t;
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(width, height, "AOC2022 " + title);
            SetTargetFPS(fps);
            ff_writer = new FFWriter(width, height, fps, title);
        }

        public void setupLights(float xmax, float ymax, float zmax, List<Model> models) {
            shader = LoadShader("resources/lighting.vs", "resources/lighting.fs");
            unsafe {
                shader.locs[(int)SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
            }
            int ambientLoc = GetShaderLocation(shader, "ambient");
            float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
            Raylib.SetShaderValue(shader, ambientLoc, ambient, ShaderUniformDataType.SHADER_UNIFORM_VEC4);
            lights[0] = Rlights.CreateLight(0, LightType.LIGHT_POINT, new Vector3(-10, zmax + 10, ymax + 10), Vector3.Zero, WHITE, shader);
            lights[1] = Rlights.CreateLight(3, LightType.LIGHT_POINT, new Vector3(0, 100, 0), Vector3.Zero, WHITE, shader);
            // lights[2] = Rlights.CreateLight(1, LightType.LIGHT_POINT, new Vector3(xmax + 10, zmax + 10, ymax + 10), Vector3.Zero, RAYWHITE, shader);
            // lights[3] = Rlights.CreateLight(2, LightType.LIGHT_POINT, new Vector3(xmax / 2, zmax + 10, -10), Vector3.Zero, YELLOW, shader);
            unsafe {
                foreach (var model in models) model.materials[0].shader = shader;
            }
        }

        public async void loop(Func<int, bool> renderFrame) {
            int cnt = 0;
            bool done = false;
            var ff_task = Task.Run(() => { if (ViewerOptions.recordVideo) return ff_writer.run(); else return true; });
            stopwatch.Start();
            long lastts = 0;
            long lastcnt = 0;
            while (!WindowShouldClose() && !done) {
                BeginDrawing();
                ClearBackground(BLACK);
                done = renderFrame(cnt++);
                EndDrawing();
                if (ViewerOptions.recordVideo) {
                    Image screen = LoadImageFromScreen();
                    unsafe {
                        ff_writer.addRawImage(screen.data);
                        MemFree(screen.data);
                    }
                }
                long ts = stopwatch.ElapsedMilliseconds;
                if (ts > lastts + 4999) {
                    Console.WriteLine("Rendered " + (cnt - lastcnt) + " frames in " + (ts - lastts) + " ms. " + " AFPS=" + ((cnt * 1000 + 500) / ts));
                    lastcnt = cnt;
                    lastts = ts;
                }
            }
            UnloadShader(shader);
            CloseWindow();
            if (ViewerOptions.recordVideo) ff_writer.finish();
            await ff_task;
        }
    }
}
