using Voxel_Engine_Rewrite.Flow;
using Voxel_Engine_Rewrite.src;
using Voxel_Engine_Rewrite.src.Input;
using Voxel_Engine_Rewrite.src.Render;

namespace Voxel_Engine_Rewrite 
{
    internal class EntryPoint
    {
        static void Main(string[] args)
        {
            Window.Init();
            Input.Init();
            Game.Init();
            Game.Run();
        }
    }
}