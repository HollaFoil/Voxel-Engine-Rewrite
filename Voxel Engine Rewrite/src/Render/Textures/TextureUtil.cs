using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Voxel_Engine_Rewrite.src.Render.Textures
{
    internal static class TextureUtils
    {
        public static byte[] LoadTexture(string filePath, out int width, out int height)
        {
            Stream imgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
    "Voxel_Engine_Rewrite.src.Assets.Textures." + filePath);
            return GetPixelsFromStream(imgStream, out width, out height);
        }
        public static byte[] LoadTexture(int id, out int width, out int height)
        {
            string filePath = "Assets/Textures/" + NameFromID[id] + ".png";
            Stream imgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
    "Voxel_Engine_Rewrite.src.Assets.Textures." + NameFromID[id] + ".png");
            return GetPixelsFromStream(imgStream, out width, out height);
        }
        private static byte[] GetPixelsFromStream(Stream stream, out int width, out int height)
        {
            var image = new Bitmap(stream);
            width = image.Width;
            height = image.Height;
            
            byte[] pixels = new byte[width * height * 4];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixels[i * width * 4 + j * 4] = image.GetPixel(i,j).R;
                    pixels[i * width * 4 + j * 4 + 1] = image.GetPixel(i, j).G;
                    pixels[i * width * 4 + j * 4 + 2] = image.GetPixel(i, j).B;
                    pixels[i * width * 4 + j * 4 + 3] = image.GetPixel(i, j).A;
                }
            }
            return pixels;
        }
        public static int GetCount()
        {
            return NameFromID.Count;
        }
        public static Texture[] CreateTextures()
        {
            Texture[] textures = new Texture[NameFromID.Count];
            for (int i = 0; i < NameFromID.Count; i++)
            {
                textures[i] = new Texture(i, NameFromID[i], LoadTexture(i, out int width, out int height));
            }
            return textures;
        }


        public static Dictionary<int, string> NameFromID = new Dictionary<int, string>
        {
            {0, "map" },
            {1, "cobblestone"},
            {2, "dirt"},
            {3, "grass_block_side"},
            {4, "grass_block_top"},
            {5, "oak_planks"},
            {6, "sand"},
            {7, "stone" },
            {8, "oak_log" },
            {9, "oak_log_top" },
            {10, "oak_leaves" }
        };
    }

    public enum ID : int
    {
        Map = 0,
        Cobblestone = 1,
        Dirt = 2,
        GrassSide = 3,
        GrassTop = 4,
        OakPlanks = 5,
        Sand = 6,
        Stone = 7,
        OakLogSide = 8,
        OakLogTop = 9,
        OakLeaves = 10,
    }
}
