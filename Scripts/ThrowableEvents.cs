using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Throwable Events")]
    public class ThrowableEvents : MonoBehaviour
    {
        private Throwable throwable;
        
        private void Start()
        {
            if (GetComponent<Throwable>()) throwable = GetComponent<Throwable>();
            if (GetComponentInParent<Throwable>()) throwable = GetComponentInParent<Throwable>();
            if (GetComponentInChildren<Throwable>()) throwable = GetComponentInChildren<Throwable>();
        }
        public void Throw()
        {
            throwable.Throw();
        }
    }
}