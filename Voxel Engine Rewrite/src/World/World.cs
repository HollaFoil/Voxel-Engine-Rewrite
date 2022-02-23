using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Util;

namespace Voxel_Engine_Rewrite.src.World
{
    internal class World : ITickable
    {
        Task<Chunk> GenerateChunkTask = null;
        Dictionary<Pos2, Chunk> chunks = new Dictionary<Pos2, Chunk>();

        public byte[,,] ChunkArrayPool { get; private set; }

        protected override void OnInit()
        {
            
        }
        protected override async void Update()
        {
            var position = Game.GetPlayer().GetChunkLoc();
            if (GenerateChunkTask == null && !chunks.ContainsKey(position)) GenerateChunkTask = GenerateChunkAsync(position);
            if (!GenerateChunkTask?.IsCompleted == true || chunks.ContainsKey(position)) return;
            var c = GenerateChunkTask.Result;
            GenerateChunkTask = null;
            chunks.Add(position, c);
            Render.RenderCore.AssignChunk(c);
        }

        private Task<Chunk> GenerateChunkAsync(Pos2 position)
        {
            var task = Task.Run(() => GenerateChunk(position));
            return task;
        }
        public Chunk? GetChunk(int x, int y)
        {
            if (!chunks.TryGetValue(new Pos2(x,y), out Chunk? c)) return null;
            return c;
        }
        public Chunk? GetChunk(Pos2 pos)
        {
            if (!chunks.TryGetValue(pos, out Chunk? c)) return null;
            return c;
        }
        public byte? GetBlockType(int x, int y, int z)
        {
            if (y > 255 || y < 0) return null;
            Chunk? chunk = GetChunk(x, z);
            return chunk?.GetBlock(MathUtil.mod16Fast(x), y, MathUtil.mod16Fast(z));
        }
        public byte? GetBlockType(Pos3 pos)
        {
            if (pos.y > 255 || pos.y < 0) return 0;
            Chunk? chunk = GetChunk(pos.x, pos.z);
            return chunk.GetBlock(MathUtil.mod16Fast(pos.x), pos.y, MathUtil.mod16Fast(pos.z));
        }
        private Chunk GenerateChunk(Pos2 position)
        {
            int arrayId = ChunkDataArrayPool.Rent3DArray();
            byte[,,] blocks = ChunkDataArrayPool.GetArray(arrayId);
            blocks[0, 0, 0] = 2;
            /*for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        blocks[x,y,z] = 2;
                    }
                }
            }*/
            return new Chunk(position, arrayId);
        }
    }
}
