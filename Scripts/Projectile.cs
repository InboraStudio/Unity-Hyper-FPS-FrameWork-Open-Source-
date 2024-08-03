using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Projectile")]
    public class Projectile : MonoBehaviour
    {
        [Header("Base Settings")]
        public LayerMask hittableLayers = -1;
        public Vector3Direction decalDirection = Vector3Direction.forward;
        public float penetrationStrenght = 100;
        public float speed = 50;
        public float gravity = 1;
        public float force = 10;
        public int lifeTime = 5;
        public GameObject defaultDecal;

        [Header("Additional Settings")]
        public bool destroyOnImpact = false;
        public bool useSourceVelocity = true;
        public bool useAutoScaling = true;
        public float scaleMultipler = 45;

        [Header("Range Control")]
        public float range = 300;
        public AnimationCurve damageRangeCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0.3f) });


        public Firearm source { get; set; }
        public Vector3 direction { get; set; }
        public Vector3 shooterVelocity { get; set; }
        private float damage;
        private float damageRangeFactor;
        private float maxVelocity;
        private Vector3 velocity;
        private TrailRenderer trail;
        private Explosive explosive;

        private Rigidbody rb;

        private Vector3 previousPosition;

        private Transform Effects;

        private Vector3 startPosition;

        /// <summary>
        /// returns true if the shooter has any component with the interface ICharacterController implemented
        /// </summary>
        public bool isLocallyMine
        {
            get
            {
                return source?.characterManager?.character != null;
            }
        }

        private void Awake()
        {
            Setup();
        }

        public virtual void Setup()
        {
            previousPosition = transform.position;
            startPosition = transform.position;

            transform.localScale = useAutoScaling ? Vector3.zero : Vector3.one * scaleMultipler;

            FindComponents();

            if (trail && useAutoScaling) trail.widthMultiplier = 0;
        }

        public virtual void FindComponents()
        {
            explosive = GetComponent<Explosive>();
            trail = GetComponentInChildren<TrailRenderer>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Vector3 sorceVelocity = useSourceVelocity ? shooterVelocity : Vector3.zero;

            velocity = (transform.forward + direction) * (speed / 2) + sorceVelocity;

            rb.AddForce(velocity, ForceMode.VelocityChange);

            maxVelocity = source.preset.muzzleVelocity;

            if (transform.Find("Effects"))
            {
                Effects = transform.Find("Effects");
                Effects.parent = null;
                Destroy(gameObject, lifeTime + 1);
            }
            if (explosive) explosive.source = source.Actor;

            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            float distanceFromStartPosition = Vector3.Distance(startPosition, transform.position);
            distanceFromStartPosition = Mathf.Clamp(distanceFromStartPosition, 0, range);

            damageRangeFactor = (rb.velocity.magnitude / maxVelocity) * (damageRangeCurve.Evaluate(distanceFromStartPosition / range));
            damage = (!source.preset.alwaysApplyFire ? source.preset.damage / source.preset.shotCount : source.preset.damage) * damageRangeFactor;

            Ray ray = new Ray(previousPosition, -(previousPosition - transform.position));
            RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, previousPosition));
            if (penetrationStrenght <= 0) Destroy(gameObject);

            for (int i = 0; i < hits.Length; i++)
            {
                if (penetrationStrenght > 0)
                {
                    RaycastHit hit = hits[i];
                    UpdateHits(ray, hit);
                }
            }

            if (useAutoScaling)
            {
                float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                float scale = (distance / scaleMultipler) * (Camera.main.fieldOfView / 360);

                transform.localScale = Vector3.one * scale;
                if (trail) trail.widthMultiplier = scale;
            }

            if (!useAutoScaling)
            {
                transform.localScale = Vector3.one * scaleMultipler;
            }

            if (Effects)
            {
                Effects.position = transform.position;
            }
        }

        private void FixedUpdate()
        {
            rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }

        private void LateUpdate()
        {
            previousPosition = transform.position;
        }

        private void UpdateHits(Ray ray, RaycastHit hit)
        {
            //stop if object has ignore component
            if (hit.transform.TryGetComponent(out IgnoreHitDetection ignore)) return;
            OnHit(hit);

            if (explosive)
            {
                explosive.Explode(true);
                Destroy(gameObject);
                return;
            }

            if (hit.transform.TryGetComponent(out CustomDecal customDecal))
            {
                penetrationStrenght -= customDecal.materialStrenght;
            }
            else
            {
                penetrationStrenght -= 10;
            }

            if (penetrationStrenght > 0)
                Firearm.UpdateHits(source, this, defaultDecal, ray, hit, damage, damageRangeFactor, decalDirection);
            else
                Destroy(gameObject);

        }

        public virtual void OnHit(RaycastHit hit)
        {

        }

        private void OnDestroy()
        {
            source?.Projectiles?.Remove(this);
        }
    }
}