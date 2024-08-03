using UnityEngine;

namespace Inbora.FPSFramework
{
    [System.Serializable]
    public class CameraShakeProfile
    {
        public float Magnitude = 1;
        public float Roughness = 1;
        public Vector3 Position = new Vector3(0.01f, 0.01f, 0.01f);
        public Vector3 Rotation = new Vector3(10, 10, 10);
        public bool EndOnInactive = true;

        private float fadeOutDuration;
        private float fadeInDuration;
        private bool sustain;
        private float fadeTime;
        private float tick = 0;
        private Vector3 noise;

        public CameraShakeProfile(float magnitude, float roughness)
        {
            Magnitude = magnitude;
            Roughness = roughness;
            sustain = true;

            tick = Random.Range(-100, 100);
        }

        public CameraShakeProfile(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
        {
            Magnitude = magnitude;
            Roughness = roughness;
            fadeInDuration = fadeInTime;
            fadeOutDuration = fadeOutTime;

            if (fadeInTime > 0)
            {
                sustain = true;
                fadeTime = 0;
            }
            else
            {
                sustain = false;
                fadeTime = 1;
            }

            tick = Random.Range(-100, 100);
        }

        /// <summary>
        /// returns current shake noise
        /// </summary>
        /// <returns></returns>
        public Vector3 Result()
        {
            noise.x = Mathf.PerlinNoise(tick, 0) - 0.5f;
            noise.y = Mathf.PerlinNoise(0, tick) - 0.5f;
            noise.z = Mathf.PerlinNoise(tick, tick) - 0.5f;

            if (fadeInDuration > 0 && sustain)
            {
                if (fadeTime < 1)
                    fadeTime += Time.deltaTime / fadeInDuration;
                else if (fadeOutDuration > 0)
                    sustain = false;
            }

            if (!sustain)
                fadeTime -= Time.deltaTime / fadeOutDuration;

            if (sustain)
                tick += Time.deltaTime * Roughness;
            else
                tick += Time.deltaTime * Roughness * fadeTime;

            return noise * Magnitude * fadeTime;
        }

        /// <summary>
        /// starts to fade in
        /// </summary>
        /// <param name="fadeInTime">fade in duration</param>
        public void BeginFade(float fadeInTime)
        {
            if (fadeInTime == 0)
                fadeTime = 1;

            fadeInDuration = fadeInTime;
            fadeOutDuration = 0;
            sustain = true;
        }

        /// <summary>
        /// current state
        /// </summary>
        public CameraShakeState State
        {
            get
            {
                if (fadeTime < 1 && sustain && fadeInDuration > 0)
                    return CameraShakeState.FadingIn;

                else if (!sustain && fadeTime > 0)
                    return CameraShakeState.FadingOut;

                else if (fadeTime > 0 || sustain)
                    return CameraShakeState.Sustained;

                else
                    return CameraShakeState.Inactive;
            }
        }
    }

    public enum CameraShakeState
    {
        FadingIn,
        FadingOut,
        Sustained,
        Inactive
    }
}