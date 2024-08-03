using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Interactable")]
    public class Interactable : MonoBehaviour, IInteractable
    {
        public string interactionName = "Interact";
        public UnityEvent OnInteract;

        public string GetInteractionName()
        {
            return interactionName;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Interact(InteractablesManager source)
        {
            OnInteract?.Invoke();
        }
    }
}