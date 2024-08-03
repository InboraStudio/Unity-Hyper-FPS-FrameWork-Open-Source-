using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Inventory")]
    public class Inventory : MonoBehaviour, IEnumerable
    {
        public List<Item> items;
        public List<AmmoProfile> ammo;
        public int maxSlots = 3;
        public float dropForce = 1;
        public Transform dropLocation;

        public CharacterManager characterManager { get; set; }

        private int prevIndex;

        public int currentItemIndex { get; set; }
        private Controls inputActions;

        public Actor Actor
        {
            get
            {
                if(characterManager != null)
                return characterManager.actor;

                return null;
            }
        }
        protected virtual void Awake()
        {
            prevIndex = currentItemIndex;

            foreach (Transform child in transform)
            {
                if(child.TryGetComponent<Item>(out Item item))
                {
                    items.Add(item);
                }
            }

            inputActions = new Controls();
            inputActions.Player.Enable();
inputActions.Player.NextItem.performed += context => currentItemIndex++;

        }

        protected virtual void Start()
        {
            StartCoroutine(AddDefaultItems());
        }

        public IEnumerator AddDefaultItems()
        {
            yield return new WaitForFixedUpdate();

            if (characterManager != null && characterManager.actor && characterManager.actor.actorManager && characterManager.actor.actorManager.defaultItems != null)
            {

                foreach (Item item in characterManager.actor.actorManager.defaultItems)
                {
                    CreateAndAddItem(item);
                }

                Switch(0);
            }
        }

        protected virtual void Update()
        {
            if (inputActions.Player.SwitchItem.ReadValue<float>() > 0) currentItemIndex++;
            if (inputActions.Player.SwitchItem.ReadValue<float>() < 0) currentItemIndex--;

            GetAlphaKeys();

            for(int i = 0; i < items.ToArray().Length; i++)
            {
                if(i >= maxSlots)
                {
                    items[i].Drop(Vector3.down * dropForce, Vector3.up * dropForce * 3);
                }
            }

            if (currentItemIndex > items.ToArray().Length - 1) currentItemIndex = 0;
            if (currentItemIndex < 0) currentItemIndex = items.ToArray().Length - 1;

            if (items.Count > 1 && prevIndex != currentItemIndex)
            {
                Switch();
            }

            prevIndex = currentItemIndex;
        }

        protected virtual void GetAlphaKeys()
        {
            //items.Count >= 2
            if (inputActions.Player.Item1.triggered) currentItemIndex = 0;
            if (inputActions.Player.Item2.triggered && items.Count >= 2) currentItemIndex = 1;
            if (inputActions.Player.Item3.triggered && items.Count >= 3) currentItemIndex = 2;
            if (inputActions.Player.Item4.triggered && items.Count >= 4) currentItemIndex = 3;
            if (inputActions.Player.Item5.triggered && items.Count >= 5) currentItemIndex = 4;
            if (inputActions.Player.Item6.triggered && items.Count >= 6) currentItemIndex = 5;
            if (inputActions.Player.Item7.triggered && items.Count >= 7) currentItemIndex = 6;
            if (inputActions.Player.Item8.triggered && items.Count >= 8) currentItemIndex = 7;
            if (inputActions.Player.Item9.triggered && items.Count >= 9) currentItemIndex = 8;
        }

        /// <summary>
        /// switchs current item by current item index in inventory
        /// </summary>
        public void Switch()
        {
            Switch(currentItemIndex);
        }

        /// <summary>
        /// switchs current item index by index
        /// </summary>
        /// <param name="index">what index to switch to</param>
        public void Switch(int index)
        {
            int i = 0;
            foreach (Item item in items)
            {
                if (i == index)
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
                i++;
            }

            currentItemIndex = index;

            OnItemSwitch();
        }

        /// <summary>
        /// switchs current item index by name
        /// </summary>
        /// <param name="name">what to switch to</param>
        public void Switch(string name)
        {
            if (!FindItem(name))
            {
                Debug.Log($"Can't find item with name of '{name}'");
                return;
            }

            int index = GetItemIndex(name);

            Switch(index);
            currentItemIndex = index;
            OnItemSwitch();
        }

        /// <summary>
        /// drops item and replaces it with another by reference
        /// </summary>
        /// <param name="from">what to drop</param>
        /// <param name="to">what to add</param>
        /// <returns></returns>
        public Item ReplaceItem(Item from, Item to)
        {
            Item oldItem = FindItem(from);
            Item newItem = CreateAndAddItem(to);

            oldItem.Drop(transform.forward * dropForce, transform.right * dropForce * 3);

            OnItemReplaced(from, to);

            return newItem;
        }

        /// <summary>
        ///  creates and adds item by reference
        /// </summary>
        /// <param name="item">item reference</param>
        /// <returns></returns>
        public Item CreateAndAddItem(Item item)
        {
            Item newItem = Instantiate(item, transform);
            items.Add(newItem);

            newItem.gameObject.SetActive(true);
            Switch(items.Count - 1);

            OnItemCreated(newItem);
            OnItemAdded(newItem);

            return newItem;
        }

        /// <summary>
        ///  adds item to items list by reference
        /// </summary>
        /// <param name="item">item reference</param>
        /// <returns></returns>
        public void AddItem(Item item)
        {
            items.Add(item);
            OnItemAdded(item);
        }

        /// <summary>
        ///  removes item from items list by reference
        /// </summary>
        /// <param name="item">item reference</param>
        /// <returns></returns>
        public void RemoveItem(Item item)
        {
            items.Remove(item);
            OnItemRemoved(item);
        }

        /// <summary>
        ///  returns item index by reference
        /// </summary>
        /// <param name="item">item reference</param>
        /// <returns></returns>
        public Item FindItem(Item item)
        {
            Item result = items.Find(_item => _item == item);

            return result;
        }

        /// <summary>
        ///  returns item by it's index in items list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Item FindItem(int index)
        {
            Item result = items[index];

            return result;
        }


        /// <summary>
        /// returns item by name
        /// </summary>
        /// <param name="name">name of item</param>
        /// <returns></returns>
        public Item FindItem(string name)
        {
            Item result = items.Find(item => item.Name == name);

            return result;
        }
        
        /// <summary>
        /// returns item index in items list by name
        /// </summary>
        /// <param name="name">name of item</param>
        /// <returns></returns>
        public int GetItemIndex(string name)
        {
            return items.IndexOf(FindItem(name));
        }
        
        /// <summary>
        /// returns ammo profile
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AmmoProfile FindAmmunition(string name)
        {
            AmmoProfile result = ammo.Find(data => data.data.Name == name);

            return result;
        }
        
        /// <summary>
        /// returns ammo profile
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AmmoProfile FindAmmunition(AmmoProfileData data)
        {
            AmmoProfile result = ammo.Find(resultData => resultData.data == data);

            return result;
        }
        
        /// <summary>
        /// returns ammo profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public AmmoProfile FindAmmunition(AmmoProfile profile)
        {
            AmmoProfile result = ammo.Find(data => data == profile);

            return result;
        }
        
        /// <summary>
        /// returns true if all slots are full
        /// </summary>
        /// <returns></returns>
        public bool IsFullSlots()
        {
            return items.ToArray().Length >= maxSlots;
        }
        
        /// <summary>
        /// adds new ammo
        /// </summary>
        /// <param name="data"></param>
        public void AddAmmunition(AmmoProfile data)
        {
            ammo.Add(data);
            OnAmmunitionAdded(data);
        }
        
        /// <summary>
        /// adds new ammo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="profile"></param>
        public void AddAmmunition(AmmoProfile data, AmmoProfileData profile)
        {
            ammo.Add(data);
            AmmoProfile newAmmo = FindAmmunition(data);
            newAmmo.data = profile;
            OnAmmunitionAdded(data);
        }
        
        /// <summary>
        /// adds amount of ammo
        /// </summary>
        /// <param name="name">what ammo to add</param>
        /// <param name="amount">how much ammo to add</param>
        public void AddAmmunition(string name, int amount)
        {
            AmmoProfile profile = FindAmmunition(name);

            if (profile == null)
            {
                Debug.LogError($"Can't find ammo profile with the name {name}.");
            }
            else
            profile.amount += amount;

            OnAmmunitionAdded(profile, amount);
        }

        /// <summary>
        /// adds amount of ammo
        /// </summary>
        /// <param name="profile">what ammo to add</param>
        /// <param name="amount">how much ammo to add</param>
        public void AddAmmunition(AmmoProfile profile, int amount)
        {
            AmmoProfile _profile = FindAmmunition(profile);

            if (_profile != null) 
                _profile.amount += amount;

            OnAmmunitionAdded(_profile, amount);
        }

        /// <summary>
        /// adds amount of ammo
        /// </summary>
        /// <param name="profile">what ammo to add</param>
        /// <param name="amount">how much ammo to add</param>
        public void AddAmmunition(AmmoProfileData profile, int amount)
        {
            AmmoProfile _profile = FindAmmunition(profile);

            if (_profile != null)
                _profile.amount += amount;

            OnAmmunitionAdded(_profile, amount);
        }

        /// <summary>
        /// drops all items
        /// </summary>
        public void DropAllItems()
        {
            Item[] itemsArray = items.ToArray();
            foreach (Item item in itemsArray)
            {
                item.gameObject.SetActive(true);
                item.Drop(Vector3.down * dropForce, Vector3.up * dropForce * 3);
            }
        }
        
        /// <summary>
        /// drops all items with random force
        /// </summary>
        public void DropAllItemsWithRandomForce()
        {
            Item[] itemsArray = items.ToArray();
            foreach (Item item in itemsArray)
            {
                item.gameObject.SetActive(true);

                Vector3 dropForce = Random.insideUnitSphere * this.dropForce;
                if (dropForce.y < 0) dropForce.y *= -1;

                item.Drop(dropForce, Random.insideUnitSphere * this.dropForce * 3);
            }
        }

        /// <summary>
        /// gets called when any item has been replaced from inventory
        /// </summary>
        /// <param name="from">what item has been droped</param>
        /// <param name="to">what item has been created and added</param>
        protected virtual void OnItemReplaced(Item from, Item to) { }

        /// <summary>
        /// gets called when current item index changes
        /// </summary>
        protected virtual void OnItemSwitch() { }

        /// <summary>
        /// gets called when any item has been added to inventory
        /// </summary>
        /// <param name="item">what item has been added</param>
        protected virtual void OnItemAdded(Item item) { }

        /// <summary>
        /// gets called when any item has been created in inventory
        /// </summary>
        /// <param name="item">what item has been created</param>
        protected virtual void OnItemCreated(Item item) { }

        /// <summary>
        /// gets called when any item has been removed from inventory
        /// </summary>
        /// <param name="item">what item has been removed</param>
        protected virtual void OnItemRemoved(Item item) { }

        /// <summary>
        /// gets called when any ammo has been added to inventory
        /// </summary>
        /// <param name="profile">what ammo has been added</param>
        protected virtual void OnAmmunitionAdded(AmmoProfile profile) { }

        /// <summary>
        /// gets called when any ammo has been removed from inventory
        /// </summary>
        /// <param name="profile">what ammo has been removed</param>
        protected virtual void OnAmmunitionRemoved(AmmoProfile profile) { }

        /// <summary>
        /// gets called when any ammo has been added to inventory
        /// </summary>
        /// <param name="profile">what profile has been added</param>
        /// <param name="amount">how much ammo has been added to profile</param>
        protected virtual void OnAmmunitionAdded(AmmoProfile profile, int amount) { }


        private IEnumerable<string> Events()
        {
            yield return "a";
            yield return "b";
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Events().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}