using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Entity;

namespace Voxel_Engine_Rewrite.Flow.States
{
    internal class Active : State
    {
        private static int targetFps = 60;
        private static int targetTickRate = 20;
        private static src.Util.Timer frameTimer;
        private static src.Util.Timer tickTimer;
        private static src.Util.Timer inputTimer;
        public override void Init()
        {
            inputTimer = new src.Util.Timer();
            frameTimer = new src.Util.Timer(targetFps);
            tickTimer = new src.Util.Timer(targetTickRate);
            Console.WriteLine("Active state initialized");
        }
        public override void Update()
        {
            
            if (tickTimer.Next(out int temp)) Tick.DoTick();
            if (frameTimer.Next(out int elapsedTime)) Player.Update(elapsedTime);
        }
        public override void Disable()
        {
            Console.WriteLine("Active state disabled");
        }
    }
}
