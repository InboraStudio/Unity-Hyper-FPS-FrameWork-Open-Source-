using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Attachments/Laser Sight")]
    public class LaserSight : MonoBehaviour
    {
        public InputAction laserToggleInputAction;
        public LayerMask hitableLayers;
        public Transform source;
        public Transform dot;
        public Transform range;
        public LineRenderer line;
        public bool IsOn = true;

        private Vector3 normal;
        private Vector3 point;
        RaycastHit hit;

        private void Awake()
        {
            if(line)
            {
                line.transform.Reset();
                line.useWorldSpace = true;
            }

            laserToggleInputAction.Enable();
            laserToggleInputAction.performed += context => IsOn = !IsOn;
        }

        private void LateUpdate()
        {
            if (line) line.SetPosition(0, transform.position);

            if (Physics.Raycast(transform.position, transform.forward, out hit, hitableLayers))
            {
                if (!hit.transform.TryGetComponent(out IgnoreLaserDetection ignore))
                {
                    Enable(hit);
                }
                else
                {
                    Disable();
                }
            }
            else
            {
                Disable();
            }
        }

        private void Enable(RaycastHit hit)
        {
            if (line) line.SetPosition(1, hit.point);

            normal = hit.normal;
            point = hit.point;

            dot.gameObject.SetActive(true);
            dot.position = point;
            dot.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        }

        private void Disable()
        {
            dot.gameObject.SetActive(false);
            if (line) line.SetPosition(1, range.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point, 0.4f);
        }
    }
}