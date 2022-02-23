using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.Render
{
    internal class Constants
    {
        public const int FaceSize = 6 * (VertexSize);
        public const int VertexSize = 2 * sizeof(int) + 6 * sizeof(byte);
        public const byte BACK = 1, LEFT = 2, FRONT = 4, RIGHT = 8, BOTTOM = 16, TOP = 32;
    }
}
