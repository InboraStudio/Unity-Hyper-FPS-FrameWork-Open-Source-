using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace Inbora.FPSFramework.Animation
{
    [Serializable]
    public class ProceduralAnimationEvents
    {
        public UnityEvent OnTriggerEnter;
        public UnityEvent OnTriggerStay;
        public UnityEvent OnTriggerExit;

        private bool isPreviouslyTriggered;

        /// <summary>
        /// calles every event in the right time
        /// </summary>
        /// <param name="target"></param>
        public void HandleEvents(ProceduralAnimation target)
        {
            bool hasAvoidConnection = false;

            for (int i = 0; i < target.connections.Count; i++)
            {
                ProceduralAnimationConnection connection = target.connections[i];
                if (connection != null &&
                    connection.type == ProceduralAnimationConnection.ConnectionType.Avoid &&
                    connection.clip.IsTriggered)
                {

                    hasAvoidConnection = true;
                }
            }

            if (!target.gameObject.activeInHierarchy || hasAvoidConnection) return;

            if (!isPreviouslyTriggered && target.IsTriggered)
            {
                OnTriggerEnter?.Invoke();
            }

            if (target.IsTriggered) OnTriggerStay?.Invoke();

            if (isPreviouslyTriggered && !target.IsTriggered)
            {
                OnTriggerExit?.Invoke();
            }

            isPreviouslyTriggered = target.IsTriggered;
        }
    }
}