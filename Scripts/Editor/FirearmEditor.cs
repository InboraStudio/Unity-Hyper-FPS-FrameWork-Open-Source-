using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(Firearm))]
    public class FirearmEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            Firearm firearm = (Firearm)target;
            if (!firearm.GetComponent<ItemInput>())
            {
                EditorGUILayout.HelpBox("Firearm must have Firearm Input Manager in order to get input.", MessageType.Info);
            }
        }
    }
}