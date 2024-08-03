using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Audio System/Audio High Pass Area")]
    [RequireComponent(typeof(AudioListener))]
    public class AudioFiltersManager : MonoBehaviour
    {
        public bool HighPass;
        public bool LowPass;
        public bool Reverb;

        public AudioHighPassFilter HighPassFilter { get; set; }
        public AudioLowPassFilter LowPassFilter { get; set; }
        public AudioReverbFilter ReverbFilter { get; set; }

        [HideInInspector] public float highPassCutoffFrequency;
        [HideInInspector] public float hightPassLerpTime;

        [HideInInspector] public float lowPassCutoffFrequency;
        [HideInInspector] public float lowPassLerpTime;

        public void Start()
        {
            HighPassFilter = HighPass ? gameObject.AddComponent<AudioHighPassFilter>() : null;
            LowPassFilter = LowPass ? gameObject.AddComponent<AudioLowPassFilter>() : null;
            ReverbFilter = Reverb ? gameObject.AddComponent<AudioReverbFilter>() : null;

            if (HighPass) HighPassFilter.cutoffFrequency = 10;
            if (LowPass) LowPassFilter.cutoffFrequency = 22000;
            if (Reverb) ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        }

        private void Update()
        {
            if (HighPass)
            {
                if (HighPassFilter.cutoffFrequency != highPassCutoffFrequency)
                {
                    HighPassFilter.cutoffFrequency = Mathf.Lerp(HighPassFilter.cutoffFrequency, highPassCutoffFrequency, hightPassLerpTime);
                }
            }

            if (LowPass)
            {
                if (LowPassFilter.cutoffFrequency != lowPassCutoffFrequency)
                {
                    LowPassFilter.cutoffFrequency = Mathf.Lerp(LowPassFilter.cutoffFrequency, lowPassCutoffFrequency, lowPassLerpTime);
                }
            }
        }

        public void SetHightPass(float cutoffFrequency, float time)
        {
            highPassCutoffFrequency = cutoffFrequency;
            hightPassLerpTime = time;
        }

        public void ResetHighPass(float time)
        {
            highPassCutoffFrequency = 10;
            hightPassLerpTime = time;
        }

        public void ResetHighPass(float time, float delay)
        {
            StartCoroutine(ResetHighPassAfterDelay(delay, time));
        }

        public void SetLowPass(float cutoffFrequency, float time)
        {
            lowPassCutoffFrequency = cutoffFrequency;
            lowPassLerpTime = time;
        }

        public void ResetLowPass(float time)
        {
            lowPassCutoffFrequency = 22000;
            lowPassLerpTime = time;
        }

        public void ResetLowPass(float time, float delay)
        {
            StartCoroutine(ResetLowPassAfterDelay(delay, time));
        }

        private IEnumerator ResetHighPassAfterDelay(float delay, float time)
        {
            yield return new WaitForSeconds(delay);
            ResetHighPass(time);
        }

        private IEnumerator ResetLowPassAfterDelay(float delay, float time)
        {
            yield return new WaitForSeconds(delay);
            ResetLowPass(time);
        }

        public void SetReverp(AudioReverbPreset preset)
        {
            ReverbFilter.reverbPreset = preset;
        }

        public void ResetReverp()
        {
            ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        }
    }
}