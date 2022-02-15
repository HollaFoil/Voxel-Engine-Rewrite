using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Event.Events;

namespace Voxel_Engine_Rewrite.src.Entity
{
    public static class Player
    {
        private static vec3 position;
        private static vec2 facing;
        private static vec3 directionPressed;
        private static float speed = 0.02f;
        static Player()
        {
            DirectionalKeyPressEvent.Listeners += OnDirectionalKeyPress;
            MouseMovedEvent.Listeners += OnMouseMoved;
            position = new vec3();
            facing = new vec2();
            directionPressed = new vec3();
        }
        public static vec3 GetPosition()
        {
            return position;
        }
        public static vec2 GetYawPitch()
        {
            return facing;
        }
        private static void OnDirectionalKeyPress(DirectionalKeyPressEvent e)
        {
            directionPressed = e.GetDirection();
            Console.WriteLine("Direction press: " + directionPressed);
        }
        private static void OnMouseMoved(MouseMovedEvent e)
        {
            facing += e.GetChange();
            if (facing.x <= -180.0f) facing.x += 360.0f;
            if (facing.x > 180.0f) facing.x -= 360.0f;
            if (facing.y < -90.0f) facing.y = -90.0f;
            if (facing.y > 90.0f) facing.y = 90.0f;
            Console.WriteLine("Facing: " + facing);
        }
        public static void Update(int elapsedTime)
        {
            position += GetMovementDirection() * elapsedTime * speed;
            Console.WriteLine("Position: " + position);
        }
        private static vec3 GetMovementDirection()
        {
            vec3 force = new vec3(0,0,0);
            force.y += directionPressed.y;
            force.z += (glm.Sin(glm.Radians(facing.x + 180)) * directionPressed.z +
                           glm.Cos(glm.Radians(facing.x)) * directionPressed.x);
            force.x += (glm.Sin(glm.Radians(facing.x)) * directionPressed.x +
                           glm.Cos(glm.Radians(facing.x)) * directionPressed.z);
            return force.NormalizedSafe;
        }
    }
}
