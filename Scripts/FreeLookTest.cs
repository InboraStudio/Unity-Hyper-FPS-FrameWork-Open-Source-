using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.Experimental
{
    /// <summary>
    /// script for free look NOT READY
    /// </summary>
    [AddComponentMenu("Inbora/FPS Framework/Examples/Free Look Test")]
    public class FreeLookTest : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera overlayCamera;
        public float smoothness = 10;
        public Vector2 minimum = new Vector2(-50, 50);
        public Vector2 maximum = new Vector2(-50, 50);

        float xRotation;
        float yRotation;

        float mainToOverlayFOVRatio
        {
            get
            {
                return overlayCamera.fieldOfView / mainCamera.fieldOfView;
            }
        }

        private void Update()
        {
            float MouseX = 0;
            float MouseY = 0;

            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                xRotation = Mathf.Lerp(xRotation, 0, smoothness * Time.deltaTime);
                yRotation = Mathf.Lerp(yRotation, 0, smoothness * Time.deltaTime);
            }
            else
            {
                MouseX += Input.GetAxisRaw("Mouse X");
                MouseY += Input.GetAxisRaw("Mouse Y");
            }

            yRotation += MouseX;
            xRotation -= MouseY;

            xRotation = Mathf.Clamp(xRotation, minimum.x, maximum.x);
            yRotation = Mathf.Clamp(yRotation, minimum.y, maximum.y);

            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            overlayCamera.transform.localRotation = Quaternion.Euler(xRotation * mainToOverlayFOVRatio, yRotation * mainToOverlayFOVRatio, 0);
        }
    }
}