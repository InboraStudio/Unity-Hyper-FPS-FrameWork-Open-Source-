using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IDamageableGroup
    {
        public IDamageable GetDamageable();
        public HumanBodyBones GetBone();
        public float GetDamageMultipler();
    }
}