using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Camera Manager")]
    public class CameraManager : MonoBehaviour
    {
        [Header("FOV Kick")]
        public float FOVKick = 5;
        public float overlayFOVKick = 5;
        public float FOVKickSmoothness = 10;
        public Camera mainCamera;
        public Camera overlayCamera;

        [Header("Lean")]
        public float rotationAngle = 4;
        public float offset = 0.35f;
        public float smoothness = 10;

        [Header("Camera Shake")]
        public CameraShaker mainCameraShaker;
        public float mainCameraShakeMagnitude = 1.6f;
        public float cameraShakeRoughness = 7;
        public float cameraShakeFadeInTime = 0.2f;
        public float cameraShakeFadeOutTime = 2;

        [Header("Camera Recoil")]
        public float RecoilDampTime = 10f;
        public Vector3 RecoilAmount = new Vector3(-3f, 4f, 4f);

        [Header("Head Bob")]
        public float headbobAmount = 20;
        public float headbobRotationAmount = 30;

        private float headbobTimer;

        public CharacterManager characterManager { get; set; }
        [HideInInspector] public AudioFiltersManager audioFiltersManager;
        private float movemnetPercentage;
        [HideInInspector] public float fieldOfView;
        [HideInInspector] public float overlayFieldOfView;

        private CharacterInput characterInput;

        #region a
        private float defaultFieldOfView;
        private float defaultOverlayFieldOfView;
        private float currentLeanAngle;
        private Vector3 leanRightPosition;
        private Vector3 leanLeftPosition;
        private Vector3 CurrentRecoil;


        public bool Use_FOVKick = true;
        public bool Use_Lean = true;
        public bool Use_CameraShake = true;
        public bool Use_CameraRecoil = true;
        public bool Use_Headbob = true;

        public bool Foldout_FOVKick;
        public bool Foldout_Lean;
        public bool Foldout_CameraShake;
        public bool Foldout_CameraRecoil;
        public bool Foldout_Headbob;
        #endregion

        public Vector3 ResultPosition
        {
            get
            {
                Vector3 result = Vector3.zero;
                return result += ResultLeanPosition + HeadbobPosition;
            }
        }

        public Vector3 ResultRotation
        {
            get
            {
                Vector3 result = Vector3.zero;
                return result += ResultLeanRotation + ResultRecoilRotation + HeadbobRotation;
            }
        }

        public Vector3 ResultLeanPosition { get; set; }
        public Vector3 ResultLeanRotation { get; set; }
        public Vector3 ResultRecoilRotation { get; set; }
        public Vector3 HeadbobPosition { get; set; }
        public Vector3 HeadbobRotation { get; set; }
        private SettingsManager SettingsManager { get; set; }

        #region Logic
        private void Start()
        {
            if (mainCamera == null) mainCamera = Camera.main;

            characterManager = GetComponentInParent<CharacterManager>();
            audioFiltersManager = FindObjectOfType<AudioListener>().GetComponent<AudioFiltersManager>();
            characterInput = GetComponentInParent<CharacterInput>();
            SettingsManager = FindObjectOfType<SettingsManager>();


            if (mainCamera && !SettingsManager)
            {
                fieldOfView = mainCamera.fieldOfView;
                defaultFieldOfView = mainCamera.fieldOfView;
            }

            if (overlayCamera && !SettingsManager)
            {
                overlayFieldOfView = overlayCamera.fieldOfView;
                defaultOverlayFieldOfView = overlayCamera.fieldOfView;
            }

            if (SettingsManager)
            {
                if(mainCamera) mainCamera.fieldOfView = FPSFrameworkUtility.fieldOfView;
                if (overlayCamera) overlayCamera.fieldOfView = FPSFrameworkUtility.weaponFieldOfView;
                fieldOfView = FPSFrameworkUtility.fieldOfView;
                overlayFieldOfView = FPSFrameworkUtility.weaponFieldOfView;
            }

            if (Use_Lean)
            {
                leanRightPosition = new Vector3(offset, 0, 0);
                leanLeftPosition = new Vector3(-offset, 0, 0);
            }
        }

        private void FixedUpdate()
        {
            if (!Use_CameraRecoil) return;
            CurrentRecoil = Vector3.Lerp(CurrentRecoil, Vector3.zero, 35 * Time.deltaTime);

            ResultRecoilRotation = Vector3.Slerp(ResultRecoilRotation, CurrentRecoil, RecoilDampTime * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (SettingsManager)
            {
                defaultFieldOfView = FPSFrameworkUtility.fieldOfView;
                defaultOverlayFieldOfView = FPSFrameworkUtility.weaponFieldOfView;
            }

            if (characterManager != null)
            {
                movemnetPercentage = characterManager.velocity.magnitude / characterManager.character.sprintSpeed;
                movemnetPercentage = Mathf.Clamp(movemnetPercentage, 0, 1.3f);
            }

            if (mainCamera)
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, fieldOfView + FOVKickResult(), Time.deltaTime * FOVKickSmoothness);

            if (overlayCamera)
                overlayCamera.fieldOfView = Mathf.Lerp(overlayCamera.fieldOfView, overlayFieldOfView + OverlayFOVKickResult(), Time.deltaTime * FOVKickSmoothness);

            if (characterInput.leanRightInput && Use_Lean)
            {
                ResultLeanPosition = Vector3.Lerp(ResultLeanPosition, leanRightPosition, Time.deltaTime * smoothness);
                currentLeanAngle = Mathf.Lerp(currentLeanAngle, -rotationAngle, Time.deltaTime * smoothness);
            }

            if (characterInput.leanLeftInput && Use_Lean)
            {
                ResultLeanPosition = Vector3.Lerp(ResultLeanPosition, leanLeftPosition, Time.deltaTime * smoothness);
                currentLeanAngle = Mathf.Lerp(currentLeanAngle, rotationAngle, Time.deltaTime * smoothness);
            }

            if (!characterInput.leanRightInput && !characterInput.leanLeftInput && Use_Lean)
            {
                ResultLeanPosition = Vector3.Lerp(ResultLeanPosition, Vector3.zero, Time.deltaTime * smoothness);
                currentLeanAngle = Mathf.Lerp(currentLeanAngle, 0, Time.deltaTime * smoothness);
            }

            if (Use_Lean)
                ResultLeanRotation = new Vector3(0, 0, currentLeanAngle);

            if (Use_Headbob) UpdateHeadbob();

            transform.localPosition = ResultPosition;
            transform.localRotation = Quaternion.Euler(ResultRotation);
        }

        public void ApplyRecoil(float vertical, float horizontal, float shakeMultipler, bool isAiming = false)
        {
            if (!Use_CameraRecoil) return;

            float multipler = isAiming ? 1 : 0.7f;
            characterManager.AddLookValue(vertical * multipler, horizontal * multipler);
            CurrentRecoil += new Vector3(RecoilAmount.x, Random.Range(-RecoilAmount.y, RecoilAmount.y), Random.Range(-RecoilAmount.z, RecoilAmount.z)) * multipler * shakeMultipler;
        }

        public void ShakeCameras(float multipler)
        {
            if (!Use_CameraShake) return;
            if (mainCameraShaker)

                mainCameraShaker.Shake(mainCameraShakeMagnitude * multipler, cameraShakeRoughness, cameraShakeFadeInTime, cameraShakeFadeOutTime);
        }

        public void ShakeCameras(float multipler, float fadeOutTime)
        {
            if (!Use_CameraShake) return;

            if (mainCameraShaker)
                mainCameraShaker.Shake(mainCameraShakeMagnitude * multipler, cameraShakeRoughness, cameraShakeFadeInTime, fadeOutTime);
        }

        public void ShakeCameras(float multipler, float roughness, float fadeOutTime)
        {
            if (!Use_CameraShake) return;
            if (mainCameraShaker)

                mainCameraShaker.Shake(mainCameraShakeMagnitude * multipler, roughness, cameraShakeFadeInTime, fadeOutTime);
        }

        public void SetFieldOfView(float main, float overlay)
        {
            fieldOfView = main;
            overlayFieldOfView = overlay;
        }

        public void SetFieldOfView(float main, float overlay, float time)
        {
            fieldOfView = Mathf.Lerp(fieldOfView, main, time);
            overlayFieldOfView = Mathf.Lerp(overlayFieldOfView, overlay, time);
        }

        public void SetFieldOfView(float main, float overlay, float timeMain, float timeOverlay)
        {
            fieldOfView = Mathf.Lerp(fieldOfView, main, timeMain);
            overlayFieldOfView = Mathf.Lerp(overlayFieldOfView, overlay, timeOverlay);
        }

        public void ResetFieldOfView(float time)
        {
            fieldOfView = Mathf.Lerp(fieldOfView, defaultFieldOfView, time);
            overlayFieldOfView = Mathf.Lerp(overlayFieldOfView, defaultOverlayFieldOfView, time);
        }

        private float FOVKickResult()
        {
            if (!Use_FOVKick) return 0;

            float value;

            if (movemnetPercentage > 0.8f)
            {
                value = FOVKick * movemnetPercentage * movemnetPercentage;
            }
            else
            {
                value = 0;
            }

            return value;
        }

        private float OverlayFOVKickResult()
        {
            if (!Use_FOVKick) return 0;

            float value;

            if (movemnetPercentage > 0.8f)
            {
                value = overlayFOVKick * movemnetPercentage;
            }
            else
            {
                value = 0;
            }

            return value;
        }


        private void UpdateHeadbob()
        {
            headbobTimer += Time.deltaTime * characterManager.velocity.magnitude;

            float posX = 0f;
            float posY = 0f;
            float rotZ = 0;
            float multipler = characterManager.velocity.magnitude / characterManager.character.tacticalSprintSpeed;
            posX += ((headbobAmount / 100) / 2f * Mathf.Sin(headbobTimer) * multipler);
            posY += ((headbobAmount / 100) / 2f * Mathf.Sin(headbobTimer * 2f) * multipler);
            rotZ += ((headbobRotationAmount / 100) / 2 * Mathf.Sin(headbobTimer) * multipler);

            Vector3 posResult = new Vector3(posX, posY);
            Vector3 rotResult = new Vector3(0, 0, rotZ);

            if (!characterManager.IsVelocityZero() && characterManager.isGrounded)
            {
                HeadbobPosition = Vector3.Lerp(HeadbobPosition, posResult, Time.deltaTime * 5);
                HeadbobRotation = Vector3.Lerp(HeadbobRotation, rotResult, Time.deltaTime * 20);
            }
            else
            {
                HeadbobPosition = Vector3.Lerp(HeadbobPosition, Vector3.zero, Time.deltaTime * 5);
                HeadbobRotation = Vector3.Lerp(HeadbobRotation, Vector3.zero, Time.deltaTime * 5);
            }
        }

        #endregion
    }
}