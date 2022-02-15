using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.Flow.States
{
    internal class Paused : State
    {
        public override void Init()
        {
            Console.WriteLine("Paused state initialized");
        }
        public override void Update()
        {
            Console.WriteLine("Currently Paused");
        }
        public override void Disable()
        {
            Console.WriteLine("Paused state disabled");
        }
    }
}
