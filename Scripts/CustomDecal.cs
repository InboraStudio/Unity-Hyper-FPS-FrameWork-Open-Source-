using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Audio System/Audio Low Pass Area")]
    public class CustomDecal : MonoBehaviour
    {
        public GameObject decalVFX;
        public float lifeTime = 60;
        public float materialStrenght = 10;
    }
}