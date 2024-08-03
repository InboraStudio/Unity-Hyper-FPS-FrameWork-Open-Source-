using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [RequireComponent(typeof(Attachment))]
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Attachments/Attachement Behavior")]
    public class AttachementBehavior : MonoBehaviour
    {
        [Header("Multiplers")]
        [Range(1, 500), SerializeField, Tooltip("Effects firearm damage")] float m_damage = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm shooting spread")] float m_spread = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm fire rate")] float m_fireRate = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm damage range (Damage Only)")] float m_range = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm projectile velocity")] float m_muzzleVelocity = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm recoil (Camera Movment)")] float m_recoil = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm visible model recoile")] float m_visualRecoil = 100;
        [Range(1, 500), SerializeField, Tooltip("Effects firearm aim down sight speed")] float m_aimSpeed = 100;

        public float Damage()
        {
            return m_damage / 100;
        }

        public float Spread()
        {
            return m_spread / 100;
        }

        public float FireRate()
        {
            return m_fireRate / 100;
        }

        public float Range()
        {
            return m_range / 100;
        }

        public float MuzzleVelocity()
        {
            return m_muzzleVelocity / 100;
        }

        public float Recoil()
        {
            return m_recoil / 100;
        }

        public float VisualRecoil()
        {
            return m_visualRecoil / 100;
        }

        public float AimSpeed()
        {
            return m_aimSpeed / 100;
        }

        public bool IsUsed()
        {
            if (attachment)
                return attachment.IsUsed();

            return false;
        }

        private Attachment attachment;

        private void Awake()
        {
            attachment = GetComponent<Attachment>();
        }
    }
}