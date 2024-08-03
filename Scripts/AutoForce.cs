using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/Auto Force")]
    [RequireComponent(typeof(Rigidbody))]
    public class AutoForce : MonoBehaviour
    {
        public ForceMode forceMode = ForceMode.VelocityChange;
        public Vector3 forceValue = Vector3.zero;
        public Vector3 torqueValue = Vector3.zero;
        public bool stopOnCollision = true;

        private Rigidbody body;
        private bool isOn = true;

        private void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!isOn) return;

            body.AddForce(forceValue, forceMode);
            body.AddRelativeTorque(torqueValue, forceMode);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!stopOnCollision) return;
            isOn = false;
        }
    }
}