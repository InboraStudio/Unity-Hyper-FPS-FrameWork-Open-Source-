using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [System.Serializable]
    public class CustomProceduralAnimationEvent
    {
        /// <summary>
        /// Which type of animation event is this. 
        /// (BackAndForth) Calls the event when the aniamtion is fading in & out.
        /// (BackOnly) Only calls the event when the animation is fading out.
        /// (ForthOnly) Only calls the event when the animation is fading in.
        /// </summary>
        [Tooltip(" Which type of animation event is this. 1-(BackAndForth) Calls the event when the aniamtion is fading in & out. 2-(BackOnly) Only calls the event when the animation is fading out. 3-(ForthOnly) Only calls the event when the animation is fading in.")]
        public CustomProceduralAnimationEventType type;

        /// <summary>
        /// Where the event should be called, a location of 0.5 will playt the animation in the half of it.
        /// </summary>
        [Tooltip("Where the event should be called, a location of 0.5 will playt the animation in the half of it.")]
        [Range(0.01f, 0.99f)] public float location;

        /// <summary>
        /// The event that will be called.
        /// </summary>
        [Tooltip("The event that will be called.")]
        public UnityEvent onLocate;

        private float current;
        private float previous;

        /// <summary>
        /// Manages the event with its type internally.
        /// </summary>
        /// <param name="parentAnimation"></param>
        public void UpdateEvent(ProceduralAnimation parentAnimation)
        {
            current = parentAnimation.progress;

            if(current > location && previous < location)
            {
                if (type == CustomProceduralAnimationEventType.BackAndForth)
                {
                    onLocate?.Invoke();
                }

                if (type == CustomProceduralAnimationEventType.ForthOnly)
                {
                    onLocate?.Invoke();
                }
            }

            if(current < location && previous > location)
            {
                if (type == CustomProceduralAnimationEventType.BackAndForth)
                {
                    onLocate?.Invoke();
                }

                if (type == CustomProceduralAnimationEventType.BackOnly)
                {
                    onLocate?.Invoke();
                }
            }

            previous = parentAnimation.progress;
        }

        public void Print()
        {
            Debug.Log("a");
        }

        public enum CustomProceduralAnimationEventType
        {
            BackAndForth,
            BackOnly,
            ForthOnly
        }
    }
}