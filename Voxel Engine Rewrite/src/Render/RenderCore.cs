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
using static OpenGL.GL;

namespace Voxel_Engine_Rewrite.src.Render
{
    internal static class RenderCore
    {
        static List<int> bufferMaxSizes, bufferedDataSize;
        static uint texMap;
        static List<uint> vbochunks, vaochunks;
        static List<bool> bufferAvailability;
        static int viewLoc;

        static List<Task> BufferChunks;

        static ProgramShaders program;
        static RenderCore()
        {
            program = new ProgramShaders();
            viewLoc = glGetUniformLocation(GetProgram(), "view");

            bufferMaxSizes = new List<int>();
            bufferedDataSize = new List<int>();
            vbochunks = new List<uint>();
            vaochunks = new List<uint>();
            bufferAvailability = new List<bool>();
            BufferChunks = new List<Task>();
            //AssignBuffers(chunks);

            CreateTextureMap(out texMap);

            glEnable(GL_DEPTH_TEST);
            glDepthFunc(GL_LESS);
            glClearColor(0.5f, 0.95f, 1.0f, 1.0f);

            glEnable(GL_CULL_FACE);
            glCullFace(GL_BACK);
        }
        static public void Flush()
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            RenderBuffers();
            Glfw.SwapBuffers(Window.GetWindow());

        }
        static public uint GetProgram()
        {
            return program.program;
        }
        static private unsafe void RenderBuffers()
        {
            glUniformMatrix4fv(viewLoc, 1, false, Game.GetPlayer()?.GetLookAtMatrix().ToArray());
            glBindTexture(GL_TEXTURE_2D_ARRAY, texMap);
            foreach (var buffer in vaochunks)
            {
                if (bufferAvailability[(int)buffer - 1]) continue;
                glBindVertexArray(buffer);
                glDrawArrays(GL_TRIANGLES, 0, bufferedDataSize[(int)buffer - 1] / sizeOfVertex);
            }
        }
        static private unsafe void CreateBuffer(out uint vao, out uint vbo)
        {
            vao = glGenVertexArray();
            vbo = glGenBuffer();
            vaochunks.Add(vao);
            vbochunks.Add(vbo);
            bufferAvailability.Add(false);
            bufferMaxSizes.Add(-1);
            bufferedDataSize.Add(0);

            glBindVertexArray(vao);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);
            glVertexAttribIPointer(0, 1, GL_UNSIGNED_BYTE, sizeOfVertex, NULL);
            glEnableVertexAttribArray(0);
            glVertexAttribIPointer(1, 1, GL_UNSIGNED_BYTE, sizeOfVertex, (void*)(sizeof(byte)));
            glEnableVertexAttribArray(1);
            glVertexAttribIPointer(2, 1, GL_UNSIGNED_BYTE, sizeOfVertex, (void*)(2 * sizeof(byte)));
            glEnableVertexAttribArray(2);
            glVertexAttribIPointer(3, 1, GL_UNSIGNED_BYTE, sizeOfVertex, (void*)(3 * sizeof(byte)));
            glEnableVertexAttribArray(3);
            glVertexAttribIPointer(4, 1, GL_UNSIGNED_BYTE, sizeOfVertex, (void*)(4 * sizeof(byte)));
            glEnableVertexAttribArray(4);
            glVertexAttribIPointer(5, 1, GL_UNSIGNED_BYTE, sizeOfVertex, (void*)(5 * sizeof(byte)));
            glEnableVertexAttribArray(5);
            glVertexAttribIPointer(6, 1, GL_INT, sizeOfVertex, (void*)(6 * sizeof(byte)));
            glEnableVertexAttribArray(6);
            glVertexAttribIPointer(7, 1, GL_INT, sizeOfVertex, (void*)(6 * sizeof(byte) + sizeof(int)));
            glEnableVertexAttribArray(7);
        }
        static private unsafe void CreateTextureMap(out uint id)
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
