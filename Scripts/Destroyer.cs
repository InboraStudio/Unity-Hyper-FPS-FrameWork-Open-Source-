using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/Destroyer")]
    public class Destroyer : MonoBehaviour
    {
        public bool destoryOnAwake = true;
        public float destroyDelay = 1;

        private void Awake()
        {
            if (destoryOnAwake) Destory(destroyDelay);
        }

        public void Destory(float destoryDelay)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}