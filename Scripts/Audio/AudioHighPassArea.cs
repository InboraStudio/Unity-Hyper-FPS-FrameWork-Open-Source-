using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.AudioManagement
{
    [AddComponentMenu("Inbora/FPS Framework/Audio System/Audio High Pass Area")]
    public class AudioHighPassArea : AudioFilterArea
    {
        public float cutoffFrequency = 10;
        public float time = 1;

        protected override void OnAreaEntered(CharacterManager characterManager, AudioFiltersManager audioFiltersManager)
        {
            audioFiltersManager.SetHightPass(cutoffFrequency, time);
        }

        protected override void OnAreaExited(CharacterManager characterManager, AudioFiltersManager audioFiltersManager)
        {
            audioFiltersManager.ResetHighPass(time);
        }
    }
}