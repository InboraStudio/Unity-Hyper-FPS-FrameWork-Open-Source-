using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inbora.FPSFramework.UI
{
    [AddComponentMenu("Inbora/FPS Framework/UI/FPS Counter")]
    public class FPSCounter : MonoBehaviour
    {
        public float updateInterval = 10;
        public Color labelColor = Color.gray;
        public Color countColor = Color.white;

        private float framerate;

        private TextMeshProUGUI text;
        const float FPSMeasurePeriod = 0.5f;
        private int FPSAccumulator = 0;
        private float FPSPeriod = 0;
        private float nextUpdate;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            FPSPeriod = Time.realtimeSinceStartup + FPSMeasurePeriod;
        }

        private void Update()
        {
            FPSAccumulator++;
            if (Time.realtimeSinceStartup > FPSPeriod)
            {
                framerate = (int)(FPSAccumulator / FPSMeasurePeriod);
                FPSAccumulator = 0;
                FPSPeriod += FPSMeasurePeriod;
            }

            if (Time.time >= nextUpdate)
            {
                nextUpdate = Time.time + 1 / updateInterval;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(labelColor)}>FPS: <color=#{ColorUtility.ToHtmlStringRGB(countColor)}>{framerate}";
        }
    }
}