using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite
{
    internal class ITickable
    {
        public ITickable()
        {
            Init();
        }
        protected void Init()
        {
            Tick.PreUpdate += PreUpdate;
            Tick.Update += Update;
            Tick.PostUpdate += PostUpdate;
            OnInit();
        }
        public void Disable()
        {
            Tick.PreUpdate -= PreUpdate;
            Tick.Update -= Update;
            Tick.PostUpdate -= PostUpdate;
            OnDisable();
        }
        virtual protected void OnDisable()
        {

        }
        virtual protected void OnInit()
        {

        }
        virtual protected void PreUpdate()
        {

        }
        virtual protected void Update()
        {

        }
        virtual protected void PostUpdate()
        {

        }
    }
}
