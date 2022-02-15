using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Entity;

namespace Voxel_Engine_Rewrite.src.Event.Events
{
    public class PlayerMoveEvent : Event<PlayerMoveEvent>
    {
        private vec3 before;
        private vec3 after;
        public PlayerMoveEvent(vec3 current, vec3 previous)
        {
            before = previous;
            after = current;
        }
        public vec3 GetLocationAfter()
        {
            return after;
        }
        public vec3 GetLocationBefore()
        {
            return before;
        }
        protected override PlayerMoveEvent GetEvent()
        {
            return this;
        }
    }
}
