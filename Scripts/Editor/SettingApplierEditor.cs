using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(SettingApplier))]
    public class SettingApplierEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SettingApplier settingApplier = (SettingApplier)target;
            SettingsManager settingsManager = FindObjectOfType<SettingsManager>();

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(settingApplier, "Chnaged setting applier");

            if (!settingsManager)
            {
                EditorGUILayout.HelpBox("Couldn't find any game object with the component 'Settings Manager. Please add the component anywhere in your scene to show this component's properties.", MessageType.Warning);
                return;
            }

            SettingsPreset preset = settingsManager.settingsPreset;

            if (!preset)
            {
                EditorGUILayout.HelpBox("Couldn't find a preset assigned to the field 'Settings Preset' in the component 'Settings Manager. Please assign any preset into your settings manager component to show this component's properties.", MessageType.Warning);
                return;
            }
            List<string> options = new List<string>();

            foreach (SettingSection section in preset.sections)
            {
                string s = section.name;

                foreach (SettingOption option in section.options)
                {
                    string o = option.name;
                    options.Add($"{s}/{o}");
                }
            }

            settingApplier.selectedPathIndex = EditorGUILayout.Popup("Path", settingApplier.selectedPathIndex, options.ToArray());
            settingApplier.path = options[settingApplier.selectedPathIndex];

            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(settingApplier);
            }
        }
    }
}