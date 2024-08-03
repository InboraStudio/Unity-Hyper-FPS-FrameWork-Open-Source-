using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Attachments/Muzzle")]
    public class AttachmentMuzzle : MonoBehaviour
    {
        public AudioProfile fireSFX;
        public ParticleSystem[] muzzleEffects;
    }
}