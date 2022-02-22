using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Util;

namespace Voxel_Engine_Rewrite.src.World
{
    internal class Chunk
    {
        public Pos2 location;
        private byte[,,] blocks;
        private int arrayPoolId;

        public void Free()
        {
            blocks = null;
            ChunkDataArrayPool.Return3DArray(arrayPoolId);
            arrayPoolId = -1;
        }
        public Chunk(Pos2 pos, int dataArrayPool)
        {
            location = pos;
            arrayPoolId = dataArrayPool;
            blocks = ChunkDataArrayPool.GetArray(arrayPoolId);
        }
        public Chunk(Pos2 pos)
        {
            location = pos;
            arrayPoolId = ChunkDataArrayPool.Rent3DArray();
            blocks = ChunkDataArrayPool.GetArray(arrayPoolId);
        }
        public byte GetBlock(int x, int y, int z)
        {
            return blocks[x, y, z];
        }
        public void SetBlock(int x, int y, int z, byte type)
        {
            blocks[x, y, z] = type;
        }
    }
}
