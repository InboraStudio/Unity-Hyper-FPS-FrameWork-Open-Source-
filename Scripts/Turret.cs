using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Turret")]
    public class Turret : MonoBehaviour, IOnAnyHit, IOnAnyHitInChildren, IOnAnyHitInParent
    {
        public GameObject target;
        public Transform muzzle;
        public Firearm firearm;
        public float lookSpeed = 50;
        public float forgivenessTime = 5;

        Vector3 enemyPos;

        private bool forceFocus = false;
        private float forceFocusDuration = 2.0f;
        private float forceFocusTimer = 0.0f;
        private bool greenLight = false;
        private float forgivenessTimer = 0;

        private void Start()
        {
            Invoke(nameof(GiveGreenLight), Random.Range(1, 10));

            // cache target reference on start
            if (target == null)
                target = FindObjectOfType<CharacterController>()?.gameObject;
        }

        private void GiveGreenLight()
        {
            forgivenessTimer = 0;
            greenLight = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (forgivenessTimer > forgivenessTime) greenLight = false;

            if (!greenLight)
            {
                forgivenessTimer += Time.deltaTime;
                return;
            }

            forgivenessTimer = 0;
            if (target != null)
            {
                Vector3 directionToTarget = target.transform.position - transform.position;

                if (forceFocus)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToTarget, out hit))
                    {
                        if (hit.transform == target.transform) // Check if the first hit object is the player.
                        {
                            enemyPos = Vector3.Lerp(enemyPos, target.transform.position, Time.deltaTime * lookSpeed);
                            if (muzzle) muzzle.LookAt(enemyPos);
                            firearm.Fire(directionToTarget.normalized);
                        }
                    }

                    if (forceFocus) // reduce the timer when forceFocus is active
                    {
                        forceFocusTimer -= Time.deltaTime;
                        if (forceFocusTimer <= 0.0f)
                        {
                            forceFocus = false;
                        }
                    }
                }
            }
            else
            {
                target = FindObjectOfType<CharacterController>()?.gameObject;
            }
        }

        public void OnAnyHit(HitInfo hitInfo)
        {
            forceFocus = true;  // Set the flag to true when hit
            forceFocusTimer = forceFocusDuration;   // reset the timer
            GiveGreenLight();
        }

        public void OnAnyHitInChildren(HitInfo hitInfo)
        {
            forceFocus = true;  // Set the flag to true when hit
            forceFocusTimer = forceFocusDuration;   // reset the timer
            GiveGreenLight();
        }

        public void OnAnyHitInParent(HitInfo hitInfo)
        {
            forceFocus = true;  // Set the flag to true when hit
            forceFocusTimer = forceFocusDuration;   // reset the timer
            GiveGreenLight();
        }
    }
}