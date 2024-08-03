using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.AudioManagement
{
    [AddComponentMenu("Inbora/FPS Framework/Audio System/Audio Low Pass Area")]
    public class AudioLowPassArea : AudioFilterArea
    {
        public float cutoffFrequency = 5000;
        public float time = 1;

        protected override void OnAreaEntered(CharacterManager characterManager, AudioFiltersManager audioFiltersManager)
        {
            audioFiltersManager.SetLowPass(cutoffFrequency, time);
        }

        protected override void OnAreaExited(CharacterManager characterManager, AudioFiltersManager audioFiltersManager)
        {
            audioFiltersManager.ResetLowPass(time);
        }
    }
}