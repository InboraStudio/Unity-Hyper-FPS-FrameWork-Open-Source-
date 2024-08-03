using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IOnExplosionHit
    {
        void OnExplosionHit(HitInfo hitInfo);
    }
}