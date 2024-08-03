using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(SprayPattern))]
    public class SprayPatternEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SprayPattern pattern = (SprayPattern)target;

            Undo.RecordObject(pattern, $"Modified {pattern}");
            EditorGUI.BeginChangeCheck();

            pattern.amount = EditorGUILayout.FloatField("Amount", pattern.amount);
            pattern.random = EditorGUILayout.Toggle("Random", pattern.random);

            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(pattern.random);
            pattern.vertical = EditorGUILayout.CurveField("Vertical", pattern.vertical);
            pattern.horizontal = EditorGUILayout.CurveField("Horizontal", pattern.horizontal);
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(pattern);
            }
        }
    }
}