using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Procedural Animation Trigger")]
    public class ProceduralAnimationTrigger : MonoBehaviour
    {
        public InputAction inputAction;
        public TriggerType type = TriggerType.None;

        [Space]
        public UnityEvent onTriggerEnter;
        public UnityEvent onTrigger;
        public UnityEvent onTriggerExit;

        public ProceduralAnimation Animation { get; private set; }

        private bool isTriggered;
        private bool prevTriggered;

        private void Start()
        {
            Animation = GetComponent<ProceduralAnimation>();

            inputAction.Enable();
            inputAction.performed += context =>
            {
                if (type == TriggerType.Toggle) isTriggered = !isTriggered;
                else isTriggered = true;
            };

            inputAction.canceled += context =>
            {
                if (type == TriggerType.Hold)
                isTriggered = false;
            };
        }

        private void Update()
        {
            //Call events
            if (prevTriggered != isTriggered) onTrigger?.Invoke();
            if (prevTriggered && !isTriggered) onTriggerEnter?.Invoke();
            if (!prevTriggered && isTriggered) onTriggerExit?.Invoke();

            //Update trigger state if it's allowed.
            if(type != TriggerType.None) Animation?.SetTrigger(isTriggered);
        }

        public enum TriggerType
        {
            None = 0,
            Toggle = 1,
            Hold = 2,
        }
    }
}