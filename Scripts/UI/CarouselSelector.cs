using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Inbora.FPSFramework.UI
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Carousel Selector")]
    public class CarouselSelector : MonoBehaviour
    {
        [Header("Input")]
        public Button rightButton;
        public Button leftButton;
        public TextMeshProUGUI label;
        
        [Space]
        public List<string> options = new List<string>() { "Option A"};
        public int value = 0;

        [Space]
        public UnityEvent<int> onValueChanged = new UnityEvent<int>();

        private void Start()
        {
            UpdateGraphics(value);

            rightButton?.onClick.AddListener(GoRight);
            leftButton?.onClick.AddListener(GoLeft);
        }

        private void Update()
        {
            UpdateGraphics(value);
        }

        public void GoRight()
        {
            int nextValue = value + 1;
            if (nextValue > options.Count - 1) value = 0;
            else value = nextValue;

            onValueChanged?.Invoke(value);
            UpdateGraphics(value);
        }

        public void GoLeft()
        {
            int previousValue = value - 1;
            if (previousValue < 0) value = options.Count - 1;
            else value = previousValue;

            onValueChanged?.Invoke(value);
            UpdateGraphics(value);
        }

        public void UpdateGraphics(int value)
        {
            if (value > options.Count - 1 || value < 0) return;
            label.text = options[value];
        }

        public void AddOption(string option)
        {
            options.Add(option);
        }

        public void AddOptions(string[] options)
        {
            this.options.AddRange(options);
        }

        public void ClearOptions()
        {
            options.Clear();
            options = new List<string>();
        }
    }
}
