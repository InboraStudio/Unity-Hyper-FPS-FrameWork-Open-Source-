using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(Explosive))]
    public class ExplosiveEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            Explosive explosive = (Explosive)target;
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(explosive, "Modified explosive");

            EditorGUILayout.LabelField("Base", EditorStyles.boldLabel);

            explosive.type = (ExplosionType)EditorGUILayout.EnumPopup("Type", explosive.type);
            LayerMask layerMask = EditorGUILayout.MaskField("Layer Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(explosive.layerMask), InternalEditorUtility.layers);
            explosive.layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(layerMask);
            explosive.radius = EditorGUILayout.FloatField("Radius", explosive.radius);
            explosive.effectRadius = EditorGUILayout.FloatField("Effect Radius", explosive.effectRadius);
            explosive.damage = EditorGUILayout.FloatField("Damage", explosive.damage);
            explosive.force = EditorGUILayout.FloatField("Force", explosive.force);
            explosive.delay = EditorGUILayout.FloatField("Delay", explosive.delay);
            explosive.friction = EditorGUILayout.FloatField("Friction", explosive.friction);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            explosive.ignoreGlobalScale = GUILayout.Toggle(explosive.ignoreGlobalScale, "Ignore Global Scale");
            explosive.sticky = GUILayout.Toggle(explosive.sticky, "Sticky");
            explosive.damageable = GUILayout.Toggle(explosive.damageable, "Damageable");

            if (explosive.damageable)
            {
                EditorGUI.indentLevel++;
                explosive.health = EditorGUILayout.FloatField("Health", explosive.health);
                EditorGUI.indentLevel--;
            }

            explosive.exlopeAfterDelay = GUILayout.Toggle(explosive.exlopeAfterDelay, "Exlope After Delay");
            explosive.destroyOnExplode = GUILayout.Toggle(explosive.destroyOnExplode, "Destroy On Explode");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("VFX", EditorStyles.boldLabel);
            explosive.explosion = EditorGUILayout.ObjectField("Explosion", explosive.explosion, typeof(GameObject), true) as GameObject;
            explosive.craterDecal = EditorGUILayout.ObjectField("Crater Decal", explosive.craterDecal, typeof(GameObject), true) as GameObject;
            explosive.clearDelay = EditorGUILayout.FloatField("Clear Delay", explosive.clearDelay);
            explosive.explosionEffactOffcet = EditorGUILayout.Vector3Field("Explosion Offcet", explosive.explosionEffactOffcet);
            explosive.explosionEffactRotationOffset = EditorGUILayout.Vector3Field("Explosion Rotation Offcet", explosive.explosionEffactRotationOffset);
            EditorGUILayout.Space();

            explosive.explosionSize = EditorGUILayout.FloatField("Explosion Size", explosive.explosionSize);
            explosive.craterSize = EditorGUILayout.FloatField("Crater Size", explosive.craterSize);
            explosive.cameraShake = EditorGUILayout.FloatField("Camera Shake", explosive.cameraShake);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
            explosive.audioLowPassFilter = GUILayout.Toggle(explosive.audioLowPassFilter, "Audio Low Pass Filter");

            if (explosive.audioLowPassFilter)
            {
                explosive.lowPassCutoffFrequency = EditorGUILayout.FloatField("Cutoff Frequency", explosive.lowPassCutoffFrequency);
                explosive.lowPassSmoothness = EditorGUILayout.FloatField("Smoothness", explosive.lowPassSmoothness);
                explosive.lowPassTime = EditorGUILayout.FloatField("Time", explosive.lowPassTime);
            }

            EditorGUILayout.Space();
            if (explosive.debug) EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            explosive.debug = EditorGUILayout.Toggle(explosive.debug, GUILayout.MaxWidth(15));
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            if (explosive.debug)
            {
                EditorGUILayout.BeginHorizontal();
                explosive.ranges = GUILayout.Toggle(explosive.ranges, "Ranges", "Button");
                explosive.rays = GUILayout.Toggle(explosive.rays, "Rays", "Button");
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(explosive);
        }
    }
}