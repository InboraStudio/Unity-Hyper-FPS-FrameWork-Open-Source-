using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Selectable Behavior")]
    public class SelectableBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [Header("Base")]
        public TextMeshProUGUI[] texts;
        [FormerlySerializedAs("canvasGroup")]
        public CanvasGroup clickDot;

        [Header("Settings")]
        [FormerlySerializedAs("imageSize")]
        public float clickDotSize = 1000;
        [FormerlySerializedAs("movementSpeed")]
        public float clickDotMovementSpeed = 20;
        [FormerlySerializedAs("imageFadeSpeed")]
        public float clickDotImageFadeSpeed = 1;
        [FormerlySerializedAs("fadeSpeed")]
        public float clickDotFadeSpeed = 10;

        [Header("Colors")]
        [FormerlySerializedAs("normal")]
        public Color textNormalColor = Color.white;
        [FormerlySerializedAs("highlight")]
        public Color textHighlightedColor = Color.gray;
        [Space]
        public Color backgroundNormalColor = Color.white;
        public Color backgroundHighlightedColor = Color.gray;

        [Header("Audio")]
        public AudioClip pointerEnterSFX;
        public AudioClip buttonDownSFX;

        bool isOn;
        AudioSource audioSource;
        Selectable selectable;
        RectTransform clickDotTransform;
        Image background;

        private void Start()
        {
            foreach (TextMeshProUGUI text in texts)
            {
                if (text)
                {
                    text.color = textNormalColor;
                }
            }

            selectable = GetComponent<Selectable>();
            background = GetComponent<Image>();

            if(clickDot)
            clickDotTransform = clickDot.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            isOn = false;

            if (clickDot)
            {
                clickDot.alpha = 0;
                if(clickDotTransform) clickDotTransform.sizeDelta = Vector2.one * clickDotSize;
            }
        }

        private void Update()
        {
            if (selectable && !selectable.interactable) return;

            if (isOn) Hightlight(); else ReturnToNormal();

            if (clickDot)
            {
                clickDotTransform.sizeDelta = Vector2.Lerp(clickDotTransform.sizeDelta, Vector2.one * clickDotSize, Time.unscaledDeltaTime * clickDotMovementSpeed);
            }
            if (clickDot)
            {
                clickDot.alpha = Mathf.Lerp(clickDot.alpha, 0, Time.unscaledDeltaTime * clickDotImageFadeSpeed);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectable && !selectable.interactable) return;

            isOn = true;

            if (pointerEnterSFX)
                audioSource.PlayOneShot(pointerEnterSFX);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (selectable && !selectable.interactable) return;

            isOn = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (selectable && !selectable.interactable) return;

            if (clickDot)
            {
                clickDotTransform.position = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
                clickDotTransform.sizeDelta = Vector2.zero;
            }
            if (clickDot)
            {
                clickDot.alpha = 1;
            }

            if (buttonDownSFX)
                audioSource.PlayOneShot(buttonDownSFX);
        }

        private void Hightlight()
        {
            foreach (TextMeshProUGUI text in texts)
            {
                if (text)
                    text.color = Color.Lerp(text.color, textHighlightedColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
            }

            if(background)
                background.color = Color.Lerp(background.color, backgroundHighlightedColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
        }

        private void ReturnToNormal()
        {
            foreach (TextMeshProUGUI text in texts)
            {
                if (text)
                    text.color = Color.Lerp(text.color, textNormalColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
            }

            if (background)
                background.color = Color.Lerp(background.color, backgroundNormalColor, Time.unscaledDeltaTime * clickDotFadeSpeed);
        }
    }
}