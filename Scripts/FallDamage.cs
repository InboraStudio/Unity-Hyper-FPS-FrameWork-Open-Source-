using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.Experimental
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Fall Damage")]
    [RequireComponent(typeof(CharacterController), typeof(Actor), typeof(HealthSystem))]
    public class FallDamage : MonoBehaviour
    {
        private CharacterController controller;
        private Actor actor;
        private HealthSystem healthSystem;

        public float velocity = 10;
        public float damage = 30;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            actor = GetComponent<Actor>();
            healthSystem = GetComponent<HealthSystem>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.transform.TryGetComponent(out IgnoreFallDamage ignore)) return;
            Invoke(nameof(Damage), 0);
        }

        private void Damage()
        {
            float damage = 0;

            if (controller.velocity.y < -velocity)
            {
                damage = this.damage * (controller.velocity.y / velocity);
                damage = Mathf.Abs(damage);
                healthSystem.Damage(damage, actor);
            }
        }
    }
}