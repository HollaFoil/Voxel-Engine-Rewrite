using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Util;
using Voxel_Engine_Rewrite.src.World;

namespace Voxel_Engine_Rewrite.src.Entity
{
    public class IEntity : ITickable
    {
        protected vec3 position;
        protected vec2 facing;
        public vec3 GetPosition()
        {
            return position;
        }
        public Pos2 GetChunkLoc()
        {
            return MathUtil.GetChunkCoordinates(position);
        }
        public vec2 GetYawPitch()
        {
            return facing;
        }
    }
}
