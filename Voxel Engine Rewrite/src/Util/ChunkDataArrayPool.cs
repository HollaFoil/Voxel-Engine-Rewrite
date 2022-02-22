using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.Util
{
    public static class ChunkDataArrayPool
    {
        private static List<byte[,,]> arrayPool = new List<byte[,,]>();
        private static List<bool> poolAvailability = new List<bool>();
        static ChunkDataArrayPool()
        {
        }



        public static int Rent3DArray()
        {
            int i = 0;
            int poolId = -1;
            foreach (var isAvailable in poolAvailability)
            {
                if (!isAvailable)
                {
                    i++;
                    continue;
                }
                poolId = i;
                poolAvailability[i] = false;
                break;
            }
            if (poolId == -1)
            {
                var arr = new byte[16, 256, 16];
                arrayPool.Add(arr);
                poolId = arrayPool.Count - 1;
                poolAvailability.Add(false);
            }
            return poolId;
        }
        public static void Return3DArray(int poolId)
        {
            var array = arrayPool[poolId];
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        array[x, y, z] = 0;
                    }
                }
            }
            poolAvailability[poolId] = true;
        }
        public static byte[,,] GetArray(int poolId)
        {
            return arrayPool[poolId];
        }
    }
}
