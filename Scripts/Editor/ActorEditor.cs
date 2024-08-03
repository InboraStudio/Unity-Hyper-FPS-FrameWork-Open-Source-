using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Actor actor = (Actor)target;

            if (actor.GetComponentInParent<ActorManager>()) return;

            EditorGUILayout.HelpBox("Ignore if in prefab mode. Actor must have an actor manager on it's parent in order to work if needed to respwan", MessageType.Warning);
        }
    }
}