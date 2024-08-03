using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Modifiers/Kick Modifier")]
    public class KickModifier : ProceduralAnimationModifier
    {
        public float triggerRate = 1e+08f;
        public float positionRoughness = 10;
        public float rotationRoughness = 10;
        public Vector3 position = new Vector3(0.5f, 0.5f, -4f);
        public Vector3 rotation = new Vector3(-5, 15, 15);
        [Range(0, 1)] public float positionWeight = 1;
        [Range(0, 1)] public float rotationWeight = 1;

        [Space]
        public UnityEvent OnTrigger = new UnityEvent();

        private Vector3 currentRotation;
        private Vector3 currentPosition;
        private Vector3 rotationOutput;

        private float nextTrigger;

        private void FixedUpdate()
        {
            currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, 35 * Time.fixedDeltaTime);
            currentPosition = Vector3.Lerp(currentPosition, Vector3.zero, 35 * Time.fixedDeltaTime);

            positionOffset = Vector3.Slerp(positionOffset, currentPosition, positionRoughness * Time.fixedDeltaTime);
            rotationOutput = Vector3.Slerp(rotationOutput, currentRotation, rotationRoughness * Time.fixedDeltaTime);
            rotationOffset = rotationOutput;
        }

        public override void Trigger()
        {
            if (Time.time > nextTrigger)
            {
                nextTrigger = Time.time + 1 / triggerRate;
                currentPosition += new Vector3(Random.Range(position.x, -position.x), Random.Range(position.y, -position.y), position.z) * positionWeight;
                currentRotation += new Vector3(rotation.x, Random.Range(rotation.y, -rotation.y), Random.Range(rotation.z, -rotation.z)) * rotationWeight;

                OnTrigger?.Invoke();
            }
        }
    }
}