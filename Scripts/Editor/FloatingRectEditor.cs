
using UnityEngine;
using UnityEditor;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(FloatingRect))]
    public class FloatingRectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FloatingRect rect = (FloatingRect)target;

            Undo.RecordObject(rect, $"Modified Float Rect {rect}");
            EditorGUI.BeginChangeCheck();

            rect.targetType = (FloatingRect.TargetType)EditorGUILayout.EnumPopup(new GUIContent("Targeting Type", "Does the rect follow a transform or position"), rect.targetType);

            if (rect.targetType == FloatingRect.TargetType.Transform)
            {
                rect.target = EditorGUILayout.ObjectField(new GUIContent("Target", "Target transform."), rect.target, typeof(Transform), true) as Transform;
            }

            if (rect.targetType == FloatingRect.TargetType.Position)
            {
                rect.position = EditorGUILayout.Vector3Field(new GUIContent("Target Position"), rect.position);
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(rect);
            }
        }
    }
}