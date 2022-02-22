using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Event.Events;

namespace Voxel_Engine_Rewrite.src.Entity
{
    public class Player : IEntity
    {
        private vec3 directionPressed;
        private float speed = 0.02f;
        public Player()
        {
            DirectionalKeyPressEvent.Listeners += OnDirectionalKeyPress;
            MouseMovedEvent.Listeners += OnMouseMoved;
            position = new vec3();
            facing = new vec2();
            directionPressed = new vec3();
        }
        private void OnDirectionalKeyPress(DirectionalKeyPressEvent e)
        {
            directionPressed = e.GetDirection();
        }
        private void OnMouseMoved(MouseMovedEvent e)
        {
            facing += e.GetChange();
            if (facing.x <= -180.0f) facing.x += 360.0f;
            if (facing.x > 180.0f) facing.x -= 360.0f;
            if (facing.y < -90.0f) facing.y = -90.0f;
            if (facing.y > 90.0f) facing.y = 90.0f;
        }
        public void Update(int elapsedTime)
        {
            position += GetMovementDirection() * elapsedTime * speed;
            Console.WriteLine((position + "\n") + GetYawPitch() + "\n");
        }
        private vec3 GetMovementDirection()
        {
            vec3 force = new vec3(0,0,0);
            force.y += directionPressed.y;
            force.z += (glm.Sin(glm.Radians(facing.x + 180)) * directionPressed.z +
                           glm.Cos(glm.Radians(facing.x)) * directionPressed.x);
            force.x += (glm.Sin(glm.Radians(facing.x)) * directionPressed.x +
                           glm.Cos(glm.Radians(facing.x)) * directionPressed.z);
            return force;
        }
        public static Player GetInstance()
        {
            return Game.GetPlayer();
        }


        vec3 cameraOffset = new vec3(0, 1, 0);
        vec3 up = new vec3(0, 1, 0);
        public vec3 GetCameraLocation()
        {
            return GetPosition() + cameraOffset;
        }
        public vec3 GetDirection()
        {
            float yaw = -GetYawPitch().x;
            float pitch = GetYawPitch().y;
            vec3 direction = new vec3();
            direction.x = glm.Cos(glm.Radians(yaw)) * glm.Cos(glm.Radians(pitch));
            direction.y = glm.Sin(glm.Radians(pitch));
            direction.z = glm.Sin(glm.Radians(yaw)) * glm.Cos(glm.Radians(pitch));

            return glm.Normalized(direction);
        }
        public mat4 GetLookAtMatrix()
        {
            vec3 location = GetCameraLocation();
            vec3 direction = GetDirection();
            return mat4.LookAt(location, location + direction, up);
        }
    }
}
