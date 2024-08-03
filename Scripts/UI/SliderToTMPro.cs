using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Inbora.FPSFramework.UI
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Slider To TMPro")]
    [RequireComponent(typeof(Slider)), ExecuteAlways]
    public class SliderToTMPro : MonoBehaviour
    {
        public int decimalNumberAmount = 100;

        private Slider targetSlider;
        private TextMeshProUGUI targetText;

        private void Update()
        {
            if (!targetSlider)
            {
                targetSlider = GetComponent<Slider>();

                if (!targetSlider)
                    Debug.LogWarning("You are tring to use the component 'Slider To TMPro' without attaching the component to game object with another component 'Slider'. Please attach a 'Slider'.");
            }

            if (!targetText)
            {
                targetText = GetComponentInChildren<TextMeshProUGUI>();

                if (!targetText)
                    Debug.LogWarning($"You are tring to use the component 'Slider To TMPro' without having a Text Mesh Pro inside the parent of this Slider To TMPro. Please attach a 'Text Mesh Pro' inside the parent of {gameObject}.");
            }

            if (!targetSlider && !targetText) return;

            float value = targetSlider.value;

            if (!targetSlider.wholeNumbers) { value = Mathf.Round(value * decimalNumberAmount) / decimalNumberAmount; }

            targetText?.SetText(value.ToString());
        }
    }
}
