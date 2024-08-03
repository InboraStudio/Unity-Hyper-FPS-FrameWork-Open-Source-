using Inbora.FPSFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Inbora.FPSFramework
{
    [CustomEditor(typeof(FirearmData))]
    public class FirearmDataEditor : Editor
    {
        private string fireRateUnitLabel;
        public static bool Foldout_Fire;
        public static bool Foldout_Recoil;
        public static bool Foldout_Movement;
        public static bool Foldout_Audio;

        public override void OnInspectorGUI()
        {
            FirearmData weapon = (FirearmData)target;

            Undo.RecordObject(weapon, $"Modified {weapon}");
            EditorGUI.BeginChangeCheck();

            UpdateBase(weapon);
            EditorGUILayout.Space();

            UpdateFire(weapon);
            UpdateRecoil(weapon);
            UpdateMovement(weapon);
            UpdateAudio(weapon);

            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(weapon);
        }

        private void UpdateBase(FirearmData weapon)
        {
            EditorGUILayout.LabelField("Base", EditorStyles.boldLabel);

            weapon.Name = EditorGUILayout.TextField(new GUIContent("Name"), weapon.Name);
            weapon.replacement = EditorGUILayout.ObjectField(new GUIContent("Replacement"), weapon.replacement, typeof(Pickable), true) as Pickable;
            weapon.canves = EditorGUILayout.ObjectField(new GUIContent("Canves"), weapon.canves, typeof(WeaponHUD), true) as WeaponHUD;
            weapon.crosshair = EditorGUILayout.ObjectField(new GUIContent("Crosshair"), weapon.crosshair, typeof(Crosshair), true) as Crosshair;
        }

        private void UpdateFire(FirearmData weapon)
        {
            EditorGUILayout.BeginHorizontal("box");
            weapon.useFire = EditorGUILayout.Toggle(weapon.useFire, GUILayout.MaxWidth(28));
            Foldout_Fire = EditorGUILayout.Foldout(Foldout_Fire, "Fire", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Fire) return;
            EditorGUILayout.BeginVertical("box");

            EditorGUI.BeginDisabledGroup(!weapon.useFire);

            weapon.shootingMechanism = (Weapon.ShootingMechanism)EditorGUILayout.EnumPopup(new GUIContent(" Shooting Mechanism"), weapon.shootingMechanism);
            weapon.fireMode = (Weapon.FireMode)EditorGUILayout.EnumPopup(new GUIContent(" Mode"), weapon.fireMode);
            weapon.rechargingType = (Weapon.RechargingType)EditorGUILayout.EnumPopup(new GUIContent(" Bolt Type"), weapon.rechargingType);
            weapon.fireRateUnit = (Weapon.FireRateUnit)EditorGUILayout.EnumPopup(new GUIContent(" Fire Rate Unit"), weapon.fireRateUnit);
            weapon.casingDirection = (Vector3Direction)EditorGUILayout.EnumPopup(new GUIContent(" Casing Direction"), weapon.casingDirection);

            if(weapon.shootingMechanism == Weapon.ShootingMechanism.Projectiles)
            weapon.projectile = EditorGUILayout.ObjectField(new GUIContent(" Projectile"), weapon.projectile, typeof(Projectile), false) as Projectile;

            if (!weapon.projectile && weapon.shootingMechanism == Weapon.ShootingMechanism.Projectiles)
            {
                EditorGUILayout.HelpBox("Projectile must be assgined in order to use this section.", MessageType.Error);
                EditorGUILayout.Space();
            }

            if (weapon.projectile && weapon.shootingMechanism == Weapon.ShootingMechanism.Projectiles || weapon.shootingMechanism == Weapon.ShootingMechanism.Hitscan)
            {
                weapon.casing = EditorGUILayout.ObjectField(new GUIContent(" Casing"), weapon.casing, typeof(GameObject), false) as GameObject;

                EditorGUILayout.BeginHorizontal();
                weapon.fireRate = EditorGUILayout.FloatField(new GUIContent(" Fire Rate"), weapon.fireRate);

                if (weapon.fireRateUnit == Weapon.FireRateUnit.RoundPerMinute)
                    fireRateUnitLabel = "RPM";
                if (weapon.fireRateUnit == Weapon.FireRateUnit.RoundPerSecond)
                    fireRateUnitLabel = "RPS";

                EditorGUILayout.LabelField(fireRateUnitLabel, GUILayout.MaxWidth(33));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                weapon.muzzleVelocity = EditorGUILayout.FloatField(new GUIContent(" Muzzle Velocity"), weapon.muzzleVelocity);
                EditorGUILayout.LabelField("M/S", GUILayout.MaxWidth(33));
                EditorGUILayout.EndHorizontal();

                if (weapon.casing != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    weapon.ejectionVelocity = EditorGUILayout.FloatField(new GUIContent(" Casing Ejection Velocity"), weapon.ejectionVelocity);
                    EditorGUILayout.LabelField("M/S", GUILayout.MaxWidth(33));
                    EditorGUILayout.EndHorizontal();
                }

                if (weapon.casing != null)
                {
                    weapon.casingRotationFactor = EditorGUILayout.FloatField(new GUIContent(" Casing Rotation"), weapon.casingRotationFactor);
                }

                weapon.impactForce = EditorGUILayout.FloatField(new GUIContent(" Impact Force"), weapon.impactForce);

                weapon.damage = EditorGUILayout.FloatField(new GUIContent(" Damage"), weapon.damage);

                if (weapon.shootingMechanism == Weapon.ShootingMechanism.Hitscan)
                {
                    weapon.range = EditorGUILayout.FloatField(new GUIContent(" Range"), weapon.range);
                    weapon.defaultDecal = EditorGUILayout.ObjectField(" Default Decal", weapon.defaultDecal, typeof(GameObject), true) as GameObject;
                    weapon.decalDirection = (Vector3Direction)EditorGUILayout.EnumPopup(new GUIContent(" Decal Direction"), weapon.decalDirection);
                }

                weapon.damageRangeCurve = EditorGUILayout.CurveField(" Damage Range Curve", weapon.damageRangeCurve);
                weapon.sprayPattern = EditorGUILayout.ObjectField(" Spray Pattern", weapon.sprayPattern, typeof(SprayPattern), true) as SprayPattern;
                weapon.aimSprayPattern = EditorGUILayout.ObjectField(" Aim Spray Pattern", weapon.aimSprayPattern, typeof(SprayPattern), true) as SprayPattern;
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField(" Gamepad Settings", EditorStyles.boldLabel);
                weapon.gamepadVibrationAmountRight = EditorGUILayout.FloatField(new GUIContent(" Vibration Amount Right"), weapon.gamepadVibrationAmountRight);
                weapon.gamepadVibrationAmountLeft = EditorGUILayout.FloatField(new GUIContent(" Vibration Amount Left"), weapon.gamepadVibrationAmountLeft);
                weapon.gamepadVibrationDuration = EditorGUILayout.FloatField(new GUIContent(" Vibration Duration"), weapon.gamepadVibrationDuration);


                EditorGUILayout.Space();
                EditorGUILayout.LabelField(" Other Settings", EditorStyles.boldLabel);
                weapon.fireTransition = EditorGUILayout.FloatField(new GUIContent(" Fire Transition"), weapon.fireTransition);

                if (weapon.rechargingType == Weapon.RechargingType.Manual)
                {
                    weapon.rechargingStateName = EditorGUILayout.TextField(new GUIContent(" Recharging State Name"), weapon.rechargingStateName);
                    weapon.allowFireWhileRecharging = EditorGUILayout.Toggle(new GUIContent(" Allow Fire While Recharging"), weapon.allowFireWhileRecharging);
                }

                weapon.tracerRounds = EditorGUILayout.Toggle(new GUIContent(" Tracer Rounds"), weapon.tracerRounds);
                weapon.projectileSize = EditorGUILayout.FloatField(new GUIContent(" Projectile Size"), weapon.projectileSize);
                weapon.decalSize = EditorGUILayout.FloatField(new GUIContent(" Decal Size"), weapon.decalSize);
                weapon.afterShotReliefTime = EditorGUILayout.FloatField(new GUIContent(" After Shot Relief Time"), weapon.afterShotReliefTime);
                weapon.shotCount = EditorGUILayout.IntField(new GUIContent(" Shot Count"), weapon.shotCount);

                if (weapon.shotCount > 1)
                {

                    EditorGUILayout.BeginHorizontal();
                    weapon.shotDelay = EditorGUILayout.FloatField(new GUIContent(" Shots Delay"), weapon.shotDelay);

                    if (GUILayout.Button(new GUIContent(" Calculate", "Calculates shot delay from fire rate")))
                    {
                        if (weapon.fireRateUnit == Weapon.FireRateUnit.RoundPerMinute)
                        {
                            weapon.shotDelay = 1 / (weapon.fireRate / 60);
                        }

                        if (weapon.fireRateUnit == Weapon.FireRateUnit.RoundPerSecond)
                        {
                            weapon.shotDelay = 1 / weapon.fireRate;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    weapon.alwaysApplyFire = EditorGUILayout.Toggle(new GUIContent(" Always Apply Fire"), weapon.alwaysApplyFire);
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField(" Ammunition", EditorStyles.boldLabel);

                weapon.ammoType = EditorGUILayout.ObjectField("Ammo Profile", weapon.ammoType, typeof(AmmoProfileData), true) as AmmoProfileData;

                weapon.magazineCapacity = EditorGUILayout.IntField(new GUIContent(" Magazine Capacity"), weapon.magazineCapacity);
                weapon.reserve = EditorGUILayout.IntField(new GUIContent(" Reserve"), weapon.reserve);

                weapon.automaticReload = EditorGUILayout.Toggle(" Automatic Reload", weapon.automaticReload);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField(" Reload", EditorStyles.boldLabel);

                weapon.reloadMethod = (Weapon.ReloadType)EditorGUILayout.EnumPopup(new GUIContent(" Reload Method"), weapon.reloadMethod);
                weapon.reloadTime = EditorGUILayout.FloatField(new GUIContent(" Reload Time"), weapon.reloadTime);

                if(weapon.reloadMethod == Weapon.ReloadType.Scripted)
                {
                    weapon.reloadStateName = EditorGUILayout.TextField(" Reload Animation State Name", weapon.reloadStateName);
                    weapon.reloadTransitionTime = EditorGUILayout.FloatField(" Reload Animation Transition Time", weapon.reloadTransitionTime);
                    EditorGUILayout.HelpBox("The scripted reloads are the type of reloads used in guns like shotguns and revolvers. In this type of reload, the firearm is only responsible for setting the reloading state and playing the reload animation. Reloading itself is done by throwing an event in the reload animation, calling the method \"Reload\" from the class \"WeaponeEvents.cs.\" This class must be attached to the game object that has the animator on it.", MessageType.Info);
                }

                if (weapon.reloadMethod == Weapon.ReloadType.Default)
                {

                    weapon.emptyReloadTime = EditorGUILayout.FloatField(new GUIContent(" Empty Reload Time"), weapon.emptyReloadTime);
                }
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private void UpdateRecoil(FirearmData weapon)
        {
            EditorGUILayout.BeginHorizontal("box");
            weapon.useRecoil = EditorGUILayout.Toggle(weapon.useRecoil, GUILayout.MaxWidth(28));
            Foldout_Recoil = EditorGUILayout.Foldout(Foldout_Recoil, "Recoil", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Recoil) return;
            EditorGUILayout.BeginVertical("box");

            EditorGUI.BeginDisabledGroup(!weapon.useRecoil);

            EditorGUILayout.LabelField(" Camera", EditorStyles.boldLabel);
            weapon.cameraRecoil = EditorGUILayout.FloatField(" Recoil", weapon.cameraRecoil);
            weapon.cameraShake = EditorGUILayout.FloatField(" Shake", weapon.cameraShake);
            weapon.cameraShakeFadeOutTime = EditorGUILayout.FloatField(" Fadeout Time", weapon.cameraShakeFadeOutTime);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" Camera Recoil", EditorStyles.boldLabel);
            weapon.horizontalRecoil = EditorGUILayout.FloatField(" Horizontal Recoil", weapon.horizontalRecoil);
            weapon.verticalRecoil = EditorGUILayout.FloatField(" Vertical Recoil", weapon.verticalRecoil);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" Weapon Recoil", EditorStyles.boldLabel);
            weapon.recoilPositionRoughness = EditorGUILayout.FloatField(" Position Damp Time", weapon.recoilPositionRoughness);
            weapon.recoilRotationRoughness = EditorGUILayout.FloatField(" Rotation Damp Time", weapon.recoilRotationRoughness);

            EditorGUILayout.Space();
            weapon.staticRecoilPosition = EditorGUILayout.Vector3Field(" Static Recoil Position", weapon.staticRecoilPosition);
            weapon.staticRecoilRotation = EditorGUILayout.Vector3Field(" Static Recoil Rotation", weapon.staticRecoilRotation);
            weapon.staticRecoilPositionAim = EditorGUILayout.Vector3Field(" Static Recoil Position Aim", weapon.staticRecoilPositionAim);
            weapon.staticRecoilRotationAim = EditorGUILayout.Vector3Field(" Static Recoil Rotation Aim", weapon.staticRecoilRotationAim);

            EditorGUILayout.Space();
            weapon.recoilPosition = EditorGUILayout.Vector3Field(" Recoil Position", weapon.recoilPosition);
            weapon.recoilRotation = EditorGUILayout.Vector3Field(" Recoil Rotation", weapon.recoilRotation);
            weapon.recoilPositionAim = EditorGUILayout.Vector3Field(" Recoil Position ADS", weapon.recoilPositionAim);
            weapon.recoilRotationAim = EditorGUILayout.Vector3Field(" Recoil Rotation Aim", weapon.recoilRotationAim);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private void UpdateMovement(FirearmData weapon)
        {
            EditorGUILayout.BeginHorizontal("box");
            weapon.useMovement = EditorGUILayout.Toggle(weapon.useMovement, GUILayout.MaxWidth(28));
            Foldout_Movement = EditorGUILayout.Foldout(Foldout_Movement, "Movement", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Movement) return;
            EditorGUILayout.BeginVertical("box");
            EditorGUI.BeginDisabledGroup(!weapon.useMovement);

            weapon.Movement_WalkSpeed = EditorGUILayout.FloatField(" Walk Speed", weapon.Movement_WalkSpeed);
            weapon.Movement_RunSpeed = EditorGUILayout.FloatField(" Run Speed", weapon.Movement_RunSpeed);
            weapon.Movement_TacticalSprintSpeed = EditorGUILayout.FloatField(" Tactical Sprint Speed", weapon.Movement_TacticalSprintSpeed);
            weapon.Movement_AimWalkSpeed = EditorGUILayout.FloatField(" Aim Walk Speed", weapon.Movement_AimWalkSpeed);
            weapon.Movement_FireWalkSpeed = EditorGUILayout.FloatField("  Fire Walk Speed", weapon.Movement_FireWalkSpeed);
            weapon.allowFullMovementSpeedWhileAiming = EditorGUILayout.Toggle("Allow Full Movmenet Speed During ADS", weapon.allowFullMovementSpeedWhileAiming);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private void UpdateAudio(FirearmData weapon)
        {
            EditorGUILayout.BeginHorizontal("box");
            weapon.useAudio = EditorGUILayout.Toggle(weapon.useAudio, GUILayout.MaxWidth(28));
            Foldout_Audio = EditorGUILayout.Foldout(Foldout_Audio, "Audio", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Audio) return;
            EditorGUILayout.BeginVertical("box");
            EditorGUI.BeginDisabledGroup(!weapon.useAudio);

            weapon.fire = EditorGUILayout.ObjectField(new GUIContent(" Fire"), weapon.fire, typeof(AudioProfile), true) as AudioProfile;

            if (!weapon.fire)
            {
                EditorGUILayout.HelpBox("Fire can't be null.", MessageType.Error);
                EditorGUILayout.Space();
            }

            weapon.fireTail = EditorGUILayout.ObjectField(new GUIContent(" Fire Tail"), weapon.fireTail, typeof(AudioProfile), true) as AudioProfile;

            if (!weapon.fireTail)
            {
                EditorGUILayout.HelpBox("Fire trail can't be null.", MessageType.Error);
                EditorGUILayout.Space();
            }

            weapon.reloadAudio = EditorGUILayout.ObjectField(new GUIContent(" Reload"), weapon.reloadAudio, typeof(AudioProfile), true) as AudioProfile;

            if (!weapon.reloadAudio)
            {
                EditorGUILayout.HelpBox("Reload can't be null.", MessageType.Error);
                EditorGUILayout.Space();
            }

            if (weapon.reloadMethod == Weapon.ReloadType.Default)
            {
                weapon.reloadEmptyAudio = EditorGUILayout.ObjectField(new GUIContent(" Reload Empty"), weapon.reloadEmptyAudio, typeof(AudioProfile), true) as AudioProfile;

                if (!weapon.reloadEmptyAudio)
                {
                    EditorGUILayout.HelpBox("Reload Empty can't be null.", MessageType.Error);
                    EditorGUILayout.Space();
                }
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }
    }
}