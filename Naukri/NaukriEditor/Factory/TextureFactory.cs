using UnityEngine;

namespace NaukriEditor.Factory
{
    public static class TextureFactory
    {
        public static Texture2D SolidColor(Color32 color)
        {
            return SolidColor(1, 1, color);
        }

        public static Texture2D SolidColor(int width, int height, Color32 color)
        {
            var res = new Texture2D(width, height);
            var pixels = new Color32[width * height];

            for (var i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            res.SetPixels32(pixels);
            res.Apply();

            return res;
        }
    }
}
