using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface ICharacterController
    {
        void SetSpeed(float walkSpeed, float runSpeed, float tacticalSprintSpeed);
        void ResetSpeed();
        float sensitivity { get; }
        float sprintSpeed { get; }
        float walkSpeed { get; }
        float tacticalSprintSpeed { get; }
        float tacticalSprintAmount { get; }
        bool MaxedCameraRotation();
    }
}