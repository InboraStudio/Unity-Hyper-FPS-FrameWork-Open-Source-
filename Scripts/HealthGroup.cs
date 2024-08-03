using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Health System/Health Group")]
    public class HealthGroup : MonoBehaviour, IDamageableGroup
    {
        public HumanBodyBones bone;
        public float damageMultipler = 1;

        private IDamageable damageable;

        private void Start()
        {
            damageable = GetComponentInParent<IDamageable>();
        }

        public IDamageable GetDamageable()
        {
            return damageable;
        }

        public HumanBodyBones GetBone()
        {
            return bone;
        }

        public float GetDamageMultipler()
        {
            return damageMultipler;
        }
    }
}