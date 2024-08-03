using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(CameraManager))]
    public class CameraManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CameraManager manager = (CameraManager)target;
            Undo.RecordObject(manager, $"Modified Camera Manager {manager}");
            EditorGUI.BeginChangeCheck();


            manager.mainCamera = EditorGUILayout.ObjectField(new GUIContent(" Main Camera", "the cam which is responsable for rendering the world."), manager.mainCamera, typeof(Camera), true) as Camera;

            if (manager.mainCamera)
            {
                manager.overlayCamera = EditorGUILayout.ObjectField(new GUIContent(" Overlay Camera", "the cam which is responsable for rendering anything in player is hand."), manager.overlayCamera, typeof(Camera), true) as Camera;
            }



            UpdateCameraRecoil();
            UpdateCameraShake();
            UpdateFOVKick();
            UpdateLean();
            UpdateHeadbob();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(manager);
            }
        }

        private void UpdateFOVKick()
        {
            CameraManager manager = (CameraManager)target;

            EditorGUILayout.BeginHorizontal("box");
            manager.Use_FOVKick = EditorGUILayout.Toggle(manager.Use_FOVKick, GUILayout.MaxWidth(28));
            manager.Foldout_FOVKick = EditorGUILayout.Foldout(manager.Foldout_FOVKick, "FOV Kick", true);
            EditorGUILayout.EndHorizontal();

            if (manager.Foldout_FOVKick)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUI.BeginDisabledGroup(!manager.Use_FOVKick);

                if (manager.mainCamera)
                {
                    manager.FOVKick = EditorGUILayout.FloatField(new GUIContent(" Amount", "the amount of fov added to main camera while in high speed"), manager.FOVKick);

                    if (manager.overlayCamera)
                        manager.overlayFOVKick = EditorGUILayout.FloatField(new GUIContent(" Overlay Amount", "the amount of fov added to overlay while in high speed"), manager.overlayFOVKick);
                    else
                        EditorGUILayout.HelpBox(" Overlay Camera is null to use overlay camera FOV Kick please assign overlay camera.", MessageType.Info);

                    manager.FOVKickSmoothness = EditorGUILayout.FloatField(new GUIContent(" Roughness", "How rough fov kick is"), manager.FOVKickSmoothness);
                }
                else if (manager.Use_FOVKick)
                {
                    EditorGUILayout.HelpBox("Main Camera can't be null.", MessageType.Warning);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndVertical();
            }
        }

        private void UpdateLean()
        {
            CameraManager manager = (CameraManager)target;

            EditorGUILayout.BeginHorizontal("box");
            manager.Use_Lean = EditorGUILayout.Toggle(manager.Use_Lean, GUILayout.MaxWidth(28));
            manager.Foldout_Lean = EditorGUILayout.Foldout(manager.Foldout_Lean, "Lean", true);
            EditorGUILayout.EndHorizontal();

            if (manager.Foldout_Lean)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUI.BeginDisabledGroup(!manager.Use_Lean);


                manager.rotationAngle = EditorGUILayout.FloatField(new GUIContent(" Angle", "angle of leaning"), manager.rotationAngle);
                manager.offset = EditorGUILayout.FloatField(new GUIContent(" Offset", "offset of leaning on the right and left"), manager.offset);
                manager.smoothness = EditorGUILayout.FloatField(new GUIContent(" Smoothness", "the speed of leaning"), manager.smoothness);

                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndVertical();
            }
        }

        private void UpdateHeadbob()
        {
            CameraManager manager = (CameraManager)target;

            EditorGUILayout.BeginHorizontal("box");
            manager.Use_Headbob = EditorGUILayout.Toggle(manager.Use_Headbob, GUILayout.MaxWidth(28));
            manager.Foldout_Headbob = EditorGUILayout.Foldout(manager.Foldout_Headbob, "Head Bob", true);
            EditorGUILayout.EndHorizontal();

            if (manager.Foldout_Headbob)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUI.BeginDisabledGroup(!manager.Use_Headbob);

                manager.headbobAmount = EditorGUILayout.FloatField(new GUIContent(" Amount", "amount of movement while moving"), manager.headbobAmount);
                manager.headbobRotationAmount = EditorGUILayout.FloatField(new GUIContent(" Rotation Amount", "amount of rotation movement while moving"), manager.headbobRotationAmount);


                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndVertical();
            }
        }

        private void UpdateCameraShake()
        {
            CameraManager manager = (CameraManager)target;

            EditorGUILayout.BeginHorizontal("box");
            manager.Use_CameraShake = EditorGUILayout.Toggle(manager.Use_CameraShake, GUILayout.MaxWidth(28));
            manager.Foldout_CameraShake = EditorGUILayout.Foldout(manager.Foldout_CameraShake, "Camera Shake", true);
            EditorGUILayout.EndHorizontal();

            if (manager.Foldout_CameraShake)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUI.BeginDisabledGroup(!manager.Use_CameraShake);

                manager.mainCameraShaker = EditorGUILayout.ObjectField(new GUIContent(" Shaker", "camera shaker which is responsable for shaking the camera."), manager.mainCameraShaker, typeof(CameraShaker), true) as CameraShaker;

                if (manager.mainCameraShaker)
                {
                    manager.mainCameraShakeMagnitude = EditorGUILayout.FloatField(new GUIContent(" Magnitude", "the amount of shake."), manager.mainCameraShakeMagnitude);
                    manager.cameraShakeRoughness = EditorGUILayout.FloatField(new GUIContent(" Roughness", "how hard the lerp is while shaking."), manager.cameraShakeRoughness);

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(" Fade", EditorStyles.boldLabel);
                    manager.cameraShakeFadeInTime = EditorGUILayout.FloatField(new GUIContent(" In", "the speed of leaning"), manager.cameraShakeFadeInTime);
                    manager.cameraShakeFadeOutTime = EditorGUILayout.FloatField(new GUIContent(" Out", "the speed of leaning"), manager.cameraShakeFadeOutTime);

                }
                else if (manager.Use_CameraShake)
                {
                    EditorGUILayout.HelpBox("Shaker can't be null.", MessageType.Warning);
                }

                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndVertical();
            }
        }

        private void UpdateCameraRecoil()
        {
            CameraManager manager = (CameraManager)target;

            EditorGUILayout.BeginHorizontal("box");
            manager.Use_CameraRecoil = EditorGUILayout.Toggle(manager.Use_CameraRecoil, GUILayout.MaxWidth(28));
            manager.Foldout_CameraRecoil = EditorGUILayout.Foldout(manager.Foldout_CameraRecoil, "Camera Recoil", true);
            EditorGUILayout.EndHorizontal();

            if (manager.Foldout_CameraRecoil)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUI.BeginDisabledGroup(!manager.Use_CameraRecoil);

                manager.RecoilDampTime = EditorGUILayout.FloatField(new GUIContent(" Roughness", "how hard the lerp is while shaking."), manager.RecoilDampTime);
                manager.RecoilAmount = EditorGUILayout.Vector3Field(new GUIContent(" Amount", "the amount of rotation applied when recoil applied."), manager.RecoilAmount);


                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndVertical();
            }
        }
    }
}