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
            if (GenerateChunkTask == null || chunks.ContainsKey(position)) GenerateChunkTask = GenerateChunkAsync(position);
            if (!GenerateChunkTask.IsCompleted) return;
            var c = GenerateChunkTask.Result;
            GenerateChunkTask = null;
            chunks.Add(position, c);
        }

        private Task<Chunk> GenerateChunkAsync(Pos2 position)
        {
            var task = Task.Run(() => GenerateChunk(position));
            return task;
        }
        private Chunk GenerateChunk(Pos2 position)
        {
            int arrayId = ChunkDataArrayPool.Rent3DArray();
            byte[,,] blocks = ChunkDataArrayPool.GetArray(arrayId);
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        blocks[x,y,z] = 2;
                    }
                }
            }
            return new Chunk(position, arrayId);
        }
    }
}
