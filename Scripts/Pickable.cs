using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Pickable")]
    public class Pickable : MonoBehaviour, IInteractable
    {
        public PickableType type;
        public Item item;
        public AmmoProfileData ammoProfile;
        public AudioProfile interactSound;
        public string interactionName = "Take";
        public string displayName = "Item";
        public int ammoAmount = 30;

        private FirearmAttachmentsManager firearmAttachmentsManager;

        private void Start()
        {
            firearmAttachmentsManager = transform.SearchFor<FirearmAttachmentsManager>();
        }

        /// <summary>
        /// Perform interaction from the interface.
        /// </summary>
        /// <param name="source"></param>
        public void Interact(InteractablesManager source)
        {
            if (interactSound)
                source.interactAudio?.PlayOneShot(interactSound);

            switch (type)
            {
                case PickableType.Item:
                    InteractWithItem(source);
                    break;
                case PickableType.Ammo:
                    InteractWithAmmo(source);
                    break;
            }

            //Play pickup animation
            if (source.Inventory.items.ToArray().Length != 0 && source.Inventory.items[source.Inventory.currentItemIndex].Animator)
            {
                source.Inventory.items[source.Inventory.currentItemIndex].Animator.CrossFade("Pickup", 0.1f, 0, 0);
            }
        }

        /// <summary>
        /// Returns the current correct interaction name
        /// </summary>
        /// <returns></returns>
        public string GetInteractionName()
        {
            string info = "";
            switch (type)
            {
                case PickableType.Item:
                    info = $"{displayName} - {type}";
                    break;

                case PickableType.Ammo:
                    info = $"{displayName} {ammoAmount}X - {type}";
                    break;
            }

            return $"{interactionName} {info}";
        }

        /// <summary>
        /// Performs the interaction with the item
        /// </summary>
        /// <param name="source"></param>
        public void InteractWithItem(InteractablesManager source)
        {
            Inventory inventory = source.Inventory;
            StartCoroutine(PerformItemPickup(inventory));
        }

        /// <summary>
        /// Performs the interaction with the ammo
        /// </summary>
        /// <param name="source"></param>
        public void InteractWithAmmo(InteractablesManager source)
        {
            source.Inventory.AddAmmunition(ammoProfile, ammoAmount);
            Destroy(gameObject);
        }

        //Called when firearm is dropped.
        public void OnFirearmDropped(Firearm firearm)
        {
            firearmAttachmentsManager = GetComponent<FirearmAttachmentsManager>();
            firearmAttachmentsManager.CopyFrom(firearm.GetComponent<FirearmAttachmentsManager>());
            ammoAmount = firearm.remainingAmmoCount;
        }

        private IEnumerator PerformItemPickup(Inventory inventory)
        {
            yield return null;
            Item newItem = null;
            if (inventory.items.Count < inventory.maxSlots)
            {
                newItem = inventory.CreateAndAddItem(item);
            }
            else if (inventory.items.Count >= inventory.maxSlots)
            {
                Item oldItem = inventory.FindItem(inventory.currentItemIndex);
                newItem = inventory.ReplaceItem(oldItem, item.GetComponent<Weapon>());
            }

            if (newItem.TryGetComponent<Firearm>(out Firearm firearm))
            {
                firearm.attachmentsManager.CopyFrom(firearmAttachmentsManager);

                yield return new WaitForFixedUpdate();
                firearm.remainingAmmoCount = ammoAmount;
            }

            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Pickable))]
    public class PickupableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Pickable pickupable = (Pickable)target;
            Undo.RecordObject(pickupable, $"Modified {pickupable}");
            EditorGUI.BeginChangeCheck();

            pickupable.type = (PickableType)EditorGUILayout.EnumPopup(new GUIContent("Type", "The type of the item."), pickupable.type);
            if (pickupable.type == PickableType.Item)
            {
                pickupable.item = EditorGUILayout.ObjectField(new GUIContent("Item", "The item going to be equiped"), pickupable.item, typeof(Item), true) as Item;
            }
            if(pickupable.type == PickableType.Ammo)
            {
                pickupable.ammoProfile = EditorGUILayout.ObjectField("Ammo Profile", pickupable.ammoProfile, typeof(AmmoProfileData), true) as AmmoProfileData;
            }
            pickupable.interactSound = EditorGUILayout.ObjectField("Interaction Sound", pickupable.interactSound, typeof(AudioProfile), true) as AudioProfile;
            pickupable.interactionName = EditorGUILayout.TextField("Interaction Name", pickupable.interactionName);
            pickupable.displayName = EditorGUILayout.TextField("Display Name", pickupable.displayName);


            pickupable.ammoAmount = EditorGUILayout.IntField(new GUIContent("Ammo Amount", "The amount of ammo in weapon or the ammo item"), pickupable.ammoAmount);

            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(target);
        }
    }

#endif

    public enum PickableType
    {
        Item = 0,
        Ammo = 1,
    }
}