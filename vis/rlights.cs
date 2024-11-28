using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace aoc2022 {
    public struct Light {
        public bool enabled;
        public LightType type;
        public Vector3 position;
        public Vector3 target;
        public Color color;

        public int enabledLoc;
        public int typeLoc;
        public int posLoc;
        public int targetLoc;
        public int colorLoc;
    }

    public enum LightType {
        LIGHT_DIRECTIONAL,
        LIGHT_POINT
    }

    public static class Rlights {
        public static Light CreateLight(int lightsCount, LightType type, Vector3 pos, Vector3 targ, Color color, Shader shader) {
            Light light = new Light();

            light.enabled = true;
            light.type = type;
            light.position = pos;
            light.target = targ;
            light.color = color;

            string enabledName = "lights[" + lightsCount + "].enabled";
            string typeName = "lights[" + lightsCount + "].type";
            string posName = "lights[" + lightsCount + "].position";
            string targetName = "lights[" + lightsCount + "].target";
            string colorName = "lights[" + lightsCount + "].color";

            light.enabledLoc = GetShaderLocation(shader, enabledName);
            light.typeLoc = GetShaderLocation(shader, typeName);
            light.posLoc = GetShaderLocation(shader, posName);
            light.targetLoc = GetShaderLocation(shader, targetName);
            light.colorLoc = GetShaderLocation(shader, colorName);

            UpdateLightValues(shader, light);

            return light;
        }

        public static void UpdateLightValues(Shader shader, Light light) {
            // Send to shader light enabled state and type
            Raylib.SetShaderValue(shader, light.enabledLoc, light.enabled ? 1 : 0, ShaderUniformDataType.Int);
            Raylib.SetShaderValue(shader, light.typeLoc, (int)light.type, ShaderUniformDataType.Int);

            // Send to shader light target position values
            Raylib.SetShaderValue(shader, light.posLoc, light.position, ShaderUniformDataType.Vec3);

            // Send to shader light target position values
            Raylib.SetShaderValue(shader, light.targetLoc, light.target, ShaderUniformDataType.Vec3);

            // Send to shader light color values
            float[] color = new[] { (float)light.color.R / (float)255, (float)light.color.G / (float)255, (float)light.color.B / (float)255, (float)light.color.A / (float)255 };
            Raylib.SetShaderValue(shader, light.colorLoc, color, ShaderUniformDataType.Vec4);
        }
    }
}
