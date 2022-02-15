using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Entity;

namespace Voxel_Engine_Rewrite.src.Render
{
    internal static class Camera
    {
        static vec3 cameraOffset = new vec3(0,1,0);
        static vec3 up = new vec3(0,1,0);  
        public static vec3 GetLocation()
        {
            return Player.GetPosition() + cameraOffset;
        }
        public static vec2 GetYawPitch()
        {
            return Player.GetYawPitch();
        }
        public static vec3 GetDirection()
        {
            float yaw = GetYawPitch().x;
            float pitch = GetYawPitch().y;
            vec3 direction = new vec3();
            direction.x = glm.Cos(glm.Radians(yaw)) * glm.Cos(glm.Radians(pitch));
            direction.y = glm.Sin(glm.Radians(pitch));
            direction.z = glm.Sin(glm.Radians(yaw)) * glm.Cos(glm.Radians(pitch));

            return glm.Normalized(direction);
        }
        public static mat4 GetLookAtMatrix()
        {
            vec3 location = GetLocation();
            vec3 direction = GetDirection();
            return mat4.LookAt(location, location + direction, up);
        }
    }
}
