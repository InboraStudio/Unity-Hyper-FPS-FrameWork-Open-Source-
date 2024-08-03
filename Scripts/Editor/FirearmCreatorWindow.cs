using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Inbora.FPSFramework
{
    public class FirearmCreatorWindow : EditorWindow
    {
        private FirearmData customData;
        private string weaponName;
        private FirearmPreset usedPreset;

        private bool useCustomPreset;




        #region Custom Settings

        public Pickable replacement;
        public WeaponHUD canves;
        public Crosshair crosshair;

        //fire
        public Weapon.RechargingType rechargingType;
        public Weapon.FireMode fireMode = Weapon.FireMode.Auto;
        public Weapon.FireRateUnit fireRateUnit = Weapon.FireRateUnit.RoundPerMinute;
        public Projectile projectile;
        public GameObject shell;
        public float fireRate = 833;
        public float muzzleVelocity = 250;
        public float ejectionVelocity = 10;
        public float impactForce = 10;
        public float damage = 20;
        public float fireTransition = 0.1f;
        public string rechargingStateName = "Recharging";
        public bool allowFireWhileRecharging;
        public float shellRotationFactor = 25;
        public float lowerdDelay = 10;

        public bool updateProjectileSize = true;
        public float projectileSize = 40;

        public float decalSize = 1;

        public float afterShotReliefTime = 0.2f;
        public int shotCount = 1;
        public float shotDelay = 0;
        public bool alwaysApplyFire = false;

        //ammo
        public AmmoProfileData ammoType;
        public int magazineCapacity = 30;
        public int reserve = 0;
        public bool automaticReload = true;
        public bool reloadOnAwake = false;

        public Weapon.ReloadType reloadType;
        public bool flexibleReloadTime;
        public float reloadTime = 1.6f;
        public float emptyReloadTime = 2.13f;

        //recoil
        public float recoilPositionRoughness = 10f;
        public float recoilRotationRoughness = 10f;
        public float horizontalRecoil = 0.7f;
        public float verticalRecoil = 0.1f;
        public float cameraRecoil = 1;
        public Vector3 RecoilRotation = new Vector3(-15f, 25, 15);
        public Vector3 RecoilKickBack = new Vector3(0.01f, 0.01f, -0.3f);

        public Vector3 RecoilRotation_Aim = new Vector3(-1f, 3f, 3f);
        public Vector3 RecoilKickBack_Aim = new Vector3(-0.01f, 0.01f, 0.2f);

        //aim down sight
        public float aimSpeed = 10;
        public float aimFieldOfView = 50;
        public float aimWeaponFieldOfview = 40;


        //Audio
        public AudioProfile fire;
        public AudioProfile fireLoop;
        public AudioProfile fireTail;
        #endregion

        public Vector3 staticRecoilRotation = new Vector3(-1, 2, -10);
        public float cameraShake = 0.1f;
        public float cameraShakeFadeOutTime = 0.1f;
        public AudioProfile reloadAudio;
        public AudioProfile reloadEmptyAudio;

        public bool Use_Fire = true;
        public bool Use_Recoil = true;
        public bool Use_Audio = true;

        public bool Foldout_Fire;
        public bool Foldout_Recoil;
        public bool Foldout_Audio;


        private Vector2 scrollPos;


        [MenuItem(MenuItemPaths.CreateFirearm)]
        public static void Draw()
        {
            EditorWindow window = GetWindow(typeof(FirearmCreatorWindow));

            window.titleContent = new GUIContent("Firearm Creator");
            window.minSize = new Vector2(276, 271);
        }

        private void Awake()
        {
            weaponName = null;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Base", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            customData = EditorGUILayout.ObjectField("Custom Data", customData, typeof(FirearmData), true) as FirearmData;

            EditorGUI.BeginDisabledGroup(customData != null);
            if (GUILayout.Button(new GUIContent("+", "Creates new firearm data with the settings"), GUILayout.MaxWidth(25)))
            {
                CreateData(true);
            }

            EditorGUILayout.EndHorizontal();


            weaponName = EditorGUILayout.TextField("Name", weaponName);

            EditorGUI.BeginDisabledGroup(useCustomPreset);
            usedPreset = (FirearmPreset)EditorGUILayout.EnumPopup("Preset", usedPreset);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginVertical("box");
            useCustomPreset = EditorGUILayout.ToggleLeft("Custom", useCustomPreset, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();


            EditorGUI.BeginDisabledGroup(!useCustomPreset);

            if (!useCustomPreset)
            {
                Foldout_Audio = false;
                Foldout_Fire = false;
                Foldout_Recoil = false;
            }


            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


            UpdateBase();


            UpdateFire();
            UpdateRecoil();
            UpdateAudio();

            EditorGUILayout.EndScrollView();


            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Create", GUILayout.MaxWidth(100)))
            {
                FirearmData data = CreateData();
                Firearm newFirearm = FPSFrameworkEditor.CreateFirearm(weaponName);
                newFirearm.preset = data;
                newFirearm.gameObject.AddComponent<ItemInput>();
                Close();
            }
        }

        private FirearmData CreateData(bool selectAsset = false)
        {
            if (useCustomPreset)
                return CreateFirearmData(weaponName);
            else
                return CreateFirearmFromPreset();
        }

        private FirearmData CreateFirearmFromPreset()
        {
            FirearmData data = CreateFirearmData(weaponName);

            switch (usedPreset)
            {
                #region AssaultRifle
                case FirearmPreset.AssaultRifle:

                    data.useFire = true;
                    data.useRecoil = true;

                    data.fireMode = Weapon.FireMode.Selective;
                    data.rechargingType = Weapon.RechargingType.GasPowerd;
                    data.fireRateUnit = Weapon.FireRateUnit.RoundPerMinute;
                    data.fireRate = 833;
                    data.muzzleVelocity = 500;
                    data.ejectionVelocity = 7;
                    data.casingRotationFactor = 25;
                    data.impactForce = 30;
                    data.damage = 23;
                    data.fireTransition = 0.1f;
                    data.tracerRounds = true;
                    data.projectileSize = 60;
                    data.decalSize = 1;
                    data.afterShotReliefTime = 1;
                    data.shotCount = 1;

                    data.magazineCapacity = 30;
                    data.reserve = 30;
                    data.automaticReload = false;

                    data.reloadMethod = Weapon.ReloadType.Default;
                    data.reloadTime = 1.6f;
                    data.emptyReloadTime = 2.13f;

                    data.cameraRecoil = 1;
                    data.cameraShake = 0.02f;
                    data.cameraShakeFadeOutTime = 0.2f;
                    data.horizontalRecoil = 0.7f;
                    data.verticalRecoil = 0.1f;
                    data.recoilPositionRoughness = 10;
                    data.recoilRotationRoughness = 10;
                    data.staticRecoilRotation = new Vector3(-1, 2, 10);
                    data.recoilPosition = new Vector3(0.01f, 0.01f, -0.3f);
                    data.recoilRotation = new Vector3(-10, 15, 25);
                    data.recoilPositionAim = new Vector3(-0.01f, 0.01f, -0.16f);
                    data.recoilRotationAim = new Vector3(-2, 3, 3);

                    break;
                #endregion

                #region Sniper
                case FirearmPreset.Sniper:
                    data.useFire = true;
                    data.useRecoil = true;

                    data.fireMode = Weapon.FireMode.SemiAuto;
                    data.rechargingType = Weapon.RechargingType.Manual;
                    data.fireRateUnit = Weapon.FireRateUnit.RoundPerMinute;
                    data.fireRate = 500;
                    data.muzzleVelocity = 714;
                    data.ejectionVelocity = 7;
                    data.casingRotationFactor = 25;
                    data.impactForce = 50;
                    data.damage = 70;
                    data.fireTransition = 0f;

                    data.rechargingStateName = "Fire";
                    data.allowFireWhileRecharging = false;

                    data.tracerRounds = true;
                    data.projectileSize = 40;
                    data.decalSize = 1;
                    data.afterShotReliefTime = 1;
                    data.shotCount = 1;

                    data.magazineCapacity = 5;
                    data.reserve = 5;
                    data.automaticReload = false;

                    data.reloadMethod = Weapon.ReloadType.Default;
                    data.reloadTime = 1.73f;
                    data.emptyReloadTime = 3.17f;

                    data.cameraRecoil = 3;
                    data.cameraShake = 0.14f;
                    data.cameraShakeFadeOutTime = 0.3f;
                    data.horizontalRecoil = 3.3f;
                    data.verticalRecoil = 0.3f;
                    data.recoilPositionRoughness = 10;
                    data.recoilRotationRoughness = 10;
                    data.staticRecoilRotation = new Vector3(-1, 2, 40);
                    data.recoilPosition = new Vector3(0.01f, 0.01f, -0.7f);
                    data.recoilRotation = new Vector3(-15, 25, 15);
                    data.recoilPositionAim = new Vector3(-0.01f, 0.01f, -0.7f);
                    data.recoilRotationAim = new Vector3(-2, 3, 3);
                    break;
                #endregion

                #region Shotgun
                case FirearmPreset.Shotgun:
                    data.useFire = true;
                    data.useRecoil = true;

                    data.fireMode = Weapon.FireMode.SemiAuto;
                    data.rechargingType = Weapon.RechargingType.Manual;
                    data.fireRateUnit = Weapon.FireRateUnit.RoundPerMinute;
                    data.fireRate = 100;
                    data.muzzleVelocity = 700;
                    data.ejectionVelocity = 7;
                    data.casingRotationFactor = 25;
                    data.impactForce = 25;
                    data.damage = 90;
                    data.fireTransition = 0.1f;

                    data.rechargingStateName = "Fire";
                    data.allowFireWhileRecharging = false;

                    data.tracerRounds = false;
                    data.projectileSize = 0.1f;
                    data.decalSize = 2;
                    data.afterShotReliefTime = 1;
                    data.shotCount = 7;

                    data.magazineCapacity = 5;
                    data.reserve = 5;
                    data.automaticReload = false;

                    data.reloadMethod = Weapon.ReloadType.Scripted;
                    data.reloadTime = 0.583f;
                    data.emptyReloadTime = 0;

                    data.cameraRecoil = 2;
                    data.cameraShake = 0.2f;
                    data.cameraShakeFadeOutTime = 0.4f;
                    data.horizontalRecoil = 1;
                    data.verticalRecoil = 3;
                    data.recoilPositionRoughness = 10;
                    data.recoilRotationRoughness = 10;
                    data.staticRecoilRotation = new Vector3(-25, 30, 10);
                    data.recoilPosition = new Vector3(0.01f, 0.1f, -1f);
                    data.recoilRotation = new Vector3(-15, 25, 15);
                    data.recoilPositionAim = new Vector3(-0.01f, 0.01f, -0.5f);
                    data.recoilRotationAim = new Vector3(-2, 3, 3);
                    break;
                #endregion

                #region Pistol
                case FirearmPreset.Pistol:

                    data.useFire = true;
                    data.useRecoil = true;

                    data.fireMode = Weapon.FireMode.SemiAuto;
                    data.rechargingType = Weapon.RechargingType.GasPowerd;
                    data.fireRateUnit = Weapon.FireRateUnit.RoundPerMinute;
                    data.fireRate = 1000;
                    data.muzzleVelocity = 250;
                    data.ejectionVelocity = 10;
                    data.casingRotationFactor = 25;
                    data.impactForce = 15;
                    data.damage = 15;
                    data.fireTransition = 0.05f;
                    data.tracerRounds = true;
                    data.projectileSize = 70;
                    data.decalSize = 0.7f;
                    data.afterShotReliefTime = 1;
                    data.shotCount = 1;

                    data.magazineCapacity = 12;
                    data.reserve = 12;
                    data.automaticReload = false;

                    data.reloadMethod = Weapon.ReloadType.Default;
                    data.reloadTime = 1.54f;
                    data.emptyReloadTime = 1.95f;

                    data.cameraRecoil = 2;
                    data.cameraShake = 0.06f;
                    data.cameraShakeFadeOutTime = 0.2f;
                    data.horizontalRecoil = 0.7f;
                    data.verticalRecoil = 0.1f;
                    data.recoilPositionRoughness = 10;
                    data.recoilRotationRoughness = 10;
                    data.staticRecoilRotation = new Vector3(-9, 15, 1);
                    data.recoilPosition = new Vector3(0.01f, 0.01f, -0.5f);
                    data.recoilRotation = new Vector3(-10, 15, 25);
                    data.recoilPositionAim = new Vector3(-0.01f, 0.01f, -0.4f);
                    data.recoilRotationAim = new Vector3(-20, 10, 25);

                    break;
                    #endregion
            }

            return data;
        }

        private FirearmData CreateFirearmData(string name = "Untitled Firearm", bool selectAsset = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                weaponName = "Untitled Firearm";
                name = weaponName;
            }

            FirearmData newData = CreateInstance<FirearmData>();
            newData.Name = name;
            newData.name = name;

            AssetDatabase.CreateAsset(newData, GetProjectWindowPath() + "/" + newData.Name + ".asset");
            AssetDatabase.SaveAssets();

            if (selectAsset)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newData;
            }

            customData = newData;

            return newData;
        }

        /// <summary>
        /// returns string path to current opened folder path in project window
        /// </summary>
        /// <returns></returns>
        public static string GetProjectWindowPath()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);

            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

            object obj = getActiveFolderPath.Invoke(null, new object[0]);

            return obj.ToString();
        }

        private enum FirearmPreset
        {
            AssaultRifle,
            Sniper,
            Shotgun,
            Pistol
        }


        private void UpdateBase()
        {
            replacement = EditorGUILayout.ObjectField(new GUIContent("Replacement", "The pickuable item which is going to be thrown when replacing weapon with another or throwing it."), replacement, typeof(Pickable), true) as Pickable;
            canves = EditorGUILayout.ObjectField(new GUIContent("Canves", "The object which contains all the UI elments of the weapon this sould be a prefap."), canves, typeof(WeaponHUD), true) as WeaponHUD;
            crosshair = EditorGUILayout.ObjectField(new GUIContent("Crosshair", "The used crosshair for this weapon."), crosshair, typeof(Crosshair), true) as Crosshair;
        }

        private void UpdateFire()
        {
            EditorGUILayout.BeginHorizontal("box");
            Use_Fire = EditorGUILayout.Toggle(Use_Fire, GUILayout.MaxWidth(28));
            Foldout_Fire = EditorGUILayout.Foldout(Foldout_Fire, "Fire", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Fire) return;
            EditorGUILayout.BeginVertical("box");

            EditorGUI.BeginDisabledGroup(!Use_Fire);

            fireMode = (Weapon.FireMode)EditorGUILayout.EnumPopup(new GUIContent(" Mode", "What the type of fire you want Auto or Semi-Auto"), fireMode);
            rechargingType = (Weapon.RechargingType)EditorGUILayout.EnumPopup(new GUIContent(" Bolt Type", "If set to Manual shells will be thrown via animation event and weapon won't shoot if playing the rechaing animation this is going to be used in bolt action weapons."), rechargingType);
            fireRateUnit = (Weapon.FireRateUnit)EditorGUILayout.EnumPopup(new GUIContent(" Fire Rate Unit", "How fire rate will be counted as Round per minite or Round per second."), fireRateUnit);
            projectile = EditorGUILayout.ObjectField(new GUIContent(" Projectile", "The projectile which going to be fired."), projectile, typeof(Projectile), true) as Projectile;

            if (!projectile)
            {
                EditorGUILayout.HelpBox("Projectile must be assgined in order to use this section.", MessageType.Error);
                EditorGUILayout.Space();
            }

            if (projectile)
            {
                shell = EditorGUILayout.ObjectField(new GUIContent(" Casing", "Most weapons ejects a casing after each shot however this is optinal"), shell, typeof(GameObject), true) as GameObject;

                EditorGUILayout.BeginHorizontal();
                fireRate = EditorGUILayout.FloatField(new GUIContent(" Fire Rate", "how many round will be fired depening on the unit"), fireRate);
                if (fireRateUnit == Weapon.FireRateUnit.RoundPerMinute)
                    EditorGUILayout.LabelField("RPM", GUILayout.MaxWidth(33));
                if (fireRateUnit == Weapon.FireRateUnit.RoundPerSecond)
                    EditorGUILayout.LabelField("RPS", GUILayout.MaxWidth(33));

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                muzzleVelocity = EditorGUILayout.FloatField(new GUIContent(" Muzzle Velocity", "the speed of the projectile."), muzzleVelocity);
                EditorGUILayout.LabelField("M/S", GUILayout.MaxWidth(33));
                EditorGUILayout.EndHorizontal();

                if (shell != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    ejectionVelocity = EditorGUILayout.FloatField(new GUIContent(" Casing Ejection Velocity", "the speed of the shell when fired."), ejectionVelocity);
                    EditorGUILayout.LabelField("M/S", GUILayout.MaxWidth(33));
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();

                if (shell != null)
                {
                    shellRotationFactor = EditorGUILayout.FloatField(new GUIContent(" Casing Rotation", "The amount of random rotaion for the shells."), shellRotationFactor);
                }

                impactForce = EditorGUILayout.FloatField(new GUIContent(" Impact Force", "The force applied on impact with any rigidbody"), impactForce);

                damage = EditorGUILayout.FloatField(new GUIContent(" Damage", "Amount of damage this weapon can do to any object with Health System script."), damage);


                fireTransition = EditorGUILayout.FloatField(new GUIContent(" Fire Transition", "The time needed to tranition bettwen current animation to fire animation."), fireTransition);

                if (rechargingType == Weapon.RechargingType.Manual)
                {
                    rechargingStateName = EditorGUILayout.TextField(new GUIContent(" Recharging State Name", "the bold action animation state name used in bolt action weapons."), rechargingStateName);
                    allowFireWhileRecharging = EditorGUILayout.Toggle(new GUIContent(" Allow Fire While Recharging", "Is it okay to fire when charing the bolt."), allowFireWhileRecharging);
                }

                updateProjectileSize = EditorGUILayout.Toggle(new GUIContent(" Dynamic Projectile Size", "Does the projectile scale depening on the distance from player this is used to make MW like projectiles."), updateProjectileSize);
                projectileSize = EditorGUILayout.FloatField(new GUIContent(" Projectile Size", "The scale of the projecile."), projectileSize);
                decalSize = EditorGUILayout.FloatField(new GUIContent(" Decal Size", "the scale of the decals on impact."), decalSize);
                afterShotReliefTime = EditorGUILayout.FloatField(new GUIContent(" After Shot Relief Time", "The time that you can't sprint after you fire."), afterShotReliefTime);
                shotCount = EditorGUILayout.IntField(new GUIContent(" Shot Count", "How many projectiles will be fired at once."), shotCount);

                if (shotCount > 1)
                {

                    EditorGUILayout.BeginHorizontal();
                    shotDelay = EditorGUILayout.FloatField(new GUIContent(" Shots Delay", "the delay bettwen every shot ignoring of the first one."), shotDelay);

                    if (GUILayout.Button(new GUIContent(" Calculate", "Calculates shot delay from fire rate")))
                    {
                        if (fireRateUnit == Weapon.FireRateUnit.RoundPerMinute)
                        {
                            shotDelay = 1 / (fireRate / 60);
                        }

                        if (fireRateUnit == Weapon.FireRateUnit.RoundPerSecond)
                        {
                            shotDelay = 1 / fireRate;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    alwaysApplyFire = EditorGUILayout.Toggle(new GUIContent(" Auto Apply Fire", "Always play fire sound and throw a shell."), alwaysApplyFire);
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField(" Ammunition", EditorStyles.boldLabel);
                ammoType = EditorGUILayout.ObjectField("Ammo Profile", ammoType, typeof(AmmoProfileData), true) as AmmoProfileData;
                magazineCapacity = EditorGUILayout.IntField(new GUIContent(" Magazine Capacity", "How many projectiles can the mag have at once."), magazineCapacity);
                reserve = EditorGUILayout.IntField(new GUIContent(" Reserve", "The amount of ammo in the mag at the moment."), reserve);

                automaticReload = EditorGUILayout.Toggle(" Automatic Reload", automaticReload);
                reloadOnAwake = EditorGUILayout.Toggle(" Reload On Awake", reloadOnAwake);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField(" Reload", EditorStyles.boldLabel);

                reloadType = (Weapon.ReloadType)EditorGUILayout.EnumPopup(new GUIContent(" Type", "type of reload handling manaul needs animation events."), reloadType);
                reloadTime = EditorGUILayout.FloatField(new GUIContent(" Reload Time", "Time needed to reload."), reloadTime);
                if (reloadType == Weapon.ReloadType.Scripted)
                {
                    flexibleReloadTime = EditorGUILayout.ToggleLeft(new GUIContent(" Flexible Reload Time", "is the reload time will depend on ammo reserve"), flexibleReloadTime);
                }

                if (reloadType == Weapon.ReloadType.Default)
                {

                    emptyReloadTime = EditorGUILayout.FloatField(new GUIContent(" Empty Reload Time", "Time needed to reload empty mag."), emptyReloadTime);
                }
            }


            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private void UpdateRecoil()
        {
            EditorGUILayout.BeginHorizontal("box");
            Use_Recoil = EditorGUILayout.Toggle(Use_Recoil, GUILayout.MaxWidth(28));
            Foldout_Recoil = EditorGUILayout.Foldout(Foldout_Recoil, "Recoil", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Recoil) return;
            EditorGUILayout.BeginVertical("box");

            EditorGUI.BeginDisabledGroup(!Use_Recoil);

            EditorGUILayout.LabelField(" Camera", EditorStyles.boldLabel);
            cameraRecoil = EditorGUILayout.FloatField(" Recoil", cameraRecoil);
            cameraShake = EditorGUILayout.FloatField(" Shake", cameraShake);
            cameraShakeFadeOutTime = EditorGUILayout.FloatField(" Fadeout Time", cameraShakeFadeOutTime);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" Camera Recoil", EditorStyles.boldLabel);
            horizontalRecoil = EditorGUILayout.FloatField(" Horizontal Recoil", horizontalRecoil);
            verticalRecoil = EditorGUILayout.FloatField(" Vertical Recoil", verticalRecoil);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" Weapon Recoil", EditorStyles.boldLabel);
            recoilPositionRoughness = EditorGUILayout.FloatField(" Position Damp Time", recoilPositionRoughness);
            recoilRotationRoughness = EditorGUILayout.FloatField(" Rotation Damp Time", recoilRotationRoughness);

            staticRecoilRotation = EditorGUILayout.Vector3Field(" Static Recoil Rotation", staticRecoilRotation);
            RecoilKickBack = EditorGUILayout.Vector3Field(" Kick Back", RecoilKickBack);
            RecoilRotation = EditorGUILayout.Vector3Field(" Rotation", RecoilRotation);
            RecoilKickBack_Aim = EditorGUILayout.Vector3Field(" KickBack Aim", RecoilKickBack_Aim);
            RecoilRotation_Aim = EditorGUILayout.Vector3Field(" Rotation Aim", RecoilRotation_Aim);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private void UpdateAudio()
        {
            EditorGUILayout.BeginHorizontal("box");
            Use_Audio = EditorGUILayout.Toggle(Use_Audio, GUILayout.MaxWidth(28));
            Foldout_Audio = EditorGUILayout.Foldout(Foldout_Audio, "Audio", true);
            EditorGUILayout.EndHorizontal();

            if (!Foldout_Audio) return;
            EditorGUILayout.BeginVertical("box");
            EditorGUI.BeginDisabledGroup(!Use_Audio);

            fire = EditorGUILayout.ObjectField(new GUIContent(" Fire", "The sound which going to play when firing."), fire, typeof(AudioProfile), true) as AudioProfile;

            if (!fire)
            {
                EditorGUILayout.HelpBox("Fire can't be null.", MessageType.Error);
                EditorGUILayout.Space();
            }

            fireTail = EditorGUILayout.ObjectField(new GUIContent(" Fire Tail", "The sound which going to play when firing."), fireTail, typeof(AudioProfile), true) as AudioProfile;

            if (!fireTail)
            {
                EditorGUILayout.HelpBox("Fire trail can't be null.", MessageType.Error);
                EditorGUILayout.Space();
            }

            reloadAudio = EditorGUILayout.ObjectField(new GUIContent(" Reload", "The sound which going to play when reloading with ammo in the mag."), reloadAudio, typeof(AudioProfile), true) as AudioProfile;

            if (!reloadAudio)
            {
                EditorGUILayout.HelpBox("Reload can't be null.", MessageType.Error);
                EditorGUILayout.Space();
            }

            if (reloadType == Weapon.ReloadType.Default)
            {
                reloadEmptyAudio = EditorGUILayout.ObjectField(new GUIContent(" Reload Empty", "The sound which going to play when reloading empty mag."), reloadEmptyAudio, typeof(AudioProfile), true) as AudioProfile;

                if (!reloadEmptyAudio)
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