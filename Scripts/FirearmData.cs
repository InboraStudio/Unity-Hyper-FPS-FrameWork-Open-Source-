using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Inbora.FPSFramework
{
    [CreateAssetMenu(fileName = "New Firearm", menuName = "Inbora/FPS Framework/Weapons/Firearm Data")]
    public class FirearmData : ScriptableObject
    {
        //Basics
        public string Name;
        public Pickable replacement;
        public WeaponHUD canves;
        public Crosshair crosshair;

        //Fire Fields
        public Weapon.ShootingMechanism shootingMechanism;
        public Weapon.RechargingType rechargingType;
        public Weapon.FireMode fireMode = Weapon.FireMode.Auto;
        public Weapon.FireRateUnit fireRateUnit = Weapon.FireRateUnit.RoundPerMinute;
        public Vector3Direction casingDirection = Vector3Direction.right;

        public LayerMask hittableLayers = -1;

        public Projectile projectile;
        public GameObject casing;
        public GameObject defaultDecal;
        public Vector3Direction decalDirection;
        public float fireRate = 833;
        public float muzzleVelocity = 250;
        public float ejectionVelocity = 10;
        public float impactForce = 10;
        public float damage = 20;
        public float range = 300;
        public AnimationCurve damageRangeCurve = new AnimationCurve(new Keyframe[] {new Keyframe(0, 1), new Keyframe(1, 0)});


        public SprayPattern sprayPattern;
        public SprayPattern aimSprayPattern;

        public float fireTransition = 0.1f;
        public string rechargingStateName = "Recharging";
        public bool allowFireWhileRecharging;
        public float casingRotationFactor = 25;
        public float lowerdDelay = 10;

        public bool tracerRounds = true;
        public float projectileSize = 40;

        public float decalSize = 1;

        public float afterShotReliefTime = 0.2f;
        public int shotCount = 1;
        public float shotDelay = 0;
        public bool alwaysApplyFire = false;

        public float gamepadVibrationAmountRight = 1;
        public float gamepadVibrationAmountLeft = 1;
        public float gamepadVibrationDuration = 0.1f;

        //Ammunition
        public AmmoProfileData ammoType;
        public int magazineCapacity = 30;
        public int reserve = 0;
        public bool automaticReload = true;

        
        public Weapon.ReloadType reloadMethod;
        public float reloadTime = 1.6f;
        public float emptyReloadTime = 2.13f;

        public string reloadStateName = "Reload";
        public float reloadTransitionTime = 0.3f;

        //Recoil
        public float cameraRecoil = 1;
        public float cameraShake = 0.1f;
        public float cameraShakeFadeOutTime = 0.1f;

        public float horizontalRecoil = 0.7f;
        public float verticalRecoil = 0.1f;

        public float recoilPositionRoughness = 10f;
        public float recoilRotationRoughness = 10f;

        public Vector3 staticRecoilPosition = new Vector3(0, 0, -0.1f);
        public Vector3 staticRecoilRotation = new Vector3(-1, 2, -10);
        public Vector3 staticRecoilPositionAim = new Vector3(0, 0, -0.1f);
        public Vector3 staticRecoilRotationAim = new Vector3(-1, 2, -10);


        public Vector3 recoilPosition = new Vector3(0.01f, 0.01f, -0.3f);
        public Vector3 recoilRotation = new Vector3(-15f, 25, 15);


        public Vector3 recoilRotationAim = new Vector3(-1f, 3f, 3f);
        public Vector3 recoilPositionAim = new Vector3(-0.01f, 0.01f, 0.2f);

        //ADS (Aim Down Sight)
        public float aimSpeed = 10;
        public float aimFieldOfView = 50;
        public float aimWeaponFieldOfview = 40;


        //Movement
        public float Movement_WalkSpeed = 5;
        public float Movement_RunSpeed = 10;
        public float Movement_TacticalSprintSpeed = 12f;
        public float Movement_AimWalkSpeed = 3;
        public float Movement_FireWalkSpeed = 4;
        public bool allowFullMovementSpeedWhileAiming;


        //Audio
        public AudioProfile fire;
        public AudioProfile fireTail;
        public AudioProfile reloadAudio;
        public AudioProfile reloadEmptyAudio;

        public bool useFire { get; set; } = true;
        public bool useRecoil { get; set; } = true;
        public bool useMovement { get; set; } = true;
        public bool useAudio { get; set; } = true;
    }
}