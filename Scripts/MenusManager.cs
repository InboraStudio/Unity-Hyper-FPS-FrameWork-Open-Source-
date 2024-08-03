using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Menus Manager")]
    public class MenusManager : MonoBehaviour
    {
        public string defaultMenu = "Start";
        public List<Menu> menus;

        private void Start()
        {
            Open(defaultMenu);
        }

        public Menu GetMenu(string name)
        {
            return menus.Find(menu => menu.Name == name);
        }

        public void Open(string name)
        {
            Menu menu = GetMenu(name);
            if (!menu)
            {
                Debug.LogWarning($"Couldn't find menu named {name}.");
                return;
            }

            CloseAll();
            menu.Open();
        }

        public void Close(string name)
        {
            Menu menu = GetMenu(name);
            if (!menu)
            {
                Debug.LogWarning($"Couldn't find menu named {name}.");
                return;
            }

            menu.Close();
        }

        public void OpenAll()
        {
            if (menus.Count <= 0) return;
            foreach (Menu menu in menus) menu.Open();
        }

        public void CloseAll()
        {
            if (menus.Count <= 0) return;
            foreach (Menu menu in menus) menu.Close();
        }
    }
}