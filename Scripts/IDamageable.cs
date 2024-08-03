using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IDamageable
    {
        public Transform transform { get; }
        public Actor GetActor();
        public float GetHealth();
        public void Damage(float amount, Actor damageSource);
        public bool IsDead();
        public bool deadConfirmed { get; set; }
        public  Vector3 deathForce { get; set; }
        public float MaxHealth { get; set; }
        public int GetGroupsCount();
        public Ragdoll GetRagdoll();
    }
}