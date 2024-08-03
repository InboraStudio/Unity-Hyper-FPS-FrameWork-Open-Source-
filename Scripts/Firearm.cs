using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapone/Firearm")]
    [RequireComponent(typeof(FirearmAttachmentsManager))]
    public class Firearm : Weapon
    {
        [Header("Base")]
        public FirearmData preset;
        public Transform _muzzle;
        public Transform casingEjectionPort;
        public Transform recoilTranform;
        public ParticleSystem rechargingVFX;

        [Space]
        public FirearmEvents events;

        private Vector3 defaultRecoilPosition;
        private Vector3 currentRecoilRotation;
        private Vector3 currentRecoil2;
        private Vector3 currentRecoilPosition;
        private Vector3 currentRecoil4;
        private Vector3 recoilRotationOutput;

        /// <summary>
        /// used ammo type
        /// </summary>
        public AmmoProfile AmmoProfile { get; set; }
        public WeaponHUD HUD { get; set; }
        public FirearmAttachmentsManager attachmentsManager { get; protected set; }

        /// <summary>
        /// all weapon effects
        /// </summary>
        public ParticleSystem[] VFX { get; set; }
        /// <summary>
        /// current fire modes if using selective
        /// </summary>
        public FireMode currentSelectiveFireMode { get; set; }

        public bool IsFiring { get; set; }
        public bool IsReloading { get; set; }
        private float fireTimer;


        private int shots;

        public AudioProfile currentFireAudio { get; set; }
        private float currentSpray;
        private float TacticalSprintAmount;
        private Vector3 leanOffset;

        private SprayPattern sprayPattern;
        private SprayPattern aimSprayPattern;

        private Transform farFireLocation;

        private GameObject projectilesParent;
        private GameObject casingsParent;
        public ItemInput inputManager { get; private set; }
        private Controls controls { get; set; }
        private bool fireInput { get; set; }

        public Audio fireAudio = new Audio();
        public Audio fireTailAudio = new Audio();
        public Audio reloadAudio = new Audio();
        public Audio reloadEmptyAudio = new Audio();

        /// <summary>
        /// The remianing ammo count in mag. EXMAPLE: The firearm mag, has 30 bullets left.
        /// </summary>
        public int remainingAmmoCount { get; set; }
        /// <summary>
        /// The remaning ammo count of the type this firearm is using. EXAMPLE: The inventory has 500 bullets of the green ammo. 
        /// </summary>
        public int remainingAmmoTypeCount { get; set; }
        /// <summary>
        /// Max ammo that the mag can carry.
        /// </summary>
        public int magazineCapacity { get; set; }

        public bool isLocallyMine
        {
            get
            {
                return Actor?.characterManager?.character != null;
            }
        }

        public float GetReservePercentage()
        {
            return (float)remainingAmmoCount / magazineCapacity;
        }

        public Projectile CreateProjectile(Projectile projectile, Firearm source, Transform muzzle, Vector3 direction, float speed, float range, Transform parent = null)
        {
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation, parent);
            newProjectile.direction = direction;

            if (characterManager && characterManager.characterController)
                newProjectile.shooterVelocity = characterManager.characterController.velocity;

            float muzzleModifier = source?.attachmentsManager != null ? source.attachmentsManager.muzzleVelocity : 1;

            newProjectile.speed = speed * muzzleModifier;
            newProjectile.source = source;
            newProjectile.range = range * source.attachmentsManager.range;
            newProjectile.useAutoScaling = source.preset.tracerRounds;
            newProjectile.scaleMultipler = source.preset.projectileSize;
            newProjectile.damageRangeCurve = source.preset.damageRangeCurve;

            Projectiles?.Add(newProjectile);

            return newProjectile;
        }

        /// <summary>
        /// Resets all values of the weapon
        /// </summary>
        public void ResetStates()
        {
            IsReloading = false;
        }

        private void Awake()
        {
            controls = new Controls();
            controls.Firearm.Enable();
            Setup(preset.Name, preset.replacement);
            controls.Firearm.Fire.performed += context =>
            {
                if (preset.fireMode == FireMode.Auto || preset.fireMode == FireMode.Selective && currentSelectiveFireMode == FireMode.Auto)
                {
                    fireInput = true;
                }
            };

            controls.Firearm.Fire.canceled += context =>
            {
                fireInput = false;
            };

            characterManager = GetComponentInParent<CharacterManager>();
            attachmentsManager = GetComponent<FirearmAttachmentsManager>();
            inputManager = GetComponent<ItemInput>();

            projectilesParent = GameObject.Find("Projectiles");
            casingsParent = GameObject.Find("Casings");

            if (!projectilesParent) projectilesParent = new GameObject("Projectiles");
            if (!casingsParent) casingsParent = new GameObject("Casings");

            if (!preset)
            {
                Debug.LogError("Firearm is not setup due to null preset");
                return;
            }

            //Initialize
            Firearm = this;
            Name = preset?.Name;
            Replacement = preset?.replacement;

            if (_muzzle)
            {
                farFireLocation = _muzzle.CreateChild("Far Fire Location");
                farFireLocation.position = _muzzle.forward * 1500;
            }

            VFX = GetComponentsInChildren<ParticleSystem>();

            if (Motion)
                Motion.firearm = this;

            //Initialize HUD
            if (preset.canves)
            {
                HUD = Instantiate(preset.canves, transform);
                HUD.target = this;
                HUD._ammoCount = true;
                HUD._gernadesCount = true;
                HUD._ammoName = true;
                HUD.defaultCrosshair = preset.crosshair;
            }
        }

        private void Start()
        {
            characterManager = GetComponentInParent<CharacterManager>();
            if (recoilTranform) defaultRecoilPosition = recoilTranform.localPosition;

            if (!preset) return;

            sprayPattern = preset.sprayPattern ? preset.sprayPattern : ScriptableObject.CreateInstance<SprayPattern>();
            aimSprayPattern = preset.aimSprayPattern ? preset.aimSprayPattern : ScriptableObject.CreateInstance<SprayPattern>();

            if (!preset.sprayPattern)
                Debug.LogWarning($"{preset.Name} doesn't have spray pattern firearm will use an instance but it's recommended to use custom pattern");

            if (!preset.aimSprayPattern)
                Debug.LogWarning($"{preset.Name} doesn't have aim spray pattern firearm will use an instance but it's recommended to use custom pattern");

            remainingAmmoCount = preset.reserve;
            magazineCapacity = preset.magazineCapacity;
            if (preset.fireMode == FireMode.Selective) autoFireMode = true;

            if (!_muzzle) _muzzle = transform;
            if (!casingEjectionPort) casingEjectionPort = transform;

            //reset weapon 
            ResetStates();

            if (Inventory)
            {
                //Initialize Ammunition
                AmmoProfile = Inventory.FindAmmunition(preset.ammoType);

                if (characterManager != null)
                {
                    //Reset movement speed
                    characterManager?.character?.ResetSpeed();
                }
            }

            currentFireAudio = preset.fire;

        }

        public void Update()
        {
            if (!preset)
            {
                Debug.LogError($"Firearm on {gameObject.name} has no preset. Firearm wouldn't function please stop play mode and assign preset in order to function correctly.");
                return;
            }

            if (preset.reloadMethod == ReloadType.Scripted && remainingAmmoCount >= magazineCapacity) IsReloading = false;

            if (AmmoProfile == null)
            {
                AmmoProfile = new AmmoProfile();
                AmmoProfile.data = ScriptableObject.CreateInstance<AmmoProfileData>();

                AmmoProfile.data.Name = "No Ammo Data";
                AmmoProfile.amount = 100;
            }

            remainingAmmoTypeCount = AmmoProfile.amount;

            //get input
            UpdateInput();
            UpdateMovement();
            if (AmmoProfile.amount <= 0) IsReloading = false;

            //update spray value
            if (Motion)
            {
                currentSpray = Mathf.Lerp(currentSpray, Mathf.Lerp(sprayPattern.GetAmount(this) / 2, aimSprayPattern.GetAmount(this), Motion.ADSAmount) * attachmentsManager.spread, Time.deltaTime * 10);
            }
            else
            {
                currentSpray = sprayPattern.GetAmount(this);
            }

            if (IsFiring) currentSpray = sprayPattern.GetAmount(this);


            remainingAmmoCount = Mathf.Clamp(remainingAmmoCount, 0, magazineCapacity);

            if (shots >= preset.shotCount) shots = 0;

            if (IsFiring)
            {
                Fire();
                events.OnFire?.Invoke();

                if (preset.reloadAudio) reloadAudio.Stop();
                if(preset.reloadEmptyAudio) reloadEmptyAudio.Stop();
            }

            //update animations
            if (Animator)
            {
                Animator.SetBool("Is Reloading", IsReloading);
                Animator.SetInteger("Ammo", remainingAmmoCount);
                Animator.SetFloat("ADS Amount", Motion.ADSAmount);

                if (characterManager.character.tacticalSprintAmount > 0 && !inputManager.aimInput && !IsFiring) TacticalSprintAmount = Mathf.Lerp(TacticalSprintAmount, characterManager.character.tacticalSprintAmount, Time.deltaTime * Motion.sprintSpeed * 0.2f);
                if (characterManager.character.tacticalSprintAmount <= 0 && !inputManager.aimInput && !IsFiring) TacticalSprintAmount = Mathf.Lerp(TacticalSprintAmount, characterManager.character.tacticalSprintAmount, Time.deltaTime * Motion.sprintSpeed * 5);
                if (inputManager.aimInput || IsFiring) TacticalSprintAmount = Mathf.Lerp(TacticalSprintAmount, 0, Time.deltaTime * Motion.sprintSpeed * 2);

                Animator.SetFloat("Sprint Amount", TacticalSprintAmount);
            }

            if (HUD)
            {
                HUD.UpdateUI();

                if (HUD.Crosshair)
                {
                    HUD.Crosshair.UpdateSize(currentSpray);
                    if (HUD.Crosshair.floatingRect && _muzzle)
                    {
                        if (Physics.Raycast(_muzzle.position, _muzzle.forward, out RaycastHit hit))
                            HUD.Crosshair.floatingRect.position = hit.point;
                        else if (HUD.Crosshair.floatingRect && farFireLocation)
                            HUD.Crosshair.floatingRect.position = farFireLocation.position;
                    }
                }

                if (Inventory)
                {
                    if (characterManager.characterInput.sprintInput && !inputManager.aimInput) HUD.Crosshair.HideLines();
                    if (!inputManager.aimInput && !characterManager.characterInput.sprintInput) HUD?.Crosshair?.Show();
                }

                if (inputManager.aimInput) HUD.Crosshair.HideAll();
            }
        }

        /// <summary>
        /// updates movement speed in the controller
        /// </summary>
        private void UpdateMovement()
        {
            if (!preset.useMovement) return;
            if (IsFiring || Motion.afterShotRelief)
            {
                characterManager.character.SetSpeed(preset.Movement_FireWalkSpeed, preset.Movement_FireWalkSpeed, preset.Movement_FireWalkSpeed);
            }
            else
            {
                if (inputManager && inputManager.aimInput)
                {
                    characterManager.character.SetSpeed(preset.Movement_AimWalkSpeed, preset.Movement_AimWalkSpeed, preset.Movement_AimWalkSpeed);
                }
                else
                {
                    characterManager.character.SetSpeed(preset.Movement_WalkSpeed, preset.Movement_RunSpeed, preset.Movement_TacticalSprintSpeed);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!preset) return;

            currentRecoilRotation = Vector3.Lerp(currentRecoilRotation, Vector3.zero, 35 * Time.deltaTime);
            currentRecoil2 = Vector3.Lerp(currentRecoil2, currentRecoilRotation, 50 * Time.deltaTime);
            currentRecoilPosition = Vector3.Lerp(currentRecoilPosition, Vector3.zero, 35 * Time.deltaTime);
            currentRecoil4 = Vector3.Lerp(currentRecoil4, currentRecoilPosition, 50 * Time.deltaTime);

            if (preset.useRecoil && recoilTranform)
            {
                recoilTranform.localPosition = Vector3.Slerp(recoilTranform.localPosition, defaultRecoilPosition + currentRecoilPosition, preset.recoilPositionRoughness * Time.fixedDeltaTime);
                recoilRotationOutput = Vector3.Slerp(recoilRotationOutput, currentRecoilRotation, preset.recoilRotationRoughness * Time.fixedDeltaTime);
                recoilTranform.localRotation = Quaternion.Euler(recoilRotationOutput);
            }

            if (preset.fireTail && !preset.fireTail)
                fireTailAudio.Equip(gameObject, preset.fireTail);

            if (preset.reloadAudio)
                reloadAudio.Equip(gameObject, preset.reloadAudio);

            if (preset.reloadEmptyAudio)
                reloadEmptyAudio.Equip(gameObject, preset.reloadEmptyAudio);

            if (preset.fireTail) fireTailAudio.UpdatePitch();
            if (preset.reloadAudio) reloadAudio.UpdatePitch();
            if (preset.reloadEmptyAudio) reloadEmptyAudio.UpdatePitch();
        }

        private bool autoFireMode;

        /// <summary>
        /// handles all weapon input
        /// </summary>
        private void UpdateInput()
        {
            if (!inputManager) return;

            if (Motion)
            {
                Motion.isCrouching = characterManager.characterInput.crouchInput;
                Motion.IsReloading = IsReloading;
                Motion.IsFiring = IsFiring;
                Motion.IsRecharging = IsRecharging();
            }

            if (inputManager.dropInput) Drop(Vector3.down * Inventory.dropForce, Vector3.up * Inventory.dropForce * 3, this);

            //fire mode input
            if (preset.useFire)
            {
                if (preset.fireMode == FireMode.Auto)
                {
                    IsFiring = remainingAmmoCount > 0 && !IsRecharging() && !FPSFrameworkUtility.IsPaused() ? fireInput : false;
                }

                if (preset.fireMode == FireMode.SemiAuto)
                {
                    fireInput = controls.Firearm.Fire.triggered;
                    IsFiring = remainingAmmoCount > 0 && !IsRecharging() && !FPSFrameworkUtility.IsPaused() ? fireInput : false;
                }

                if (preset.fireMode == FireMode.Selective)
                {
                    if (inputManager.fireModeSwitchInput)
                    {
                        autoFireMode = !autoFireMode;
                        if (autoFireMode) currentSelectiveFireMode = FireMode.Auto;
                        if (!autoFireMode) currentSelectiveFireMode = FireMode.SemiAuto;

                        events.OnFireModeChange?.Invoke();
                    }

                    if (currentSelectiveFireMode == FireMode.Auto)
                    {
                        IsFiring = remainingAmmoCount > 0 && !IsRecharging() && !FPSFrameworkUtility.IsPaused() ? fireInput : false;
                    }

                    if (currentSelectiveFireMode == FireMode.SemiAuto)
                    {
                        fireInput = controls.Firearm.Fire.triggered;
                        IsFiring = remainingAmmoCount > 0 && !IsRecharging() && !FPSFrameworkUtility.IsPaused() ? fireInput : false;
                    }
                }

                if (AmmoProfile != null)
                {
                    if (inputManager.reloadInput) Reload();
                    if (remainingAmmoCount <= 0 && fireInput && preset.automaticReload) Reload();
                }
            }

            if (Motion && Motion.Use_LowerdPos)
            {
                if (IsFiring || characterManager.characterInput.sprintInput || IsReloading)
                {
                    Motion.lowerdTimer = preset.lowerdDelay;
                }
                else if (Motion.lowerdTimer > 0)
                {
                    Motion.lowerdTimer -= Time.deltaTime;
                }

                if (Motion.lowerdTimer <= 0)
                {
                    Motion.IsLowerd = true;
                }
                else
                {
                    Motion.IsLowerd = false;
                }
            }
        }

        public void Fire()
        {
            if (_muzzle)
                Fire(_muzzle.forward);
        }

        /// <summary>
        ///  handles weapon shooting
        /// </summary>
        /// <param name="direction">what direction the projectile should be shot</param>
        public void Fire(Vector3 direction)
        {
            int unit = (int)preset.fireRateUnit;

            if (Time.time > fireTimer)
            {
                if (isLocallyMine && PauseMenu.Instance?.paused == true) return;

                fireTimer = Time.time + 1 / preset.fireRate * unit;
                if (preset.allowFireWhileRecharging || !preset.allowFireWhileRecharging && !IsRecharging())
                {
                    shots = 0;
                    FireDone(direction);

                    if (!preset.alwaysApplyFire)
                        ApplyFireOnce();
                }

                if (GamepadManager.Instance)
                    GamepadManager.Instance.BeginVibration(preset.gamepadVibrationAmountRight, preset.gamepadVibrationAmountLeft, preset.gamepadVibrationDuration);
            }
        }

        /// <summary>
        /// shoots a projectile without fire rate
        /// </summary>
        private void FireDone(Vector3 direction)
        {
            CancelInvoke(nameof(FireDone));

            foreach (ParticleSystem particle in VFX)
            {
                if (particle != rechargingVFX) particle.Play();
            }

            if (preset.shootingMechanism == ShootingMechanism.Projectiles)
            {
                if (preset.projectile)
                {
                    Projectile newProjectile = CreateProjectile(preset.projectile, this, _muzzle, GetSpread(_muzzle.forward), preset.muzzleVelocity, preset.range, projectilesParent.transform);
                }
                else
                {
                    Debug.LogError($"{preset.Name}'s projectile field is null, the firearm will not fire assign it and try again.");
                }
            }

            if (preset.shootingMechanism == ShootingMechanism.Hitscan)
            {
                Ray ray = new Ray(_muzzle.position, GetSpread(_muzzle.forward));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, preset.range, preset.hittableLayers))
                {
                    float damageRangeFactor = Mathf.InverseLerp(1, 0, (hit.distance / preset.range)) * (preset.damageRangeCurve.Evaluate(hit.distance / preset.range));
                    float damage = (!preset.alwaysApplyFire ? preset.damage / preset.shotCount : preset.damage) * damageRangeFactor;
                    Firearm.UpdateHits(this, null, preset.defaultDecal, ray, hit, damage, damageRangeFactor, preset.decalDirection);
                }
            }

            shots++;

            if (shots < preset.shotCount && remainingAmmoCount > 0)
            {
                if (preset.shotDelay <= 0)
                {
                    FireDone(direction);
                }
                else if (shots >= 1)
                    Invoke(nameof(FireDone), preset.shotDelay);
            }

            if (preset.alwaysApplyFire)
            {
                ApplyFireOnce();
            }
        }


        /// <summary>
        /// handles fire animation, audio, crosshair, etc..
        /// </summary>
        private void ApplyFireOnce()
        {
            if (preset.useRecoil) ApplyRecoil();
            remainingAmmoCount--;

            if (Animator) Animator.CrossFade("Fire", preset.fireTransition, 0, 0);

            CancelReload();

            characterManager?.cameraManager?.ShakeCameras(preset.cameraShake, preset.cameraShakeFadeOutTime);


            if (preset.useAudio && currentFireAudio)
            {
                fireAudio.Update(currentFireAudio);
                fireAudio.PlayOneShot(currentFireAudio);

                if (Motion && Motion.Use_Sprinting)
                {
                    Motion.afterShotRelief = true;

                    if (gameObject.activeSelf)
                        StartCoroutine(Motion.ResetSprint(preset.afterShotReliefTime));
                }

                fireTailAudio.Stop();
                fireTailAudio.PlayOneShot(preset.fireTail);
            }

            if (preset.rechargingType == RechargingType.GasPowerd)
                ThrowCasing();
        }

        public static void UpdateHits(Firearm firearm, Projectile projectile, GameObject defaultDecal, Ray ray, RaycastHit hit, float damage, float damageRangeFactor, Vector3Direction decalDirection)
        {
            //stop if object has ignore component
            if (hit.transform.TryGetComponent(out IgnoreHitDetection ignore)) return;

            //setup hit info for hit detection without directly editing on the code but using IHitable interface
            HitInfo hitInfo = new HitInfo(projectile, ray, hit);
            GameObject currentDecal = defaultDecal;

            if (firearm && firearm.characterManager != null && firearm.characterManager.gameObject == hit.transform) return;

            //Get on any hit interface
            IOnAnyHit onAnyHit = hit.transform.GetComponent<IOnAnyHit>();
            IOnAnyHitInChildren onAnyHitInChildren = hit.transform.GetComponent<IOnAnyHitInChildren>();
            IOnAnyHitInParent onAnyHitInParent = hit.transform.GetComponent<IOnAnyHitInParent>();

            //Get on hit interfaces
            IOnHit onHit = hit.transform.GetComponent<IOnHit>();
            IOnHitInChildren onHitInChildren = hit.transform.GetComponentInChildren<IOnHitInChildren>();
            IOnHitInParent onHitInParent = hit.transform.GetComponentInParent<IOnHitInParent>();


            //Call on hit
            if (onHit != null) onHit.OnHit(hitInfo);
            if (onHitInChildren != null && onHit == null) onHitInChildren.OnHitInChildren(hitInfo);
            if (onHitInParent != null && onHit == null) onHitInParent.OnHitInParent(hitInfo);

            //Call on any hits
            if (onAnyHit != null) onAnyHit.OnAnyHit(hitInfo);
            if (onAnyHitInChildren != null && onAnyHit == null) onAnyHitInChildren.OnAnyHitInChildren(hitInfo);
            if (onAnyHitInParent != null && onAnyHit == null) onAnyHitInParent.OnAnyHitInParent(hitInfo);

            if (hit.transform.TryGetComponent(out Explosive _explosive))
            {
                _explosive.source = firearm.Actor;
            }

            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                float damageResult = damage * damageRangeFactor * firearm.attachmentsManager.damage;

                Actor sourceActor = null;

                if (firearm.Actor) sourceActor = firearm.Actor;
                damageable.Damage(damageResult, sourceActor);

                bool highDamage = damageable.GetHealth() <= damageable.MaxHealth * 0.3f ? true : false;

                if (firearm.isLocallyMine)
                    UIManager.Instance?.Hitmarker?.Enable(highDamage, true);
            }

            if (hit.transform.TryGetComponent(out IDamageableGroup damageableGroup))
            {
                bool isAlive = damageableGroup.GetDamageable() != null ? !damageableGroup.GetDamageable().IsDead() : false;
                if (isAlive)
                {
                    Transform controllerTransform = null;
                    if (firearm && firearm.characterManager != null) controllerTransform = firearm.characterManager.transform;
                    if (hit.transform != controllerTransform)
                    {
                        float damageResult = damage * damageRangeFactor * damageableGroup.GetDamageMultipler() * firearm.attachmentsManager.damage;

                        damageableGroup.GetDamageable()?.Damage(damageResult, firearm.Actor);

                        float force = firearm.preset.alwaysApplyFire ?
                            firearm.preset.impactForce / firearm.preset.shotCount : firearm.preset.impactForce;
                        damageableGroup.GetDamageable().deathForce = -hit.normal * force;

                        if (firearm.isLocallyMine)
                            UIManager.Instance?.Hitmarker?.Enable();
                    }
                }
            }

            #region Verify Kill
            if (damageableGroup != null && damageable == null)
            {
                if (damageableGroup.GetDamageable() != null)
                {
                    if (!damageableGroup.GetDamageable().deadConfirmed && damageableGroup.GetDamageable().IsDead())
                    {
                        if (firearm && firearm.Actor)
                            firearm?.Actor?.onKill?.Invoke(damageableGroup.GetDamageable().GetActor(), damageableGroup);

                        damageableGroup.GetDamageable().deadConfirmed = true;
                    }
                }
            }
            #endregion


            if (hit.transform.TryGetComponent(out CustomDecal customDecal))
            {
                currentDecal = customDecal.decalVFX;
            }

            if (firearm && firearm.characterManager && hit.transform == firearm?.characterManager?.transform) return;

            if (currentDecal)
            {
                Vector3 position = hit.point;
                Quaternion rotation = MathUtilities.GetFromToRotation(hit, decalDirection);

                GameObject impact = Instantiate(currentDecal, position, rotation);
                impact.transform.localScale *= firearm.preset.decalSize;
                impact.transform.SetParent(hit.transform);

                float lifeTime = customDecal ? customDecal.lifeTime : 60;
                Destroy(impact, lifeTime);
            }


            if (hit.rigidbody)
            {
                float force = firearm.preset.shotDelay <= 0 ? firearm.preset.impactForce / firearm.preset.shotCount : firearm.preset.impactForce;
                hit.rigidbody.AddForceAtPosition(-hit.normal * force, hit.point, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Spwans a casing
        /// </summary>
        public void ThrowCasing()
        {
            if (!preset.casing || !casingEjectionPort) return;

            Rigidbody newCasing = Instantiate(preset.casing, casingEjectionPort.position, casingEjectionPort.rotation, casingsParent.transform).GetComponent<Rigidbody>();

            Vector3 shooterVelocity = Vector3.zero;
            Vector3 casingVelocity = Vector3.zero;

            if (Inventory && Inventory.characterManager != null && characterManager.characterController) shooterVelocity = characterManager.velocity;
            casingVelocity = transform.GetDirection(preset.casingDirection) * preset.ejectionVelocity * Random.Range(0.6f, 1);
            casingVelocity += shooterVelocity;

            newCasing.AddForce(casingVelocity, ForceMode.VelocityChange);
            newCasing.transform.Rotate(Random.Range(-preset.casingRotationFactor, preset.casingRotationFactor), Random.Range(-preset.casingRotationFactor, preset.casingRotationFactor), Random.Range(-preset.casingRotationFactor, preset.casingRotationFactor));
            Destroy(newCasing.gameObject, 5);

            if (rechargingVFX) rechargingVFX.Play();
        }

        /// <summary>
        /// Applies force to the weapon & camera
        /// </summary>
        private void ApplyRecoil()
        {
            //Recoil while not aiming
            if (inputManager && !inputManager.aimInput)
            {
                currentRecoilPosition += attachmentsManager.visualRecoil * preset.staticRecoilPosition + new Vector3(Random.Range(-preset.recoilPosition.x, preset.recoilPosition.x), Random.Range(-preset.recoilPosition.y, preset.recoilPosition.y), Random.Range(-preset.recoilPosition.z, preset.recoilPosition.z));
                currentRecoilRotation += attachmentsManager.visualRecoil * preset.staticRecoilRotation + new Vector3(preset.recoilRotation.x, Random.Range(-preset.recoilRotation.y, preset.recoilRotation.y), Random.Range(-preset.recoilRotation.z, preset.recoilRotation.z));

            }

            //Recoil while aiming
            if (inputManager && inputManager.aimInput || !inputManager)
            {
                currentRecoilPosition += attachmentsManager.visualRecoil * preset.staticRecoilPositionAim + new Vector3(Random.Range(-preset.recoilPositionAim.x, preset.recoilPositionAim.x), Random.Range(-preset.recoilPositionAim.y, preset.recoilPositionAim.y), Random.Range(-preset.recoilPositionAim.z, preset.recoilPositionAim.z));
                currentRecoilRotation += attachmentsManager.visualRecoil * preset.staticRecoilRotationAim + new Vector3(preset.recoilRotationAim.x, Random.Range(-preset.recoilRotationAim.y, preset.recoilRotationAim.y));
            }

            if (characterManager)
                characterManager.cameraManager.ApplyRecoil(preset.verticalRecoil * attachmentsManager.recoil, preset.horizontalRecoil * attachmentsManager.recoil, preset.cameraRecoil * attachmentsManager.recoil, inputManager.aimInput);
        }


        /// <summary>
        /// Stops reload process
        /// </summary>
        public void CancelReload()
        {
            if (IsReloading)
            {
                CancelInvoke(nameof(ApplyReload));
                IsReloading = false;

                if (preset.reloadAudio) reloadAudio.Stop();
                if (preset.reloadEmptyAudio) reloadEmptyAudio.Stop();
            }
        }

        /// <summary>
        /// Handles delayed reloads
        /// </summary>
        public void Reload()
        {
            if (remainingAmmoCount >= magazineCapacity || remainingAmmoTypeCount <= 0 || IsReloading) return;

            if (preset.reloadAudio) reloadAudio.Stop();
            if (preset.reloadEmptyAudio) reloadEmptyAudio.Stop();

            if (AmmoProfile.amount > 0)
            {
                if (GetComponentInChildren<WeaponEvents>())
                {
                    GetComponentInChildren<WeaponEvents>().MagThrown = false;
                }

                IsReloading = true;
                events.OnReload?.Invoke();
                StartReload();

                if (preset && preset.reloadMethod == ReloadType.Default)
                {
                    if (preset.reloadAudio && remainingAmmoCount <= 0) reloadEmptyAudio.Play(preset.reloadEmptyAudio);
                    else if (preset.reloadEmptyAudio) reloadAudio.Play(preset.reloadAudio);
                }
            }
            else
            {
                ResetReload();
            }
        }

        private void StartReload()
        {

            if (preset.reloadMethod == ReloadType.Scripted)
            {
                Animator.CrossFade(preset.reloadStateName, preset.reloadTransitionTime, 0, 0);
                IsReloading = true;
            }
            else
            {
                Invoke(nameof(ApplyReload), remainingAmmoCount <= 0 ? preset.emptyReloadTime : preset.reloadTime);
            }


        }

        /// <summary>
        /// handles ammo reserve and inventory ammo
        /// </summary>
        public void ApplyReload()
        {
            if (AmmoProfile.amount <= 0)
            {
                return;
            }

            int current;
            int result;

            if (preset.reloadMethod == ReloadType.Default)
            {
                current = magazineCapacity - remainingAmmoCount;
                result = AmmoProfile.amount >= current ? current : AmmoProfile.amount;

                if (AmmoProfile.data.Name != "No Ammo Data") AmmoProfile.amount -= result;
                remainingAmmoCount += result;
            }

            ResetReload();
        }

        /// <summary>
        /// adds (amount) to reserve and removes (amount) from inventory
        /// </summary>
        /// <param name="amount">amount of ammo to add</param>
        public void ApplyReloadOnce(int amount = 1)
        {
            if (AmmoProfile.amount <= 0)
            {
                ResetReload();
                return;
            }

            AmmoProfile.amount -= amount;
            remainingAmmoCount += amount;

            AmmoProfile.amount = Mathf.Clamp(AmmoProfile.amount, 0, int.MaxValue);

            reloadAudio.Stop();
            reloadEmptyAudio.Stop();

            reloadAudio.Equip(characterManager.gameObject, preset.reloadAudio);
            reloadAudio.Play(preset.reloadAudio);
        }

        /// <summary>
        /// resets is reloading to false
        /// </summary>
        public void ResetReload()
        {
            IsReloading = false;
        }

        /// <summary>
        /// returns random direction from muzzle to muzzle forward
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 GetSpread(Vector3 direction)
        {
            Vector3 targetSpread = Vector3.zero;

            if (Motion)
                targetSpread = !inputManager.aimInput ? sprayPattern.GetPattern(this, direction) : aimSprayPattern.GetPattern(this, direction);
            else
                targetSpread = sprayPattern.GetPattern(this, direction);

            return targetSpread;
        }

        private void OnDestroy()
        {
            if (Inventory && Inventory.characterManager != null)
            {
                Inventory.characterManager.character.ResetSpeed();
            }
        }

        private void OnEnable()
        {
            ResetStates();
            CancelReload();

            if (preset)
            {
                if (preset.fire) fireAudio.Equip(gameObject, preset.fire);
                if (preset.fireTail) fireTailAudio.Equip(gameObject, preset.fireTail);
                if (preset.reloadAudio) reloadAudio.Equip(gameObject, preset.reloadAudio);
                if (preset.reloadEmptyAudio) reloadEmptyAudio.Equip(gameObject, preset.reloadEmptyAudio);
            }
        }

        private void OnDisable()
        {
            CancelReload();


            if (Inventory && Inventory.characterManager != null)
            {
                Inventory.characterManager.character.ResetSpeed();
            }

            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                Destroy(source);
            }
        }

        /// returns true if recharging animation is playing
        /// </summary>
        /// <returns></returns>
        public bool IsRecharging()
        {
            return Animator ? Animator.GetCurrentAnimatorStateInfo(0).IsName(preset.rechargingStateName) : false;
        }

        public float FiringAmount
        {
            get
            {
                return IsFiring ? 1 : Mathf.Lerp(1, 0, Time.deltaTime * 10);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Setup/Animator parameter")]
        public void AddMissingAnimatorParameters()
        {
            //get the target animator
            Animator animator = GetComponentInChildren<Animator>();

            if (!animator)
            {
                Debug.LogError($"Can't find animator in {gameObject.name}'s children make sure to add animator and assgin animator controller.");
                return;
            }

            AddParameter("Is Reloading", AnimatorControllerParameterType.Bool, false);
            AddParameter("Sprint Amount", AnimatorControllerParameterType.Float, false);
            AddParameter("ADS Amount", AnimatorControllerParameterType.Float, false);
            AddParameter("Ammo", AnimatorControllerParameterType.Int, false);
        }

        protected void AddParameter(string name, AnimatorControllerParameterType type, bool overwrite)
        {
            //get the target animator
            Animator animator = GetComponentInChildren<Animator>();

            //check if animator controller has state with the name given
            if (!overwrite && HasParameter(name, animator))
            {
                //if overwrite is false and animator controller has paremator don't continue and print a message to the consle
                Debug.Log($"Animator on {animator.gameObject.name} already has Parameter with the name ({name}).");
                return;
            }

            //get animator controller
            UnityEditor.Animations.AnimatorController animatorController = (UnityEditor.Animations.AnimatorController)animator.runtimeAnimatorController;

            // upate name and type
            AnimatorControllerParameter parameter = new AnimatorControllerParameter();
            parameter.type = type;
            parameter.name = name;

            //add the parameter
            animatorController.AddParameter(parameter);
        }

        public static bool HasParameter(string paramName, Animator animator)
        {
            //loop through every parameter and if animator has parameter return true if not return false
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                //check if parameter name matchs given name
                if (param.name == paramName)
                    return true;
            }
            return false;
        }
#endif
    }
}