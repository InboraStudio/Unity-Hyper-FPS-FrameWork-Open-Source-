using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Inbora.FPSFramework.Animation
{
    [Serializable]
    public class ProceduralAnimationConnection
    {
        public ConnectionType type;
        public ProceduralAnimation clip;

        public enum ConnectionType
        {
            None = 0,
            SyncAll = 1,
            SyncTrigger = 2,
            SyncProgress = 3,
            Avoid = 4
        }
    }
}