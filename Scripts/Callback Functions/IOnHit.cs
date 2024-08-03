using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IOnHit
    {
        void OnHit(HitInfo hitInfo);
    }
}