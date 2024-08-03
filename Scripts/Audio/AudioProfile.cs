using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine;


namespace Inbora.FPSFramework
{
    [CreateAssetMenu(fileName = "New Audio Profile", menuName = "Inbora/FPS Framework/Audio Profile")]
    public class AudioProfile : ScriptableObject
    {
        public AudioClip clip;
        public AudioMixerGroup output;
        public bool mute;
        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;
        public bool playOnAwake = false;
        public bool loop;

        [Space]
        [Range(0, 256)] public int priority = 128;
        [Range(0, 1)] public float volume = 1;
        [Range(-3, 3)] public float pitch = 1;
        [Range(-1, 1)] public float stereoPan = 0;
        [Range(0, 1)] public float spatialBlend = 1;
        [Range(0, 1.1f)] public float reverbZoneMix = 1;

        [Header("3D Sound Settings")]
        [Range(0, 5)] public float dopplerLevel = 1;
        [Range(0, 360)] public float spread = 0;
        public AudioRolloffMode volumeRolloff = AudioRolloffMode.Custom;
        public float minDistance = 1;
        public float maxDistance = 500;

        [Space]
        public bool dymaicPitch = true;
        public float pitchFactor = 0.2f;

        [Space]
        public List<CustomAudioEvent> customEvents = new List<CustomAudioEvent>();

        public void print(string message)
        {
            Debug.Log(message);
        }

        [System.Serializable]
        public class CustomAudioEvent
        {
            [Range(0.01f, 0.99f)]
            public float location = 0.5f;
            public UnityEvent onLocate = new UnityEvent();

            public CustomAudioEvent() { }
            public CustomAudioEvent(float location) { this.location = location; }
        }
    }
}