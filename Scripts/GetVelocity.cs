using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/Get Velocity")]
    public class GetVelocity : MonoBehaviour
    {
        public float velocity;
        public Vector3 direction;

        private float previousDistance;
        private Vector3 previousPosition;

        private void FixedUpdate()
        {
            direction = (transform.position - previousPosition) * velocity;

            previousDistance = Vector3.Distance(transform.position, previousPosition);
            velocity = previousDistance / Time.fixedDeltaTime;

            previousPosition = transform.position;
        }
    }
}