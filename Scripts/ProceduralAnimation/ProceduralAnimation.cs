using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [RequireComponent(typeof(ProceduralAnimationModifiersManager), typeof(ProceduralAnimationTrigger)), AddComponentMenu("Inbora/FPS Framework/Animation/Procedural Animation")]
    public class ProceduralAnimation : MonoBehaviour
    {
        [Header("Base")]
        public string Name = "Animation Clip";
        public float length = 0.15f;
        [Range(0, 1)] public float weight = 1;

        [Header("Value")]
        public Vector3 defaultPosition;
        public Vector3 defaultRotation;

        [Space]
        public Vector3 position;
        public Vector3 rotation;

        [Space]
        public List<ProceduralAnimationConnection> connections = new List<ProceduralAnimationConnection>();

        public ProceduralAnimationEvents events;
        public List<CustomProceduralAnimationEvent> customEvents;

        /// <summary>
        /// List of all modifieres applied to this animation
        /// </summary>
        public ProceduralAnimationModifiersManager modifiers { get; private set; }

        /// <summary>
        /// final position result for this clip
        /// </summary>
        public Vector3 targetPosition
        {
            get
            {
                Vector3 result = Vector3.zero;

                if(modifiers)
                    result = (modifiers.GetPositionOffset() + Vector3.Lerp(defaultPosition + modifiers.GetDefaultPosition(), position + modifiers.GetPosition(), progress)) * weight;

                return result;
            }
        }

        /// <summary>
        /// final rotation result for this clip
        /// </summary>
        public Vector3 targetRotation
        {
            get
            {
                Vector3 result = Vector3.zero;

                if (modifiers)
                    result = (modifiers.GetRotationOffset() + Vector3.Lerp(defaultRotation + modifiers.GetDefaultRotation(), rotation + modifiers.GetRotation(), progress)) * weight;

                return result;
            }
        }

        /// <summary>
        /// current animation progress by value from 0 to 1
        /// </summary>
        public float progress { get; set; }

        //acutal velocity
        private float currentVelocity;

        /// <summary>
        /// current animation movement speed
        /// </summary>
        public float velocity { get => currentVelocity; }

        public bool IsTriggered { get; set; }

        private void Start()
        {
            GetComponentInParent<ProceduralAnimator>().RefreshClips();
            modifiers = GetComponent<ProceduralAnimationModifiersManager>();
        }

        private void Update()
        {
            //Handles the custom events and progress for this animation.
            HandleEvents();
            HandleProgress();
        }

        /// <summary>
        /// sets the current trigger value to value changing IsTriggered value directly is not recommended
        /// </summary>
        /// <param name="value"></param>
        public void SetTrigger(bool value)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                ProceduralAnimationConnection connection = connections[i];
                if (connection != null && connection.type == ProceduralAnimationConnection.ConnectionType.SyncTrigger || connection.type == ProceduralAnimationConnection.ConnectionType.SyncAll)
                {
                    Debug.Log($"Can't trigger {Name} due to synced connection with {connection.clip.Name}");
                }
            }

            IsTriggered = value;
        }

        /// <summary>
        /// updates the current animation progress
        /// </summary>
        private void HandleProgress()
        {
            bool hasAvoidConnection = false;
            bool hasSyncProgressConnection = false;

            for (int i = 0; i < connections.Count; i++)
            {
                ProceduralAnimationConnection connection = connections[i];
                if (connection.clip == null)
                {
                    if (connection.type == ProceduralAnimationConnection.ConnectionType.Avoid && connection.clip.IsTriggered)
                    {
                        hasAvoidConnection = true;
                    }

                    if (connection.type == ProceduralAnimationConnection.ConnectionType.SyncProgress
                        && connection.clip.IsTriggered
                        || connection.type == ProceduralAnimationConnection.ConnectionType.SyncAll)
                    {
                        hasSyncProgressConnection = true;
                        progress = connection.clip.progress;
                    }

                    if (connection.type == ProceduralAnimationConnection.ConnectionType.SyncTrigger
                        && connection.clip.IsTriggered
                        || connection.type == ProceduralAnimationConnection.ConnectionType.SyncAll)
                    {
                        IsTriggered = connection.clip.IsTriggered;
                    }
                }
            }

            if (hasSyncProgressConnection) return;
            float targetValue = IsTriggered && !hasAvoidConnection ? 1 : 0;

            progress = Mathf.SmoothDamp(progress, targetValue, ref currentVelocity, length);
        }

        private void HandleEvents()
        {
            events.HandleEvents(this);
            foreach (CustomProceduralAnimationEvent animationEvent in customEvents) animationEvent.UpdateEvent(this);
        }
    }
}