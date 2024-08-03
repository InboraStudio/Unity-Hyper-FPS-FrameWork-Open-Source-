using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Ping")]
    [RequireComponent(typeof(FloatingRect))]
    public class Ping : MonoBehaviour
    {
        public AudioProfile soundEffect;
    }
}