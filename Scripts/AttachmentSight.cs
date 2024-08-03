using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Attachments/Attachment Sight")]
    public class AttachmentSight : MonoBehaviour
    {
        public bool isDefault;
        [Tooltip("Aim presets more than one preset can be used as multi sight")]
        public SightPreset[] presets;

        public int index { get; set; }

        public SightPreset usedPreset
        {
            get
            {
                return presets[index];
            }
        }

        [System.Serializable]
        public class SightPreset
        {
            [Header("Zoom Level")]
            [Tooltip("FOV for the main camera")]
            public float fieldOfView = 50;
            [Tooltip("FOV for the weapon camera")]
            public float weaponFieldOfView = 40;
            [Tooltip("How fast the weapon will aim down the sight")]
            public float aimDownSightSpeed = 5;

            [Header("Offset")]
            [Tooltip("Position of aiming")]
            public Vector3 position;
            [Tooltip("Rotation of aiming")]
            public Vector3 rotation;
            [Tooltip("Position offset while leaning")]
            public Vector3 leanOffset;
        }
    }
}