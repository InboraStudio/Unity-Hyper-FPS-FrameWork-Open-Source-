using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Inbora.FPSFramework.AudioManagement
{
    [AddComponentMenu("Inbora/FPS Framework/Audio System/Audio Reverb Area")]
    public class AudioReverbArea : AudioFilterArea
    {
        public AudioReverbPreset preset;

        protected override void OnAreaEntered(CharacterManager characterManager, AudioFiltersManager audioFiltersManager)
        {
            audioFiltersManager = characterManager.audioFiltersManager;
            audioFiltersManager.SetReverp(preset);
        }

        protected override void OnAreaExited(CharacterManager characterManager, AudioFiltersManager audioFiltersManager)
        {
            audioFiltersManager.ResetReverp();
        }
    }
}