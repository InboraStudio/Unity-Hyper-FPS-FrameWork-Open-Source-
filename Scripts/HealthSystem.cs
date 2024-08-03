using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Health System/Health System")]
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        public HealthType type = HealthType.Other;
        public float health = 100;
        public float destroyDelay;
        [Range(0, 1)] public float damageCameraShake = 0.3f;

        [Space]
        public bool destoryOnDeath;
        public bool destroyRoot;
        public bool ragdolls;
        public GameObject deathEffect;

        [Space]
        public UnityEvent OnDeath;

        public Actor Actor { get; set; }
        public Ragdoll ragdoll { get; set; }
        private Actor killer;
        private DeathCamera deathCamera;
        public Vector3 deathForce { get; set; }
        public float MaxHealth { get; set; }
        public IDamageableGroup[] groups { get; set; }
        private bool died;
        public bool deadConfirmed { get; set; }

        private void Start()
        {
            Actor = GetComponent<Actor>();
            ragdoll = GetComponent<Ragdoll>();

            OnDeath.AddListener(Die);
            if (FindObjectOfType<GameManager>()) deathCamera = FindObjectOfType<GameManager>().DeathCamera;

            MaxHealth = health;

            if (type == HealthType.Humanoid)
            {
                if (Actor && Actor.characterManager != null) deathCamera?.Disable();

                groups = GetComponentsInChildren<IDamageableGroup>();

                if (Actor && Actor.characterManager != null)
                {
                    if (UIManager.Instance && UIManager.Instance.HealthDisplay && Actor)
                    {
                        UIManager.Instance.HealthDisplay?.UpdateCard(GetHealth(), Actor.actorName, false);
                        UIManager.Instance.HealthDisplay.actorNameText.text = Actor.actorName;
                    }
                }
            }

            if(type == HealthType.Other)
            {
                if (ragdoll || Actor) Debug.LogWarning($"{this} has humanoid components and it's type is Other please change type to Humanoid to avoid errors.");
            }
        }

        private void UpdateSystem()
        {
            if (!died && health <= 0)
            {
                OnDeath?.Invoke();
            }

            UpdateUI(1);
        }

        public void Heal(float heal)
        {
            health += heal;
        }

        private void DoDamage(float damage, Actor killer)
        {
            health -= damage;
            this.killer = killer;

            if (type == HealthType.Humanoid && Actor.characterManager != null)
            {
                Actor.characterManager.cameraManager.ShakeCameras(damageCameraShake);
            }

            UpdateSystem();
        }

        private void UpdateUI(float alpha)
        {
            if (type == HealthType.Humanoid && Actor.characterManager != null)
            {
                UIManager.Instance.DamageIndicator.Show(alpha);
                UIManager.Instance.HealthDisplay.UpdateCard(health, Actor.actorName, true);
            }
        }

        private void Die()
        {
            if(type == HealthType.Humanoid)
            {
                if (Actor.actorManager && Actor.actorManager.respawnable) Actor.ConfirmDeath();
            }

            if (destoryOnDeath && !destroyRoot) Destroy(gameObject, destroyDelay);
            if (destoryOnDeath && destroyRoot) Destroy(gameObject.transform.parent.gameObject, destroyDelay);
            if (ragdoll) ragdoll.Enable(deathForce);

            if (!died) Respwan();

            if (deathEffect)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
                effect.SetActive(true);
            }

            died = true;
        }

        private void Respwan()
        {
            if (type != HealthType.Humanoid && !Actor) return;

            if (Actor.actorManager && Actor.actorManager.respawnable)
                Actor.actorManager.Respwan(Actor.actorManager.SpwanManager.respawnDelay);

            //if player enable death cam
            if (Actor.characterManager != null) deathCamera.Enable(Actor, killer);
        }

        public float GetHealth()
        {
            return health;
        }

        public void Damage(float amount, Actor damageSource)
        {
            DoDamage(amount, damageSource);
        }

        public bool IsDead()
        {
            return health <= 0 ? true : false;
        }

        public Actor GetActor()
        {
            return Actor;
        }

        public int GetGroupsCount()
        {
            if (groups != null) return groups.Length;

            return 0;
        }

        public Ragdoll GetRagdoll()
        {
            return ragdoll;
        }
    }

    public enum HealthType
    {
        Humanoid = 0,
        Other = 1
    }
}