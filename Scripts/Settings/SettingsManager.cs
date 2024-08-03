using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Settings Menu/Settings Manager")]
    public class SettingsManager : MonoBehaviour
    {
        public SettingsPreset settingsPreset;

        public SettingSection GetSection(string sectionName)
        {
            foreach (SettingSection section in settingsPreset.sections)
            {
                if (section.name == sectionName)
                {
                    return section;
                }
            }

            return null;
        }

        private void Awake()
        {
            settingsPreset?.OnAwake();
        }

        private void Start()
        {
            settingsPreset?.OnStart();
            settingsPreset?.Load();
        }

        private void Update()
        {
            settingsPreset?.OnUpdate();
        }

        private void OnDisable()
        {
            settingsPreset?.Save();
        }

        private void OnDestroy()
        {
            settingsPreset?.Save();
        }

        private void OnApplicationQuit()
        {
            settingsPreset?.OnApplicationQuit();
            settingsPreset?.Save();
        }
    }
}