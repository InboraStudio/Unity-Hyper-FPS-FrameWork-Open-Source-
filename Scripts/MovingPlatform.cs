using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/MovingPlatform")]
    public class MovingPlatform : MonoBehaviour
    {
        public float distance;
        public float speed;
        [HideInInspector] public bool x, y, z;

        private Vector3 position;
        private Vector3 defaultPosition;
        private ShootingSheet shootingSheet;

        private void Start()
        {
            defaultPosition = transform.position;
            shootingSheet = GetComponent<ShootingSheet>() ? GetComponent<ShootingSheet>() : GetComponentInChildren<ShootingSheet>();
        }

        private void Update()
        {
            if (shootingSheet && shootingSheet.downed) return;

            if (x) position.x = ((distance) / 2f * Mathf.Sin(Time.time * speed));
            if (y) position.y = ((distance) / 2f * Mathf.Sin(Time.time * speed));
            if (z) position.z = ((distance) / 2f * Mathf.Sin(Time.time * speed));

            Vector3 p = defaultPosition + position;

            transform.position = p;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatformEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();

            MovingPlatform platform = (MovingPlatform)target;

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            platform.x = GUILayout.Toggle(platform.x, "X", "Button", GUILayout.MaxHeight(24));
            platform.y = GUILayout.Toggle(platform.y, "Y", "Button", GUILayout.MaxHeight(24));
            platform.z = GUILayout.Toggle(platform.z, "Z", "Button", GUILayout.MaxHeight(24));

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject((MovingPlatform)target, "Changed Moving Platform");
            }
        }
    }
#endif
}