using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.Flow.States;

namespace Voxel_Engine_Rewrite.Flow
{
    internal class StateController
    {
        private State _state;

        public StateController(State state)
        {
            SetState(state);
        }
        private State SetState(State state)
        {
            _state?.Disable();
            _state = state;
            _state.Init();
            return _state;
        }
        public State Next()
        {
            if (_state is Active) return SetState(new Paused());
            else return SetState(new Active());
        }
        public void Update()
        {
            _state.Update();
        }
    }
}
