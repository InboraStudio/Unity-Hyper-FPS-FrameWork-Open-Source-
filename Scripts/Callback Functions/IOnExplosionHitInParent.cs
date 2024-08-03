using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IOnExplosionHitInParent
    {
        void OnExplosionHitInParent(HitInfo hitInfo);
    }
}