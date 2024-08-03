using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [RequireComponent(typeof(ProceduralAnimation))]
    public abstract class ProceduralAnimationModifier : MonoBehaviour
    {
        /// <summary>
        /// clip offset position result for this modifier
        /// </summary>
        public virtual Vector3 positionOffset { get; set; }

        /// <summary>
        /// clip offset rotation result for this modifier
        /// </summary>
        public virtual Vector3 rotationOffset { get; set; }

        /// <summary>
        /// default position for this modifier
        /// </summary>
        public virtual Vector3 defaultPosition { get; set; }

        /// <summary>
        /// default rotation for this modifier
        /// </summary>
        public virtual Vector3 defaultRotation { get; set; }

        /// <summary>
        /// final position result for this modifier
        /// </summary>
        public virtual Vector3 targetPosition { get; set; }

        /// <summary>
        /// final rotation result for this modifier
        /// </summary>
        public virtual Vector3 targetRotation { get; set; }


        public virtual void Trigger()
        {

        }
    }
}