using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.Flow.States;

namespace Voxel_Engine_Rewrite.src.Flow.States
{
    internal class Stopped : State
    {
        public override void Init()
        {
            Console.WriteLine("Stopped state initialized");
        }
        public override void Update()
        {
            Console.WriteLine("Currently Stopped");
        }
        public override void Disable()
        {
            Console.WriteLine("Stopped state disabled");
        }
    }
}
