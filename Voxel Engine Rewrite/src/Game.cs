using GLFW;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.Flow;
using Voxel_Engine_Rewrite.Flow.States;
using Voxel_Engine_Rewrite.src.Entity;

namespace Voxel_Engine_Rewrite.src
{
    internal static class Game
    {
        private static StateController? GameState;
        private static Player? player;
        private static World.World? world;
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
            player = new Player();
            world = new World.World();
        }
        public static void Stop()
        {

        }
        public static Player? GetPlayer()
        {
            return player;
        }
        public static World.World? GetWorld()
        {
            return world;
        }
    }
}
