using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/Toggle Objects")]
    public class ToggleObjects : MonoBehaviour
    {
        public bool isOn { get; set; }

        public InputAction inputAction;
        public List<GameObject> objects;

        private void Start()
        {
            SetActive(false);
            inputAction.Enable();
            inputAction.performed += context =>
            {
                isOn = !isOn;
                SetActive(isOn);
            };
        }

        /// <summary>
        /// enables or disables all objects in the list (Must be called once not in update)
        /// </summary>
        public void SetActive(bool state)
        {
            isOn = state;
            foreach (GameObject _object in objects) _object.SetActive(isOn);
        }
    }
}