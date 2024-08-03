using UnityEngine;
using System;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Actor")]
    [DisallowMultipleComponent()]
    public class Actor : MonoBehaviour
    {
        /// <summary>
        /// The name of the actor, this will be set automaticly if on actor repsawn.
        /// </summary>
        public string actorName;

        /// <summary>
        /// The actor manager for this actor.The actor manager is repsonsible for handling respawning.
        /// </summary>
        public ActorManager actorManager { get; set; }
        /// <summary>
        /// The character manager for this actor. It's used to get the data of your FPS Controller as it could be custom.
        /// </summary>
        public CharacterManager characterManager { get; set; }
        /// <summary>
        /// The IDamageable interface of this actor. It's used to damage the actor.
        /// </summary>
        public IDamageable healthSystem { get; set; }
        /// <summary>
        /// An action called when the actor kills another actor.
        /// </summary>
        public Action<Actor, IDamageableGroup> onKill {  get; set; }
        /// <summary>
        /// The inventory of this actor. It's used to manage the actor's items, ammo and ect...
        /// </summary>
        public Inventory inventory { get; private set; }

        /// <summary>
        /// returns true if the shooter has any component with the interface ICharacterController implemented
        /// </summary>
        public bool isLocallyMine
        {
            get
            {
                return characterManager?.character != null;
            }
        }

        private void Awake()
        {
            actorManager = GetComponentInParent<ActorManager>();
            inventory = transform.SearchFor<Inventory>();
            characterManager = GetComponent<CharacterManager>();
            healthSystem = GetComponent<IDamageable>();
        }

        private void Start()
        {
            onKill = ConfirmKill;
            if (actorManager) actorManager.actor = this;

            if (UIManager.Instance && isLocallyMine)
            {
                UIManager.Instance.HealthDisplay?.UpdateCard(healthSystem.GetHealth(), actorName, false);
                UIManager.Instance.HealthDisplay.slider.maxValue = healthSystem.GetHealth();
                UIManager.Instance.HealthDisplay.backgroundSlider.maxValue = healthSystem.GetHealth();
                UIManager.Instance.HealthDisplay.actorNameText.text = actorName;
            }
        }

        private void Update()
        {
            if (UIManager.Instance && characterManager != null)
                UIManager.Instance.HealthDisplay?.UpdateCard(healthSystem.GetHealth(), actorName, true);
        }

        /// <summary>
        /// Increases the kill count of this actor by one and shows kill message. This prevents throwing errors if the actor manager of this actor is null.
        /// </summary>
        /// <param name="victim">The actoe which has been killed. This is used to show the name of the actor.</param>
        /// <param name="group">The part which has been hit on kill. This is used to tell the kill feed if it's a headshot or not.</param>
        public void ConfirmKill(Actor victim, IDamageableGroup group)
        {
            if(actorManager)
            actorManager.kills++;

            if (isLocallyMine)
            {
                UIManager.Instance?.KillFeed?.Show(this, victim.actorName, group.GetBone() == HumanBodyBones.Head ? true : false);
                UIManager.Instance?.Hitmarker?.Enable(true, true, UIManager.Instance.Hitmarker.maxSize);
            }
        }

        /// <summary>
        ///  Increases the death count of this actor by one. This prevents throwing errors if the actor manager of this actor is null.
        /// </summary>
        public void ConfirmDeath()
        {
            if(actorManager)
            actorManager.deaths++;
        }
    }
}