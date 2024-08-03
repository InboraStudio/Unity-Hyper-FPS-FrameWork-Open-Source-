using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Menu System")]
    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : MonoBehaviour
    {
        public string Name = "Menu";
        public float fadeTime = 0.05f;

        public bool isOpened { get; set; }

        private CanvasGroup canvasGroup;
        private float fadeVelocity;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            float targetAlpha = isOpened ? 1 : 0;
            if (canvasGroup.alpha != targetAlpha) canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, targetAlpha, ref fadeVelocity, fadeTime);
        }

        public void Open()
        {
            isOpened = true;
        }

        public void Close()
        {
            isOpened = false;
        }
    }
}