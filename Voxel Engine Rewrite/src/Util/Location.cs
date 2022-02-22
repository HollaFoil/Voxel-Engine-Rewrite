using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.World
{
    public struct Pos2
    {
        public int x, y;
        public Pos2(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public override int GetHashCode()
        {
            return x + y << 16;
        }
    }
    public struct Pos3
    {
        public int x, y, z;
        public Pos3(int X, int Y, int Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public override int GetHashCode()
        {
            return x + y << 8 + z << 16;
        }
    }
}
