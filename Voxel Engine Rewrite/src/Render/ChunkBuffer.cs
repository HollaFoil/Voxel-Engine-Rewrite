using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Render.Mesh;
using static OpenGL.GL;
using GLFW;
using Voxel_Engine_Rewrite.src.World;

namespace Voxel_Engine_Rewrite.src.Render
{
    internal class ChunkBuffer
    {
        public bool available = true;
        private ChunkMesh mesh = null;
        private int bufferSize = 0;
        private uint vao, vbo;
        
        public ChunkBuffer()
        {
            GenerateBuffer();
        }
        public void Free()
        {
            mesh?.Free();
            mesh = null;
            available = true;
        }
        public void Assign(Chunk chunk)
        {
            mesh = new ChunkMesh(chunk);
            available = false;
        }
        public Task BufferAsync()
        {
            return Task.Run(() => Buffer());
        }
        private unsafe void Buffer()
        {
            
            if (mesh == null || available) return;
            
            var meshSize = mesh.GetSize();
            var data = mesh.GetMesh();
            Console.WriteLine("Buffering" + meshSize);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);
            if (meshSize < bufferSize)
            {
                fixed (void* ptr = &data[0]) glBufferSubData(GL_ARRAY_BUFFER, 0, meshSize, ptr);
            }
            else
            {
                fixed (void* ptr = &data[0]) glBufferData(GL_ARRAY_BUFFER, meshSize, ptr, GL_DYNAMIC_DRAW);
                bufferSize = meshSize;
            }
            mesh.Free();
            mesh = null;
        }
        public void Render()
        {
            if (available) return;
            glBindVertexArray(vao);
            glDrawArrays(GL_TRIANGLES, 0, mesh.GetVertexCount());
        }
        private unsafe void GenerateBuffer()
        {
            vao = glGenVertexArray();
            vbo = glGenBuffer();

            glBindVertexArray(vao);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);
            //Block local to chunk x
            glVertexAttribIPointer(0, 1, GL_UNSIGNED_BYTE, Constants.VertexSize, NULL);
            glEnableVertexAttribArray(0);
            //Block local to chunk y
            glVertexAttribIPointer(1, 1, GL_UNSIGNED_BYTE, Constants.VertexSize, (void*)(sizeof(byte)));
            glEnableVertexAttribArray(1);
            //Block local to chunk z
            glVertexAttribIPointer(2, 1, GL_UNSIGNED_BYTE, Constants.VertexSize, (void*)(2 * sizeof(byte)));
            glEnableVertexAttribArray(2);

            //Vertex ID
            glVertexAttribIPointer(3, 1, GL_UNSIGNED_BYTE, Constants.VertexSize, (void*)(3 * sizeof(byte)));
            glEnableVertexAttribArray(3);
            //Texture ID
            glVertexAttribIPointer(4, 1, GL_UNSIGNED_BYTE, Constants.VertexSize, (void*)(4 * sizeof(byte)));
            glEnableVertexAttribArray(4);
            //Ambient occlusion value
            glVertexAttribIPointer(5, 1, GL_UNSIGNED_BYTE, Constants.VertexSize, (void*)(5 * sizeof(byte)));
            glEnableVertexAttribArray(5);

            //Chunk coordinate x
            glVertexAttribIPointer(6, 1, GL_INT, Constants.VertexSize, (void*)(6 * sizeof(byte)));
            glEnableVertexAttribArray(6);
            //Chunk coordinate y
            glVertexAttribIPointer(7, 1, GL_INT, Constants.VertexSize, (void*)(6 * sizeof(byte) + sizeof(int)));
            glEnableVertexAttribArray(7);
        }
        public override int GetHashCode()
        {
            return (int)vao;
        }
    }
}
