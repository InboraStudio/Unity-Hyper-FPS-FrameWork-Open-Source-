using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Modifiers/Procedural Animation Modifiers Manager")]
    public class ProceduralAnimationModifiersManager : MonoBehaviour
    {
        public List<ProceduralAnimationModifier> modifiers { get; set; } = new List<ProceduralAnimationModifier>();

        private void Start()
        {
            RefreshClips();
        }

        /// <summary>
        /// returns all the clip modifiers for this clip in a List of ProceduralAnimationClip and refreshes the animtor clips 
        /// </summary>
        public List<ProceduralAnimationModifier> RefreshClips()
        {
            modifiers = GetComponentsInChildren<ProceduralAnimationModifier>().ToList();

            return modifiers;
        }

        /// <summary>
        /// returns result position for all modifiers
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.targetPosition;

            return result;
        }

        /// <summary>
        /// returns result rotation for all modifiers
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRotation()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.targetRotation;

            return result;
        }

        /// <summary>
        /// clip offset position result for this modifier
        /// </summary>
        public Vector3 GetPositionOffset()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.positionOffset;

            return result;
        }

        /// <summary>
        /// clip offset rotation result for this modifier
        /// </summary>
        public Vector3 GetRotationOffset()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.rotationOffset;

            return result;
        }

        /// <summary>
        /// default position for this modifier
        /// </summary>
        public Vector3 GetDefaultPosition()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.defaultPosition;

            return result;
        }

        /// <summary>
        /// default rotation for this modifier
        /// </summary>
        public Vector3 GetDefaultRotation()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.defaultRotation;

            return result;
        }

        /// <summary>
        /// final position result for this modifier
        /// </summary>
        public Vector3 GetTargetPosition()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.targetPosition;

            return result;
        }

        /// <summary>
        /// final rotation result for this modifier
        /// </summary>
        public Vector3 GetTargetRotation()
        {
            Vector3 result = Vector3.zero;

            foreach (ProceduralAnimationModifier modifier in modifiers) result += modifier.targetRotation;

            return result;
        }
    }
}