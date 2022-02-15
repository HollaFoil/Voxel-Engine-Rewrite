using GLFW;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.Flow;
using Voxel_Engine_Rewrite.Flow.States;

namespace Voxel_Engine_Rewrite.src
{
    internal static class Game
    {
        private static StateController GameState;
        public static void Run()
        {
            GameState.Next();
            while (true)
            {
                Glfw.PollEvents();
                GameState.Update();
            }
        }
        public static void Init()
        {
            GameState = new StateController(new Paused());
        }
        public static void Stop()
        {

        }
    }
}
