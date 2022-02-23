using GLFW;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Render.Shaders;
using Voxel_Engine_Rewrite.src.Render.Textures;
using Voxel_Engine_Rewrite.src.World;
using static OpenGL.GL;

namespace Voxel_Engine_Rewrite.src.Render
{
    internal static class RenderCore
    {
        static List<ChunkBuffer?> buffers;
        static Dictionary<Chunk, ChunkBuffer?> ChunkToBuffer;
        static uint texArray;
        static int viewLoc;

        static List<Task> BufferTasks;

        static ProgramShaders program;
        static RenderCore()
        {
            program = new ProgramShaders();
            viewLoc = glGetUniformLocation(GetProgram(), "view");

            buffers = new List<ChunkBuffer?>();
            ChunkToBuffer = new Dictionary<Chunk, ChunkBuffer?>();
            BufferTasks = new List<Task>();
            //AssignBuffers(chunks);

            CreateTextureArray(out texArray);

            glEnable(GL_DEPTH_TEST);
            glDepthFunc(GL_LESS);
            glClearColor(0.5f, 0.95f, 1.0f, 1.0f);

            glEnable(GL_CULL_FACE);
            glCullFace(GL_BACK);
        }
        static public void Flush()
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            UpdateMeshes();
            RenderBuffers();
            Glfw.SwapBuffers(Window.GetWindow());

        }
        static public uint GetProgram()
        {
            return program.program;
        }
        static public async void UpdateMeshes()
        {
            foreach (var buffer in buffers)
            {
                BufferTasks.Add(buffer.BufferAsync());
            }
            await Task.WhenAll(BufferTasks);
            BufferTasks.Clear();
        }
        static private unsafe void RenderBuffers()
        {
            glUniformMatrix4fv(viewLoc, 1, false, Game.GetPlayer()?.GetLookAtMatrix().ToArray());
            glBindTexture(GL_TEXTURE_2D_ARRAY, texArray);
            foreach (var buffer in buffers)
            {
                buffer.Render();
            }
        }
        static public void AssignChunk(Chunk c)
        {
            if (!ChunkToBuffer.TryGetValue(c, out ChunkBuffer? buffer))
            {
                buffer = FindAvailableBuffer();
                if (buffer == null)
                {
                    buffer = new ChunkBuffer();
                    buffers.Add(buffer);
                }
                ChunkToBuffer.Add(c, buffer);
            }
            buffer.Assign(c);

        }
        static public void FreeChunk(Chunk c)
        {
            if (!ChunkToBuffer.TryGetValue(c, out ChunkBuffer? buffer)) return;
            buffer.Free();
            ChunkToBuffer.Remove(c);
        }
        static private ChunkBuffer? FindAvailableBuffer()
        {
            foreach (var b in buffers)
            {
                if (b.available) return b;
            }
            return null;
        }
        static private unsafe void CreateTextureArray(out uint id)
        {
            
            id = glGenTexture();
            //byte[] tex = TextureUtils.LoadTexture(0, out int width, out int height);
            int layerCount = TextureUtils.GetCount()-1;
            byte[] temp = new byte[16 * 16 * layerCount];
            for (int i = 0; i < 16 * 16 * layerCount; i++) temp[i] = 255;
            glBindTexture(GL_TEXTURE_2D_ARRAY, id);
            glTexImage3D(GL_TEXTURE_2D_ARRAY, 0, GL_RGBA8, 16, 16, layerCount, 0, GL_RGBA, GL_UNSIGNED_BYTE, null);
            
            for (int i = 1; i <= layerCount; i++)
            {
                byte[] tex = TextureUtils.LoadTexture(i, out int width, out int height);
                Console.WriteLine(tex[0] + " " + tex[1] + " " + tex[2] + " " + tex[3]);
               
                fixed (byte* ptr = &tex[0])
                {
                    glTexSubImage3D(GL_TEXTURE_2D_ARRAY, 0, 0, 0, i-1, width, height, 1, GL_RGBA, GL_UNSIGNED_BYTE, ptr);
                }
            }
            glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);
            glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
            glGenerateMipmap(GL_TEXTURE_2D_ARRAY);
        }
        public static void RefreshProjectionMatrix()
        {
            mat4 projection;
            projection = mat4.Perspective(glm.Radians(90.0f), 1280 / (float)720, 0.1f, 600.0f);
            float[] p = projection.ToArray();
            int projectionLoc = glGetUniformLocation(GetProgram(), "projection");
            glUniformMatrix4fv(projectionLoc, 1, false, p);
        }
        public const int sizeOfVertex = 2 * sizeof(int) + 6 * sizeof(byte);
    }
}
