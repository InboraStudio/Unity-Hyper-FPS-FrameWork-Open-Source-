using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IOnExplosionHitInChildren
    {
        void OnExplosionHitInChildren(HitInfo hitInfo);
    }
}