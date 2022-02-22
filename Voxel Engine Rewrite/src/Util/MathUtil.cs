using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.World;

namespace Voxel_Engine_Rewrite.src.Util
{
    internal static class MathUtil
    {
        public static int mod16Fast(int x)
        {
            return x & ((1 << 4) - 1);
        }
        public static Pos2 GetChunkCoordinates(vec3 pos)
        {
            return new Pos2( (int)MathF.Floor(pos.x / 16), (int)MathF.Floor(pos.z / 16) );
        }
    }
}
