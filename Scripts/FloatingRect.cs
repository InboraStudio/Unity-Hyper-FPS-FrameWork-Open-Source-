using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Floating Rect")]
    public class FloatingRect : MonoBehaviour
    {
        public TargetType targetType;
        public Transform target;
        public Vector3 position;

        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void LateUpdate()
        {
            rectTransform.position = GetPosition();
        }

        /// <summary>
        /// Returns the final rect transform position.
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetPosition()
        {
            Camera mainCamera = Camera.main;
            Vector3 targetPosition = targetType == TargetType.Transform ? target.position : position;
            Vector3 rectTransformPosition = mainCamera.WorldToScreenPoint(targetPosition);

            Vector3 targetToCameraDirection = (targetPosition - mainCamera.transform.position);
            float cameraToTargetDot = Vector3.Dot(targetToCameraDirection, mainCamera.transform.forward);


            if (cameraToTargetDot < 0)
            {
                if (rectTransformPosition.x < Screen.width / 2)
                {
                    rectTransformPosition.x = Screen.width;
                }
                else
                {
                    rectTransformPosition.x = 0;
                }
            }

            //Clamp the rect transform's position from going off screen.
            rectTransformPosition.x = Mathf.Clamp(rectTransformPosition.x, 0, Screen.width);
            rectTransformPosition.y = Mathf.Clamp(rectTransformPosition.y, 0, Screen.height);

            return rectTransformPosition;
        }

        public enum TargetType
        {
            Transform,
            Position
        }
    }
}