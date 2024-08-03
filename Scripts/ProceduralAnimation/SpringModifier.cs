using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Modifiers/Spring Modifier"), RequireComponent(typeof(ProceduralAnimation))]
    public class SpringModifier : ProceduralAnimationModifier
    {
        public float speed = 1;
        public SpringVector3 position = new SpringVector3();
        public SpringVector3 rotation = new SpringVector3();

        [Space]
        public UnityEvent OnTrigger;

        private void Update()
        {
            position.Update(speed);
            rotation.Update(speed);

            positionOffset = position.result;
            rotationOffset = rotation.result;
        }

        public override void Trigger()
        {
            position.Start();
            rotation.Start();
            OnTrigger?.Invoke();
        }
    }
}