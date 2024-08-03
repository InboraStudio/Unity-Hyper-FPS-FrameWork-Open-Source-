using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Managers/Gamepad Manager")]
    public class GamepadManager : MonoBehaviour
    {
        public static GamepadManager Instance { get; private set; }

        private void Start()
        {
            //Allow one instance at a time
            if (!Instance) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// Vibrates current controller for 0.1s
        /// </summary>
        /// <param name="rightMotorFrequency">The speed of the right motor. The value can vary from 0 to 1.</param>
        /// <param name="leftMotorFrequency">The speed of the left motor. The value can vary from 0 to 1.</param>
        public void Vibrate(float rightMotorFrequency, float leftMotorFrequency)
        {
            BeginVibration(rightMotorFrequency, leftMotorFrequency, 0.1f);
        }

        /// <summary>
        /// Vibrates current controller for 0.1s
        /// </summary>
        /// <param name="rightMotorFrequency">The speed of the right motor. The value can vary from 0 to 1.</param>
        /// <param name="leftMotorFrequency">The speed of the left motor. The value can vary from 0 to 1.</param>
        /// <param name="duration">The time needed to end vibration</param>
        public void BeginVibration(float rightMotorFrequency, float leftMotorFrequency, float duration)
        {
            Gamepad.current?.SetMotorSpeeds(leftMotorFrequency, rightMotorFrequency);

            Invoke(nameof(EndVibration), duration);
        }

        /// <summary>
        /// End all vibrations
        /// </summary>
        public void EndVibration()
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }
    }
}