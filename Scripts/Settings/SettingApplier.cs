using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;
using System;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Settings Menu/Setting Applier")]
    public class SettingApplier : MonoBehaviour
    {
        public string path = "Section/Option";
        public int selectedPathIndex;

        public SettingsManager settingsManager { get; set; }

        private void Start()
        {
            settingsManager = FindObjectOfType<SettingsManager>();
        }

        public void Apply(float value)
        {
            Apply(value, 0, false, 0);
        }

        public void Apply(int value)
        {
            Apply(0, value, false, 1);
        }

        public void Apply(bool value)
        {
            Apply(0, 0, value, 2);
        }

        private void Apply(float floatValue = 0, int intValue = 0, bool boolValue = false, float type = 0)
        {
            if (!settingsManager) return;

            string functionName = GetOption().functionName;
            MethodInfo method = settingsManager.settingsPreset.GetType().GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null)
            {
                object[] parameters = null;

                if (type == 0) parameters = new object[] { (float)floatValue };
                if (type == 1) parameters = new object[] { (int)intValue };
                if (type == 2) parameters = new object[] { (bool)boolValue };

                method.Invoke(settingsManager.settingsPreset, parameters);

            }
            else
            {
                Debug.LogError($"Function '{functionName}' not found in preset.");
            }
        }

        public SettingOption GetOption()
        {
            if (!settingsManager) return null;

            string[] pathParts = path.Split('/');

            if (pathParts.Length < 2)
            {
                Debug.LogError("Invalid path format. Please use 'Section/Option' format.");
                return null;
            }

            string sectionName = pathParts[0];
            string optionName = pathParts[1];

            SettingSection section = settingsManager.GetSection(sectionName);

            if (section == null)
            {
                Debug.LogError("Section '" + sectionName + "' not found.");
                return null;
            }

            SettingOption option = section.GetOption(optionName);

            if (option == null)
            {
                Debug.LogError("Option '" + optionName + "' not found in section '" + sectionName + "'.");
                return null;
            }

            return option;
        }
    }
}