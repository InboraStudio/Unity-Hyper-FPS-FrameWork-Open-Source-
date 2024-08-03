using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Melee")]
    public class MeleeWeapon : Weapon
    {
        public float range = 5;
        public float damage;
        public float impactForce = 1;
        public float decalSize = 1;
        public Vector3Direction decalWorldToPoint;
        public GameObject decal;

        private RaycastHit hit;


        private void Start()
        {
            Inventory = GetComponentInParent<Inventory>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

        /// <summary>
        /// handles damage for melee weapons
        /// </summary>
        public void Attack()
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                Transform transform = hit.transform;

                if (transform == Inventory.characterManager.transform) return;


                CustomDecal customDecal = transform.GetComponent<CustomDecal>();

                Vector3 decalPosition = hit.point;
                Quaternion decalRotation = GetDecalFromToRotation(hit);

                if (transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(damage, Inventory.characterManager.actor);

                    UIManager.Instance.Hitmarker.Enable(false, true);
                }

                if (transform.TryGetComponent(out IDamageableGroup damageableGroup))
                {
                    damageableGroup.GetDamageable().Damage(damage, Inventory.characterManager.actor);

                    UIManager.Instance.Hitmarker.Enable(false, true);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(-transform.forward * impactForce, transform.position, ForceMode.Impulse);
                }

                if (damageableGroup != null && damageable == null)
                {
                    if (!damageableGroup.GetDamageable().deadConfirmed && damageableGroup.GetDamageable().IsDead())
                    {
                        Inventory.characterManager.actor.actorManager.kills++;
                        UIManager.Instance.KillFeed.Show(Inventory.characterManager.actor, damageableGroup.GetDamageable().GetActor().actorName, damageableGroup.GetBone() == HumanBodyBones.Head ? true : false);
                        damageableGroup.GetDamageable().deadConfirmed = true;
                    }
                }

                if (decal)
                {
                    GameObject currentDecal = customDecal && customDecal.decalVFX ? customDecal.decalVFX : decal;
                    GameObject impact = Instantiate(currentDecal, decalPosition, decalRotation);
                    impact.transform.parent = hit.transform;
                    impact.transform.localScale *= decalSize;

                    Destroy(impact, decal ? customDecal.lifeTime : 60);
                }
            }
        }

        public Quaternion GetDecalFromToRotation(RaycastHit _hit)
        {
            Quaternion rot = new Quaternion();

            switch (decalWorldToPoint)
            {
                case Vector3Direction.forward:
                    rot = Quaternion.FromToRotation(Vector3.forward, _hit.normal);
                    break;

                case Vector3Direction.back:
                    rot = Quaternion.FromToRotation(Vector3.back, _hit.normal);
                    break;

                case Vector3Direction.right:
                    rot = Quaternion.FromToRotation(Vector3.right, _hit.normal);
                    break;

                case Vector3Direction.left:
                    rot = Quaternion.FromToRotation(Vector3.left, _hit.normal);
                    break;

                case Vector3Direction.up:
                    rot = Quaternion.FromToRotation(Vector3.up, _hit.normal);
                    break;

                case Vector3Direction.down:
                    rot = Quaternion.FromToRotation(Vector3.down, _hit.normal);

                    break;
            }

            return rot;
        }
    }
}