using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Damage Indicator")]
    [RequireComponent(typeof(CanvasGroup))]
    public class DamageIndicator : MonoBehaviour
    {
        public float alpha = 0.4f;
        public float fadeSpeed = 2;
        public CanvasGroup screenEffects;

        private void Start()
        {
            screenEffects = GetComponent<CanvasGroup>();
            screenEffects.alpha = 0;
        }

        private void Update()
        {
            if (screenEffects.alpha != 0)
            {
                screenEffects.alpha = Mathf.Lerp(screenEffects.alpha, 0, Time.deltaTime * fadeSpeed);
            }
        }

        public void Show()
        {
            screenEffects.alpha = alpha;
        }

        public void Show(float alpha)
        {
            screenEffects.alpha = alpha;
        }
    }
}