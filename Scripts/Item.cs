using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [RequireComponent(typeof(ItemInput))]
    public abstract class Item : MonoBehaviour
    {
        /// <summary>
        /// name of this item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The script which is responsable for weapon pro
        /// </summary>
        public FPSMotion Motion { get; set; }

        /// <summary>
        /// If weapon is not firearm this will be null
        /// </summary>
        public Firearm Firearm { get; set; }

        /// <summary>
        /// If weapon is not throwable this will be null
        /// </summary>
        public Throwable Throwable { get; set; }

        /// <summary>
        /// All active projectile from this weapon
        /// </summary>
        public List<Projectile> Projectiles { get; set; } = new List<Projectile>();

        public Inventory Inventory { get; protected set; }

        public Animator Animator { get; protected set; }

        public Pickable Replacement { get; protected set; }

        public Actor Actor { get; protected set; }

        public CharacterManager characterManager { get; set; }

        public ItemInput itemInput { get; set; }

        public Action onDropped { get; set; }

        public void Setup(string itemName, Pickable pickupable, Throwable throwable = null)
        {
            Throwable = throwable;
            Replacement = pickupable;
            Name = itemName;
            GetComponents();
        }

        private void GetComponents()
        {
            itemInput = GetComponent<ItemInput>();
            characterManager = GetComponentInParent<CharacterManager>();
            Inventory = GetComponentInParent<Inventory>();
            Animator = GetComponentInChildren<Animator>();
            Actor = GetComponentInParent<Actor>();
            Motion = GetComponent<FPSMotion>();
        }

        /// <summary>
        /// Drops the item on ground
        /// </summary>
        public void Drop(Vector3 force, Vector3 torque, Firearm firearm = null)
        {
            characterManager?.cameraManager?.ResetFieldOfView(10);
            Inventory?.RemoveItem(this);

            if (Replacement == null)
            {
                Inventory?.Switch(Inventory.items.Count - 1);
                Debug.LogError("Couldn't find a replacement, item will be destroyed instead.");
                Destroy(gameObject);
                return;
            }

            Pickable newPickupable = Instantiate(Replacement, Inventory.dropLocation.position, Inventory.transform.rotation);

            if (newPickupable.GetComponent<Rigidbody>())
            {
                newPickupable.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                newPickupable.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.VelocityChange);
            }

            if (Inventory.items.Count > 0)
            {
                Inventory.Switch(Inventory.items.Count - 1);
            }

            if (firearm) newPickupable.OnFirearmDropped(firearm);

            OnDropped();
            onDropped?.Invoke();

            Destroy(gameObject);
        }

        /// <summary>
        /// Called after the item has been dropped.
        /// </summary>
        protected virtual void OnDropped()
        {

        }
    }
}