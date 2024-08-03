using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public interface IInteractable
    {
        void Interact(InteractablesManager source);
        Transform transform { get; }
        string GetInteractionName();
    }
}