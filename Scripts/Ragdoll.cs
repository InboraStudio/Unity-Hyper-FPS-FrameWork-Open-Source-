using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Ragdoll")]
    public class Ragdoll : MonoBehaviour
    {
        public Rigidbody[] rigidbodies { get; set; }
        public Animator animator;

        private ICharacterController Controller;

        private void Start()
        {
            Controller = GetComponent<ICharacterController>();
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            if (!animator) animator = GetComponent<Animator>();

            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb != GetComponent<Rigidbody>())
                {
                    rb.interpolation = RigidbodyInterpolation.Interpolate;
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }
            }

            Disable();
        }

        public void Enable()
        {
            if (animator) animator.enabled = false;

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
            }
        }

        public void Enable(Vector3 deathForce)
        {
            if (animator) animator.enabled = false;

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                if (rigidbody != GetComponent<Rigidbody>())
                {
                    rigidbody.isKinematic = false;

                    if (Controller != null)
                        rigidbody.AddForce(deathForce, ForceMode.VelocityChange);

                }
            }
        }

        public void Disable()
        {
            if (animator) animator.enabled = true;

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                if (rigidbody != GetComponent<Rigidbody>())
                {
                    rigidbody.isKinematic = true;
                }
            }
        }
    }
}