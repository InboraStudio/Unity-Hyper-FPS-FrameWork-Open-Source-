using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Explosive")]
    [RequireComponent(typeof(Rigidbody))]
    public class Explosive : MonoBehaviour, IDamageable
    {
        [Header("Base")]
        [HideInInspector] public ExplosionType type = ExplosionType.RayTracking;
        [HideInInspector] public LayerMask layerMask = -1;
        [HideInInspector] public float radius = 10;
        [HideInInspector] public float effectRadius = 20;
        [HideInInspector] public float damage = 150;
        [HideInInspector] public float force = 7;
        [HideInInspector] public float delay = 5;
        [HideInInspector] public float friction = 1;

        [Header("Extras")]
        [HideInInspector] public bool ignoreGlobalScale = false;
        [HideInInspector] public bool sticky = false;
        [HideInInspector] public bool damageable = false;
        [HideInInspector] public float health = 25;
        [HideInInspector] public bool exlopeAfterDelay;
        [HideInInspector] public bool destroyOnExplode = true;
        [HideInInspector] public float clearDelay = 60;

        [Header("VFX")]
        [HideInInspector] public GameObject explosion;
        [HideInInspector] public GameObject craterDecal;
        public GameObject explosionEffect;
        public float explosionEffectForce = 1;
        [HideInInspector] public Vector3 explosionEffactOffcet;
        [HideInInspector] public Vector3 explosionEffactRotationOffset;

        [Space]
        [HideInInspector] public float explosionSize = 1;
        [HideInInspector] public float craterSize = 1;
        [HideInInspector] public float cameraShake = 1;

        [Header("Audio")]
        [HideInInspector] public bool audioLowPassFilter;
        [HideInInspector] public float lowPassCutoffFrequency = 1500;
        [HideInInspector] public float lowPassTime = 2f;
        [HideInInspector] public float lowPassSmoothness = 0.1f;

        [Header("Debug")]
        [HideInInspector] public bool debug;
        [HideInInspector] public bool ranges;
        [HideInInspector] public bool rays;

        public Actor source { get; set; }
        private Rigidbody rb;

        public bool exploded { get; set; }
        public bool deadConfirmed { get; set; }
        public Vector3 deathForce { get; set; }
        public float MaxHealth { get; set; }
        private float scale { get { return ignoreGlobalScale ? 1 : transform.lossyScale.magnitude; } }

        private void Start()
        {
            MaxHealth = health;
            if (exlopeAfterDelay) Explode(delay);
            
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (IsDead()) Explode();
        }

        public void Explode(float delay)
        {
            Invoke(nameof(DoExplode), delay);
        }

        //just to use invoke
        private void DoExplode() => Explode(true);

        public void AddEffects()
        {
            if (explosion)
            {
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;

                pos = transform.position + explosionEffactOffcet;
                rot = transform.rotation * Quaternion.Euler(explosionEffactRotationOffset);

                GameObject newExplosion = Instantiate(explosion, pos, rot);
                newExplosion.transform.localScale *= explosionSize;
                Destroy(newExplosion, clearDelay);
            }

            if (craterDecal)
            {
                RaycastHit ray;
                if (Physics.Raycast(transform.position, Vector3.down, out ray, radius * scale))
                {
                    GameObject newDecal = Instantiate(craterDecal, ray.point + Vector3.up * 0.01f, Quaternion.FromToRotation(Vector3.up, ray.normal));
                    newDecal.transform.localScale *= craterSize;
                    Destroy(newDecal, clearDelay);
                }
            }

            if (explosionEffect)
            {
                GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
                effect.SetActive(true);

                foreach (Rigidbody rb in effect.GetComponentsInChildren<Rigidbody>())
                {
                    rb.AddExplosionForce(explosionEffectForce, transform.position, radius * scale, 1, ForceMode.VelocityChange);
                }

                Destroy(effect, clearDelay);
            }
        }

        /// <summary>
        /// Tries to explode this explosive
        /// </summary>
        /// <param name="addEffects">If true the explosion will result in a crater on the ground and other effects.</param>
        public void Explode(bool addEffects = true)
        {
            if (exploded) return;
            exploded = true;


            Collider[] nearColliders = Physics.OverlapSphere(transform.position, radius * scale, layerMask);
            Collider[] farColliders = Physics.OverlapSphere(transform.position, effectRadius * scale, layerMask);

            foreach (Collider collider in nearColliders)
            {
                var dir = -(transform.position - collider.transform.position);

                switch (type)
                {
                    case ExplosionType.RayTracking:
                        if (Physics.Raycast(transform.position, dir, out RaycastHit hit))
                        {
                            ApplyExplosion(hit.transform, dir);
                            CallRayTrackingCallbacks(hit);
                        }
                        break;

                    case ExplosionType.Standard:
                        ApplyExplosion(collider.transform, dir);
                        CallStandardCallbacks(collider);
                        break;
                }
            }

            if(addEffects)
            AddEffects();

            foreach (Collider collider in farColliders)
            {
                if (collider.transform.TryGetComponent(out CharacterManager controller))
                {
                    float distanceFromPlayer = Vector3.Distance(transform.position, controller.transform.position);

                    float percentage = distanceFromPlayer / effectRadius * scale;

                    float inversedPercentage = Mathf.InverseLerp(1, 0, percentage);

                    if (distanceFromPlayer <= radius * scale) percentage = 1;

                    controller.cameraManager.ShakeCameras(cameraShake * inversedPercentage);

                    if (GamepadManager.Instance) GamepadManager.Instance.BeginVibration(1, 1, 0.8f);
                    
                    if (audioLowPassFilter)
                    {
                        controller.audioFiltersManager.SetLowPass(lowPassCutoffFrequency * distanceFromPlayer / effectRadius * scale, 1000 * Time.deltaTime);
                        controller.audioFiltersManager.ResetLowPass(lowPassSmoothness * Time.deltaTime, lowPassTime);
                    }
                }
            }

            if (destroyOnExplode) Destroy(gameObject);
        }

        private void CallStandardCallbacks(Collider collider)
        {
            HitInfo hitInfo = null;

            //Get on any hit interface
            IOnAnyHit onAnyHit = collider.transform.GetComponent<IOnAnyHit>();
            IOnAnyHitInChildren onAnyHitInChildren = collider.transform.GetComponent<IOnAnyHitInChildren>();
            IOnAnyHitInParent onAnyHitInParent = collider.transform.GetComponent<IOnAnyHitInParent>();

            //calls OnHit() for any object with IHitable interface
            IOnExplosionHit onHit = collider.transform.GetComponent<IOnExplosionHit>();
            IOnExplosionHitInChildren onHitInChildren = collider.transform.GetComponentInChildren<IOnExplosionHitInChildren>();
            IOnExplosionHitInParent onHitInParent = collider.transform.GetComponentInParent<IOnExplosionHitInParent>();

            //calls OnHit() for any object with IHitable interface
            IOnRayTrackingExplosionHit rayTrackingOnHit = collider.transform.GetComponent<IOnRayTrackingExplosionHit>();
            IOnRayTrackingExplosionHitInChildren rayTrackingOnHitInChildren = collider.transform.GetComponentInChildren<IOnRayTrackingExplosionHitInChildren>();
            IOnRayTrackingExplosionHitInParent rayTrackingOnHitInParent = collider.transform.GetComponentInParent<IOnRayTrackingExplosionHitInParent>();

            //Call on any hits
            if (onAnyHit != null) onAnyHit.OnAnyHit(hitInfo);
            if (onAnyHitInChildren != null && onAnyHit == null) onAnyHitInChildren.OnAnyHitInChildren(hitInfo);
            if (onAnyHitInParent != null && onAnyHit == null) onAnyHitInParent.OnAnyHitInParent(hitInfo);

            //Call on hit
            if (onHit != null) onHit.OnExplosionHit(hitInfo);
            if (onHitInChildren != null && onHit == null) onHitInChildren.OnExplosionHitInChildren(hitInfo);
            if (onHitInParent != null && onHit == null) onHitInParent.OnExplosionHitInParent(hitInfo);

            //Call on ray tracking hit
            if (rayTrackingOnHit != null) rayTrackingOnHit.OnRayTrackingExplosionHit(hitInfo);
            if (rayTrackingOnHitInChildren != null && rayTrackingOnHit == null) rayTrackingOnHitInChildren.OnRayTrackingExplosionHitInChildren(hitInfo);
            if (rayTrackingOnHitInParent != null && rayTrackingOnHit == null) rayTrackingOnHitInParent.OnRayTrackingExplosionHitInParent(hitInfo);
        }

        private void CallRayTrackingCallbacks(RaycastHit hit)
        {
            HitInfo hitInfo = null;

            //Get on any hit interface
            IOnAnyHit onAnyHit = hit.transform.GetComponent<IOnAnyHit>();
            IOnAnyHitInChildren onAnyHitInChildren = hit.transform.GetComponent<IOnAnyHitInChildren>();
            IOnAnyHitInParent onAnyHitInParent = hit.transform.GetComponent<IOnAnyHitInParent>();

            //calls OnHit() for any object with IHitable interface
            IOnExplosionHit onHit = hit.transform.GetComponent<IOnExplosionHit>();
            IOnExplosionHitInChildren onHitInChildren = hit.transform.GetComponentInChildren<IOnExplosionHitInChildren>();
            IOnExplosionHitInParent onHitInParent = hit.transform.GetComponentInParent<IOnExplosionHitInParent>();

            //calls OnHit() for any object with IHitable interface
            IOnStandardExplosionHit standardOnHit = hit.transform.GetComponent<IOnStandardExplosionHit>();
            IOnStandardExplosionHitInChildren standardOnHitInChildren = hit.transform.GetComponentInChildren<IOnStandardExplosionHitInChildren>();
            IOnStandardExplosionHitInParent standardOnHitInParent = hit.transform.GetComponentInParent<IOnStandardExplosionHitInParent>();

            //Call on any hits
            if (onAnyHit != null) onAnyHit.OnAnyHit(hitInfo);
            if (onAnyHitInChildren != null && onAnyHit == null) onAnyHitInChildren.OnAnyHitInChildren(hitInfo);
            if (onAnyHitInParent != null && onAnyHit == null) onAnyHitInParent.OnAnyHitInParent(hitInfo);

            //Call on hit
            if (onHit != null) onHit.OnExplosionHit(hitInfo);
            if (onHitInChildren != null && onHit == null) onHitInChildren.OnExplosionHitInChildren(hitInfo);
            if (onHitInParent != null && onHit == null) onHitInParent.OnExplosionHitInParent(hitInfo);

            //Call on standard hit
            if (standardOnHit != null) standardOnHit.OnStandardExplosionHit(hitInfo);
            if (standardOnHitInChildren != null && standardOnHit == null) standardOnHitInChildren.OnStandardExplosionHitInChildren(hitInfo);
            if (standardOnHitInParent != null && standardOnHit == null) standardOnHitInParent.OnStandardExplosionHitInParent(hitInfo);
        }

        public void ApplyExplosion(Transform _transform, Vector3 dir)
        {
            if (_transform != transform)
            {
                if (_transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(damage, source);
                    damageable.deathForce = dir * force;
                }

                if (_transform.TryGetComponent(out IDamageableGroup damageableGroup))
                {
                    float lenght = damageableGroup.GetDamageable().GetGroupsCount() > 0 ? damageableGroup.GetDamageable().GetGroupsCount() : 1;
                    damageableGroup.GetDamageable().Damage((damage / lenght), source);

                    if (!damageableGroup.GetDamageable().IsDead())
                        UIManager.Instance.Hitmarker.Enable(false, false);

                    if (source && !damageableGroup.GetDamageable().deadConfirmed && damageableGroup.GetDamageable().IsDead())
                    {
                        source?.actorManager?.actor?.onKill?.Invoke(damageableGroup.GetDamageable().GetActor(), damageableGroup);
                        damageableGroup.GetDamageable().deadConfirmed = true;
                    }

                    foreach (Rigidbody ragdollRB in damageableGroup.GetDamageable().GetRagdoll().rigidbodies) 
                        ragdollRB.AddExplosionForce(force / lenght, transform.position, radius * scale, 1, ForceMode.VelocityChange);
                }

                ShootingSheet _shootingSheet = transform.SearchFor<ShootingSheet>();
                Door _door = _transform.SearchFor<Door>();

                if (_shootingSheet) _shootingSheet.Enable();
                if (_door) _door.Interact(transform);

                if (_transform.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(force, transform.position, radius * scale, 1, ForceMode.VelocityChange);
                }
            }
        }

        private void ApplyFriction()
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;

            float coefficientOfFriction = friction / 100;

            rb.AddForce(-velocity * coefficientOfFriction, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (sticky)
            {
                rb.isKinematic = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (friction > 0) ApplyFriction();
        }

        private void OnDrawGizmosSelected()
        {
            if (!debug) return;

            if (ranges)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, radius * scale);

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, effectRadius * scale);
            }

            if (rays && type == ExplosionType.RayTracking)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius * scale, layerMask);
                foreach (Collider collider in colliders)
                {
                    RaycastHit hit;
                    var dir = -(transform.position - collider.transform.position);
                    if (Physics.Raycast(transform.position, dir, out hit))
                    {
                        if (hit.transform == transform) return;
                        bool hasDamageble = hit.transform.TryGetComponent(out IDamageable damageable) && collider.transform.GetComponentInChildren<IDamageableGroup>() == null;
                        bool hadDamageableGroup = hit.transform.TryGetComponent(out IDamageableGroup damageableGroup);
                        bool hasRigidbody = hit.transform.TryGetComponent(out Rigidbody rb);

                        if (hasDamageble || hadDamageableGroup || hasRigidbody)
                        {
                            Gizmos.color = Color.white;
                            Gizmos.DrawLine(transform.position, hit.point);
                            Gizmos.DrawSphere(hit.point, 0.1f * transform.lossyScale.magnitude);
                        }
                    }
                }

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, new Vector3(0, -radius * scale, 0) * transform.lossyScale.magnitude);

            }

            if (rays && type == ExplosionType.Standard)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius * scale, layerMask);
                foreach (Collider collider in colliders)
                {
                    bool hasDamageble = collider.transform.TryGetComponent(out IDamageable damageable) && collider.transform.GetComponentInChildren<IDamageableGroup>() == null;
                    bool hadDamageableGroup = collider.transform.TryGetComponent(out IDamageableGroup damageableGroup);
                    bool hasRigidbody = collider.transform.TryGetComponent(out Rigidbody rb);

                    if (hasDamageble || hadDamageableGroup || hasRigidbody)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(transform.position, collider.transform.position);
                        Gizmos.DrawSphere(collider.transform.position, 0.1f * transform.lossyScale.magnitude);
                    }
                }

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, new Vector3(0, -radius * scale, 0) * transform.lossyScale.magnitude);

            }
        }

        public Actor GetActor()
        {
            return source;
        }

        public float GetHealth()
        {
            return health;
        }
        
        public void Damage(float amount, Actor damageSource)
        {
            if(!damageable) return;

            health -= amount;
            source = damageSource;
        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public int GetGroupsCount()
        {
            return 0;
        }

        public Ragdoll GetRagdoll()
        {
            return null;
        }
    }

    public enum ExplosionType
    {
        Standard,
        RayTracking
    }
}