using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(SettingsPreset), true)]
    public class SettingsPresetEditor : Editor
    {
        private Vector2 scrollPosition;

        public override void OnInspectorGUI()
        {
            DrawSections();
        }

        private void DrawSections()
        {
            SettingsPreset preset = (SettingsPreset)target;
            SettingSection section = null;
            Undo.RecordObject(preset, $"Modified {preset.name}");

            List<string> sectionNames = new List<string>();

            //Add all section names to its list
            foreach (SettingSection settingSection in preset.sections)
            {
                sectionNames.Add(settingSection.name);
            }

            if (preset.sections != null && preset.sections.Count > 0) section = preset.sections[preset.currentSelectedSection];

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(preset.sections.Count <= 0);
            //Remove the currently selected section
            if (GUILayout.Button("-", GUILayout.MaxWidth(23)))
            {
                EditorUtility.SetDirty(preset);

                //Get the index of the selected section
                int index = preset.sections.IndexOf(section);

                //If the selected section is getting removed select the section next to it
                if (index == preset.currentSelectedSection && preset.currentSelectedSection > 0) preset.currentSelectedSection--;

                //Remove the section
                preset?.sections.Remove(section);
            }
            EditorGUI.EndDisabledGroup();

            if (preset.sections.Count > 0)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                //Update selected section
                preset.currentSelectedSection = GUILayout.Toolbar(preset.currentSelectedSection, sectionNames.ToArray());

                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Toolbar(0, new string[] { "None" });
                EditorGUI.EndDisabledGroup();
            }

            //Add new section
            if (GUILayout.Button("+", GUILayout.MaxWidth(23)))
            {
                EditorUtility.SetDirty(preset);

                //Create new section
                SettingSection newSection = new SettingSection("New Section");
                preset?.sections.Add(newSection);

                //Keep selecting section
                preset.currentSelectedSection = preset.sections.IndexOf(newSection);
                preset.currentSelectedSection = Mathf.Clamp(preset.currentSelectedSection, 0, preset.sections.Count);
            }
            EditorGUILayout.EndHorizontal();

            if (preset.sections.Count <= 0)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.HelpBox("There are no sections in this settings preset, click on the + button above to add a new section.", MessageType.Info);
                EditorGUILayout.EndVertical();
            }

            Object prevObj = preset;
            EditorGUI.BeginChangeCheck();
            if (preset.sections.Count > 0)
                DrawOptions();
            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(preset);
            }
        }

        private void DrawOptions()
        {
            SettingsPreset preset = (SettingsPreset)target;
            SettingSection section = null;

            if (preset.sections.Count > 0) section = preset.sections[preset.currentSelectedSection];

            EditorGUILayout.BeginVertical("Box");
            section.name = EditorGUILayout.TextField("Name", section.name);
            EditorGUILayout.EndVertical();

            if (section.options.Count <= 0)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.HelpBox("There are no options in the selected section, click on the + button below to add a new option.", MessageType.Info);
                EditorGUILayout.EndVertical();
            }

            foreach (SettingOption option in section.options)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical("Box");

                option.foldout = EditorGUILayout.Foldout(option.foldout, option.name, true);

                if (option.foldout)
                {
                    option.name = EditorGUILayout.TextField("Name", option.name);

                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    MethodInfo[] methods = preset.GetType().GetMethods(flags);
                    List<string> methodNames = new List<string>();

                    foreach (MethodInfo method in methods)
                    {
                        if (method.Name != "MemberwiseClone"
                            && method.Name != "GetType"
                            && method.Name != "SetDirty"
                            && method.Name != "ToString"
                            && method.Name != "GetType"
                            && method.Name != "Finalize"
                            && method.Name != "GetType"
                            && method.Name != "set_hideFlags"
                            && method.Name != "get_hideFlags"
                            && method.Name != "get_name"
                            && method.Name != "set_name"
                            && method.Name != "Equals"
                            && method.Name != "GetHashCode"
                            && method.Name != "GetInstanceID"
                            && method.Name != "OnAwake"
                            && method.Name != "OnStart"
                            && method.Name != "OnUpdate"
                            && method.Name != "OnApplicationQuit") methodNames.Add(method.Name);

                    }

                    option.selectedFunction = EditorGUILayout.Popup("Function", option.selectedFunction, methodNames.ToArray());
                    option.functionName = methodNames[option.selectedFunction];
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUI.BeginDisabledGroup(section.options.Count <= 0);
            //Remove the currently selected section's selected option
            if (GUILayout.Button("-", GUILayout.MaxWidth(23)))
            {
                EditorUtility.SetDirty(preset);

                section.options.Remove(section.options[section.options.Count - 1]);
            }
            EditorGUI.EndDisabledGroup();



            //Adds a new option to the selected section
            if (GUILayout.Button("+", GUILayout.MaxWidth(23)))
            {
                EditorUtility.SetDirty(preset);

                SettingOption option = new SettingOption();
                section.options.Add(option);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}