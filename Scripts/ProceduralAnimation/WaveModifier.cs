using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Modifiers/Wave Modifier"), RequireComponent(typeof(ProceduralAnimation))]
    public class WaveModifier : ProceduralAnimationModifier
    {
        public float speed = 5;
        public float amount = 1;
        public WaveProfile position = new WaveProfile();
        public WaveProfile rotation = new WaveProfile();
        
        public override Vector3 defaultPosition { get => position.result; set => base.defaultPosition = value; }
        public override Vector3 defaultRotation { get => rotation.result; set => base.defaultRotation = value; }

        private void Update()
        {
            position.Update(speed, amount);
            rotation.Update(speed, amount);
        }

        [Serializable]
        public class WaveProfile
        {
            public Vector3 amount;
            public Vector3 speed = new Vector3(1, 1, 1);

            [HideInInspector]
            public Vector3 result;
            private Vector3 time;

            public void Update(float globalSpeed, float globalAmount)
            {
                time.x += Time.deltaTime * speed.x * globalSpeed;
                time.y += Time.deltaTime * speed.y * globalSpeed;
                time.z += Time.deltaTime * speed.z * globalSpeed;

                result.x = amount.x * speed.x * Mathf.Sin(time.x) * globalAmount;
                result.y = amount.y * speed.y * Mathf.Sin(time.y) * globalAmount;
                result.z = amount.z * speed.z * Mathf.Sin(time.z) * globalAmount;
            }
        }
    }
}