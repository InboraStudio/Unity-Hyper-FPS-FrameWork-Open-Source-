using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/Shooting Sheet")]
    public class ShootingSheet : MonoBehaviour, IOnHit
    {
        public bool downed;
        public float smoothness = 20;
        public float resetDelay = 2;
        public Vector3 rotation;

        private float ratio;
        private Quaternion defaultRotation;
        private float timer;


        private void Start()
        {
            defaultRotation = transform.localRotation;
        }

        private void Update()
        {
            if (timer > 0) timer -= Time.deltaTime;
            if (timer <= 0) downed = false;


            ratio = downed ? Mathf.Lerp(ratio, 1, Time.deltaTime * smoothness) : Mathf.Lerp(ratio, 0, Time.deltaTime * smoothness);

            transform.localRotation = Quaternion.Slerp(defaultRotation, Quaternion.Euler(rotation), ratio);
        }

        public void Enable()
        {
            downed = true;
            timer = resetDelay;
        }

        public void OnHit(HitInfo info)
        {
            Enable();

            if(info.projectile?.source?.characterManager?.character != null)
                UIManager.Instance?.Hitmarker?.Enable(false, true);
        }
    }
}