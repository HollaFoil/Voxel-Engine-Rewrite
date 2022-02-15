using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.Event.Events
{
    internal class DirectionalKeyPressEvent : Event<DirectionalKeyPressEvent>
    {
        private vec3 currentDirection;
        public DirectionalKeyPressEvent(vec3 current)
        {
            currentDirection = current;
        }
        public vec3 GetDirection()
        {
            return currentDirection;
        }
        protected override DirectionalKeyPressEvent GetEvent()
        {
            return this;
        }
    }
}
