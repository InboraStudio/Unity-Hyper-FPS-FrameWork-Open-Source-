using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Death Camera")]
    [RequireComponent(typeof(Camera), typeof(AudioListener))]
    public class DeathCamera : MonoBehaviour
    {
        public float lookDistance = 5;
        public Vector3 lookOffset;

        public Camera Camera { get; set; }
        public AudioListener AudioListener { get; set; }

        private void Awake()
        {
            Camera = GetComponent<Camera>();
            AudioListener = GetComponent<AudioListener>();

            Disable();
        }

        /// <summary>
        /// enables death camera
        /// </summary>
        /// <param name="killer">the killer to look at</param>
        public void Enable(Actor self, Actor killer)
        {
            if (!killer) return;
            Vector3 insideUnitSpherePosition = (Random.insideUnitSphere * lookDistance);
            insideUnitSpherePosition.y = 0;

            transform.position = self.transform.position + insideUnitSpherePosition + lookOffset;
            transform.LookAt(killer.transform.position);

            Camera.enabled = true;
            AudioListener.enabled = true;
        }

        public void Disable()
        {
            Camera.enabled = false;
            AudioListener.enabled = false;
        }
    }
}