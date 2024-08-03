using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Examples/Attachments Switching")]
    public class AttachmentSwitching : MonoBehaviour
    {
        public static AttachmentSwitching Instance;

        public string sight { get; set; }
        public string muzzle { get; set; }
        public string magazine { get; set; }
        public string stock { get; set; }
        public string laser { get; set; }

        public static event System.Action OnSwitch;

        public static event System.Action OnSightSwitch;
        public static event System.Action OnMuzzleSwitch;
        public static event System.Action OnMagazineSwitch;
        public static event System.Action OnStockSwitch;
        public static event System.Action OnLaserSwitch;


        private void Awake()
        {
            if (!Instance) Instance = this;
            else Destroy(gameObject);
        }

        public void SwitchSight(string name)
        {
            sight = name;
            OnSwitch?.Invoke();
            OnSightSwitch?.Invoke();
        }

        public void SwitchMuzzle(string name)
        {
            muzzle = name;
            OnSwitch?.Invoke();
            OnMuzzleSwitch?.Invoke();
        }

        public void SwitchMagazine(string name)
        {
            magazine = name;
            OnSwitch?.Invoke();
            OnMagazineSwitch?.Invoke();
        }

        public void SwitchStock(string name)
        {
            stock = name;
            OnSwitch?.Invoke();
            OnStockSwitch?.Invoke();
        }

        public void SwitchLaser(string name)
        {
            laser = name;
            OnSwitch?.Invoke();
            OnLaserSwitch?.Invoke();
        }
    }
}