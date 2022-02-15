using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.Event
{
    public abstract class Event<T> where T : class
    {
        public delegate void CallBack(T e);
        public static event CallBack Listeners;
        protected abstract T GetEvent();
        public void Fire()
        {
            Listeners?.Invoke(GetEvent());
        }
    }
}
