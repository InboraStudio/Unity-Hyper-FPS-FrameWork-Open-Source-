using UnityEngine;
using System.Collections.Generic;


namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Camera Shaker")]
    public class CameraShaker : MonoBehaviour
    {
        public Vector3 position = new Vector3(0, 0f, 0f);
        public Vector3 rotation = new Vector3(5, 5, 6);

        public Vector3 defaultPosition { get; set; }
        public Vector3 defaultRotation { get; set; }
        public Vector3 currentShakePosition { get; set; }
        public Vector3 currentShakeRotation { get; set; }

        public Vector3 ResultPosition
        {
            get
            {
                return currentShakePosition + defaultPosition;
            }
        }

        public Vector3 ResultRotation
        {
            get
            {
                return currentShakeRotation + defaultRotation;
            }
        }

        private List<CameraShakeProfile> profiles = new List<CameraShakeProfile>();

        /// <summary>
        /// if true the pos and rot will update normally
        /// </summary>
        private bool updateTransform;

        private void Start()
        {
            WakeUp();
            defaultPosition = transform.localPosition;
            defaultRotation = transform.localEulerAngles;
        }

        private void Update()
        {
            currentShakePosition = Vector3.zero;
            currentShakeRotation = Vector3.zero;

            for (int i = 0; i < profiles.Count; i++)
            {
                if (i >= profiles.Count)
                    break;

                CameraShakeProfile profile = profiles[i];

                if (profile.State == CameraShakeState.Inactive && profile.EndOnInactive)
                {
                    profiles.RemoveAt(i);
                    i--;
                }
                else if (profile.State != CameraShakeState.Inactive)
                {
                    currentShakePosition += MathUtilities.MultiplyVectors(profile.Result(), profile.Position);
                    currentShakeRotation += MathUtilities.MultiplyVectors(profile.Result(), profile.Rotation);
                }
            }

            if (!updateTransform) return;
            transform.localPosition = ResultPosition;
            transform.localRotation = Quaternion.Euler(ResultRotation);
        }

        public CameraShakeProfile Shake(CameraShakeProfile shake)
        {
            profiles.Add(shake);
            return shake;
        }

        public CameraShakeProfile Shake(float magnitude, float roughness, float fadeInTime)
        {
            CameraShakeProfile shake = new CameraShakeProfile(magnitude, roughness);
            shake.Position = position;
            shake.Rotation = rotation;
            shake.BeginFade(fadeInTime);
            profiles.Add(shake);
            return shake;
        }

        public CameraShakeProfile Shake(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
        {
            CameraShakeProfile shake = new CameraShakeProfile(magnitude, roughness, fadeInTime, fadeOutTime);
            shake.Position = position;
            shake.Rotation = rotation;
            profiles.Add(shake);

            return shake;
        }

        public void WakeUp() => updateTransform = true;
        public void Sleep() => updateTransform = false;
    }
}