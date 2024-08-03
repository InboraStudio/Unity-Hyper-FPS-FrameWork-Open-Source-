using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework 
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Door")]
    public class Door : MonoBehaviour, IInteractable
    {
        [Tooltip("What's going to show to the player when opening or closing the door")]
        public string interactionName = "Open/Close";

        public Transform pivot;
        public float roughness = 10;
        public Vector3 openRotation = new Vector3(0, -90, 0);

        private Vector3 targetRotation;
        private Vector3 defaultRotation;
        private float timer;

        public bool isOpen { get; protected set; }

        private void Start()
        {
            defaultRotation = pivot.eulerAngles;
        }

        private void Update()
        {
            pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(defaultRotation + targetRotation), Time.deltaTime * roughness);
        }

        public string GetInteractionName()
        {
            return interactionName;
        }

        public void Interact(InteractablesManager source)
        {
            isOpen = !isOpen;

            if(isOpen)
            {
                Vector3 dir = (source.transform.position - transform.position).normalized;
                targetRotation.x = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * openRotation.x;
                targetRotation.y = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * openRotation.y;
                targetRotation.z = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * openRotation.z;
            }
            else
            {
                targetRotation = Vector3.zero;
            }
        }

        public void Interact(Transform source)
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                Vector3 dir = (source.transform.position - transform.position).normalized;
                targetRotation.x = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * openRotation.x;
                targetRotation.y = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * openRotation.y;
                targetRotation.z = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * openRotation.z;
            }
            else
            {
                targetRotation = Vector3.zero;
            }
        }
    }
}