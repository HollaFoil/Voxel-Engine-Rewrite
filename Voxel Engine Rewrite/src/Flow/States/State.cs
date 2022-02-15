using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.Flow.States
{
    abstract class State
    {
        abstract public void Init();
        abstract public void Disable();
        abstract public void Update();
    }
}
