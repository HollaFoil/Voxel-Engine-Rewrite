using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.Event.Events
{
    internal class MouseMovedEvent : Event<MouseMovedEvent>
    {
        private vec2 change;
        public MouseMovedEvent(vec2 change)
        {
            this.change = change;
        }
        public vec2 GetChange()
        {
            return change;
        }
        protected override MouseMovedEvent GetEvent()
        {
            return this;
        }
    }
}
