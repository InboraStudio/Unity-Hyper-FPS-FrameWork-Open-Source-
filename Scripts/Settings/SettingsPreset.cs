using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace Inbora.FPSFramework
{
    /// <summary>
    /// Base class for all settings preset.
    /// </summary>
    public class SettingsPreset : ScriptableObject
    {
        public List<SettingSection> sections = new List<SettingSection>(1);

        /// <summary>
        /// This is used only in the custom inspector class which represnts the current selected section from the editor.
        /// </summary>
        public int currentSelectedSection;

        /// <summary>
        /// Called before the first frame is drawn
        /// Called from the assgined settings manager in your scene
        /// </summary>
        public virtual void OnAwake() { }

        /// <summary>
        /// Called agter the first frame is drawn
        /// Called from the assgined settings manager in your scene
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// Called every frame
        /// Called from the assgined settings manager in your scene
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Called after the application has quited or play mode has stoped
        /// Called from the assgined settings manager in your scene
        /// </summary>
        public virtual void OnApplicationQuit() { }

        /// <summary>
        /// Override this function to save your preset
        /// </summary>
        public virtual void Save() { }

        /// <summary>
        /// Override this function to load your preset
        /// </summary>
        public virtual void Load() { }
        
        /// <summary>
        /// A shortcut for Debug.Log("Message Here!");
        /// </summary>
        /// <param name="message"></param>
        public void print(object message) => Debug.Log(message);
    }

    [Serializable]
    public class SettingSection
    {
        public string name;
        public List<SettingOption> options = new List<SettingOption>();

        public SettingOption GetOption(string optionName)
        {
            foreach (SettingOption option in options)
            {
                if (option.name == optionName)
                {
                    return option;
                }
            }

            return null;
        }

        public SettingSection(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class SettingOption
    {
        public string name = "Option";
        public string functionName = "Set";

        public bool foldout;
        public int selectedFunction;
    }
}