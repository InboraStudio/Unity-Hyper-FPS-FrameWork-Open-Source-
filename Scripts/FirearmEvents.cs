using System;
using UnityEngine.Events;

namespace Inbora.FPSFramework
{
    [Serializable]
    public class FirearmEvents
    {
        public UnityEvent OnFire;
        public UnityEvent OnReload;
        public UnityEvent OnReloadCancel;
        public UnityEvent OnFireModeChange;
    }
}