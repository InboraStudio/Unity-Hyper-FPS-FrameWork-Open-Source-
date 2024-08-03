using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.Examples
{
    [AddComponentMenu("Inbora/FPS Framework/Examples/Firearm Hit Callbacks Example")]
    public class FirearmHitCallbacksExample : MonoBehaviour, IOnHit, IOnHitInChildren, IOnHitInParent
    {
        public void OnHit(HitInfo info)
        {
            print("hit");
        }

        public void OnHitInChildren(HitInfo hitInfo)
        {
            print("hitInChildren");
        }

        public void OnHitInParent(HitInfo hitInfo)
        {
            print("hitInParent");
        }
    }
}