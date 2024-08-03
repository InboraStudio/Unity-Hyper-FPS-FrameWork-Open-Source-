using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Item Input")]
    public class ItemInput : MonoBehaviour
    {
        public bool toggleAim = true;

        public Controls controls;
        public CharacterInput characterInput { get; set; }

        public bool reloadInput { get; set; }
        public bool fireModeSwitchInput { get; set; }
        public bool sightModeSwitchInput { get; set; }
        public bool aimInput { get; set; }
        public bool dropInput { get; set; }

        public InputAction throwAction { get; set; }

        private void Awake()
        {
            characterInput = GetComponentInParent<CharacterInput>();
            controls = new Controls();
            controls.Firearm.Enable();
            controls.Throwable.Enable();
            throwAction = controls.Throwable.Throw;

            AddInputListner();
        }

        private void OnEnable()
        {
            aimInput = false;
        }

        private void Update()
        {
            reloadInput = controls.Firearm.Reload.triggered;
            fireModeSwitchInput = controls.Firearm.FireModeSwich.triggered;
            sightModeSwitchInput = controls.Firearm.SightModeSwitch.triggered;
            dropInput = controls.Firearm.Drop.triggered;
        }

        private void AddInputListner()
        {
            //Aim
            controls.Firearm.Aim.performed += context =>
            {
                if (toggleAim) aimInput = !aimInput;
                else aimInput = true;
            };

            controls.Firearm.Aim.canceled += context =>
            {
                if (!toggleAim) aimInput = false;
            };
        }
    }
}