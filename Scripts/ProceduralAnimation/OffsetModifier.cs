using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Modifiers/Offset Modifier")]
    public class OffsetModifier : ProceduralAnimationModifier
    {
        [Tooltip("clip offset position result for this modifier")]
        public Vector3 m_positionOffset;
        [Tooltip("clip offset rotation result for this modifier")]
        public Vector3 m_rotationOffset;
        [Tooltip("default position for this modifier")]
        public Vector3 m_defaultPosition;
        [Tooltip("default rotation for this modifier")]
        public Vector3 m_defaultRotation;
        [Tooltip("final position result for this modifier")]
        public Vector3 m_targetPosition;
        [Tooltip("final rotation result for this modifier")]
        public Vector3 m_targetRotation;

        private void Update()
        {
            positionOffset = m_positionOffset;
            rotationOffset = m_rotationOffset;
            defaultPosition = m_defaultPosition;
            defaultRotation = m_defaultRotation;
            targetPosition = m_targetPosition;
            targetRotation = m_targetRotation;
        }
    }
}