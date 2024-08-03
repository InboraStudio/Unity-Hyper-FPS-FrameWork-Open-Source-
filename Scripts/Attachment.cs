using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Attachments/Attachment"), RequireComponent(typeof(AttachementBehavior))]
    public class Attachment : MonoBehaviour
    {
        [Tooltip("The name weapon will disable and enable the attahcments using")]
        public string Name;

        public bool IsUsed()
        {
            bool state = false;

            if (!firearmAttachmentsManager) return state;

            if (sight)
            {
                state = Name == firearmAttachmentsManager.sight;
            }

            if (muzzle)
            {
                state = Name == firearmAttachmentsManager.muzzle;
            }

            if (stock)
            {
                state = Name == firearmAttachmentsManager.stock;
            }

            if (magazine)
            {
                state = Name == firearmAttachmentsManager.magazine;
            }

            if (laser)
            {
                state = Name == firearmAttachmentsManager.laser;
            }

            return state;
        }

        public FirearmAttachmentsManager firearmAttachmentsManager { get; protected set; }
        private Firearm target;
        private AttachmentSight sight;
        private AttachmentMagazine magazine;
        private AttachmentStock stock;
        private LaserSight laser;
        private AttachmentMuzzle muzzle;

        private void Awake()
        {
            firearmAttachmentsManager = GetComponentInParent<FirearmAttachmentsManager>();
            target = GetComponentInParent<Firearm>();

            sight = GetComponent<AttachmentSight>();
            magazine = GetComponent<AttachmentMagazine>();
            stock = GetComponent<AttachmentStock>();
            laser = GetComponent<LaserSight>();
            muzzle = GetComponent<AttachmentMuzzle>();
        }

        private void Update()
        {
            UpdateSight();
            UpdateMuzzle();
            UpdateMagazine();
            UpdateLaser();
            UpdateStock();
        }

        private void UpdateSight()
        {
            if (sight)
                foreach (Transform t in transform) t.gameObject.SetActive(Name == firearmAttachmentsManager.sight);

            if (!firearmAttachmentsManager || !target) return;

            if (!sight || sight && Name != firearmAttachmentsManager.sight)
            {
                return;
            }
            if (sight.presets.Length > 0)
            {
                if (firearmAttachmentsManager.targetFirearm && firearmAttachmentsManager.targetFirearm.inputManager.sightModeSwitchInput) sight.index++;

                sight.index = Mathf.Clamp(sight.index, 0, sight.presets.Length);

                if (sight.index > sight.presets.Length - 1) sight.index = 0;
            }

            Vector3 pos = sight.presets[sight.index].position;
            Vector3 rot = sight.presets[sight.index].rotation;

            if (target.Motion)
            {
                target.Motion.aimOffcet = pos;
                target.Motion.aimRotation = rot;
                target.Motion.leanOffset = sight.usedPreset.leanOffset;

                target.Motion.aimFieldOfView = sight.usedPreset.fieldOfView;
                target.Motion.aimWeaponFieldOfview = sight.usedPreset.weaponFieldOfView;
                target.Motion.aimSpeed = sight.usedPreset.aimDownSightSpeed;
            }
        }

        private void UpdateLaser()
        {
            if (!firearmAttachmentsManager) return;

            if (laser)
            {
                foreach (Transform t in transform) t.gameObject.SetActive(Name == firearmAttachmentsManager.laser);
            }
        }

        private void UpdateMuzzle()
        {
            if (!muzzle) return;
            foreach (Transform t in transform) t.gameObject.SetActive(Name == firearmAttachmentsManager.muzzle);

            if (!firearmAttachmentsManager || !target) return;

            if (Name != firearmAttachmentsManager.muzzle) return;

            if (!muzzle.fireSFX) firearmAttachmentsManager.targetFirearm.currentFireAudio = muzzle.fireSFX;

            target.currentFireAudio = muzzle.fireSFX;
            target.VFX = muzzle.muzzleEffects;
        }

        private void UpdateMagazine()
        {
            if (!magazine) return;
            foreach (Transform t in transform) t.gameObject.SetActive(Name == firearmAttachmentsManager.magazine);

            if (!firearmAttachmentsManager || !target) return;

            if (Name != firearmAttachmentsManager.magazine) return;
            target.magazineCapacity = magazine.capacity;
        }

        private void UpdateStock()
        {
            if (!stock) return;
            foreach (Transform t in transform) t.gameObject.SetActive(Name == firearmAttachmentsManager.stock);

            if (!firearmAttachmentsManager) return;

            if (Name != firearmAttachmentsManager.stock) return;
            //your logic here
        }
    }
}