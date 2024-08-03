using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Weapon Events")]
    /// <summary>
    /// This class is meant to have functions that you can call via animation event
    /// </summary>
    public class WeaponEvents : MonoBehaviour
    {
        /// <summary>
        /// target weapon
        /// </summary>
        private Firearm weapon;

        [Header("Camera Shake")]
        public float cameraShakeMultipler = 0.5f;
        public float cameraShakeRoughness = 5;
        public float cameraShakeFadeOutTime = 0.2f;

        [Header("Mag Throw")]
        public Transform magThrowPoint;
        public GameObject Mag;

        public bool MagThrown { get; set; } = false;

        private void Start()
        {
            if (GetComponent<Firearm>()) weapon = GetComponent<Firearm>();
            if (GetComponentInChildren<Firearm>()) weapon = GetComponentInChildren<Firearm>();
            if (GetComponentInParent<Firearm>()) weapon = GetComponentInParent<Firearm>();
        }

        /// <summary>
        /// shakes the camera with a multipler
        /// </summary>
        /// <param name="multipler"></param>
        public void ShakeCameras(float multipler)
        {
            weapon.characterManager.cameraManager.ShakeCameras(multipler * cameraShakeMultipler, cameraShakeRoughness, cameraShakeFadeOutTime);
        }

        public void ThrowShell()
        {
            weapon.ThrowCasing();
        }

        public void ThrowMag()
        {
            if (MagThrown) return;

            Instantiate(Mag, magThrowPoint.position, magThrowPoint.rotation);
            MagThrown = true;
        }

        public void Reload(int amount)
        {
            weapon.ApplyReloadOnce(amount);
        }
    }
}