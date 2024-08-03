using Inbora.FPSFramework.Animation;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Inbora.FPSFramework
{
    public static class FPSControllerCreator
    {
        [MenuItem(MenuItemPaths.CreateFPSController, false, 2)]
        public static void CreateFPSController()
        {
            //create fps controller
            GameObject player = new GameObject("First Person Controller");

            //create camera game transform
            Transform camera = player.transform.CreateChild("Camera");

            //create animations
            Transform animations = camera.CreateChild("Animations");
            Transform clipsHolder = animations.CreateChild("Clips");
            Transform jumpAnimationTransform = clipsHolder.CreateChild("Jump");
            Transform landAnimationTransform = clipsHolder.CreateChild("Land");
            Transform breathAnimationTransform = clipsHolder.CreateChild("Breath");

            //create camera manager
            Transform movement = animations.CreateChild("Movement");

            //create camera holder
            Transform cameraHolder = movement.CreateChild("Cameras Holder");
            Transform inventoryTransform = movement.CreateChild("Inventory");
            Transform dropPoint = movement.CreateChild("Drop Point");

            Camera mainCamera = cameraHolder.CreateChild("Main Camera").gameObject.AddComponent<Camera>();
            AudioFiltersManager audioFiltersManager = mainCamera.gameObject.AddComponent<AudioFiltersManager>();
            Camera overlayCamera = cameraHolder.CreateChild("Overlay Camera").gameObject.AddComponent<Camera>();

            mainCamera.tag = "MainCamera";
            camera.transform.localPosition = new Vector3(0, 0.8f, 0);

            //add fps controller componenets to the main object "First Person Controller"
            CharacterController characterController = player.AddComponent<CharacterController>();
            FirstPersonController fpsController = player.AddComponent<FirstPersonController>();
            HealthSystem healthSystem = player.AddComponent<HealthSystem>();
            Actor actor = player.AddComponent<Actor>();

            ProceduralAnimator proceduralAnimator = animations.gameObject.AddComponent<ProceduralAnimator>();

            ProceduralAnimation jumpAnimation = jumpAnimationTransform.gameObject.AddComponent<ProceduralAnimation>();
            KickModifier jumpKickModifier = jumpAnimation.gameObject.AddComponent<KickModifier>();
            SpringModifier jumpSpringModifier = jumpAnimation.gameObject.AddComponent<SpringModifier>();

            ProceduralAnimation landAnimation = landAnimationTransform.gameObject.AddComponent<ProceduralAnimation>();
            KickModifier landKickModifier = landAnimation.gameObject.AddComponent<KickModifier>();
            SpringModifier landSpringModifier = landAnimation.gameObject.AddComponent<SpringModifier>();

            ProceduralAnimation breathAnimation = breathAnimationTransform.gameObject.AddComponent<ProceduralAnimation>();
            WaveModifier breathWaveModifier = breathAnimation.gameObject.AddComponent<WaveModifier>();

            CameraManager cameraManager = movement.gameObject.AddComponent<CameraManager>();
            CameraShaker cameraShaker = cameraHolder.gameObject.AddComponent<CameraShaker>();

            cameraManager.mainCameraShaker = cameraShaker;

            Inventory inventory = inventoryTransform.gameObject.AddComponent<Inventory>();
            InteractablesManager interactables = inventoryTransform.gameObject.AddComponent<InteractablesManager>();
            CharacterManager characterManager = fpsController.gameObject.AddComponent<CharacterManager>();

            //set some defaults
            fpsController._Camera = camera.transform;

            UnityEventTools.AddPersistentListener(characterManager.onJump, jumpKickModifier.Trigger);
            UnityEventTools.AddPersistentListener(characterManager.onLand, landKickModifier.Trigger);

            healthSystem.type = HealthType.Humanoid;
            healthSystem.destoryOnDeath = true;

            proceduralAnimator.layersHolder = clipsHolder;
            proceduralAnimator.positionWeight = 0.34f;
            proceduralAnimator.rotationWeight = 0.72f;
            
            jumpAnimation.Name = "Jump";
            jumpKickModifier.position = new Vector3(0, 0.2f, 0);
            jumpKickModifier.rotation = new Vector3(2, 0, 0);
            UnityEventTools.AddPersistentListener(jumpKickModifier.OnTrigger, jumpSpringModifier.Trigger);

            landAnimation.Name = "Land";
            landAnimation.weight = 0.37f;
            landKickModifier.position = new Vector3(0, -0.2f, 0);
            landKickModifier.rotation = new Vector3(-5, 0, 0);
            UnityEventTools.AddPersistentListener(landKickModifier.OnTrigger, landSpringModifier.Trigger);

            landSpringModifier.position.value = new Vector3(0, -0.2f, 0);
            landSpringModifier.position.fadeOutTime = 0.5f;

            landSpringModifier.rotation.value = new Vector3(3f, 0, 0);
            landSpringModifier.rotation.fadeOutTime = 0.7f;

            breathAnimation.Name = "Breath";
            breathWaveModifier.speed = 0.5f;
            breathWaveModifier.amount = 0.5f;
            breathWaveModifier.rotation.amount = new Vector3(2, 1, 0);
            breathWaveModifier.rotation.speed = new Vector3(1, 2, 0);
            breathAnimation.weight = 0.5f;

            inventory.dropLocation = dropPoint;

            cameraManager.mainCamera = mainCamera;
            cameraManager.overlayCamera = overlayCamera;
            mainCamera.nearClipPlane = 0.01f;
            overlayCamera.nearClipPlane = 0.01f;
            UniversalAdditionalCameraData mainCameraAdditionalData = mainCamera.GetUniversalAdditionalCameraData();
            UniversalAdditionalCameraData overlayCameraAdditionalData = overlayCamera.GetUniversalAdditionalCameraData();
            mainCameraAdditionalData.renderPostProcessing = true;
            mainCameraAdditionalData.stopNaN = true;
            mainCameraAdditionalData.dithering = true;

            overlayCameraAdditionalData.renderPostProcessing = true;
            overlayCameraAdditionalData.stopNaN = true;
            overlayCameraAdditionalData.dithering = true;
            overlayCameraAdditionalData.renderType = CameraRenderType.Overlay;

            mainCameraAdditionalData.cameraStack.Add(overlayCamera);

            audioFiltersManager.LowPass = true;
            audioFiltersManager.HighPass = false;
            audioFiltersManager.Reverb = true;

            overlayCamera.gameObject.SetActive(false);

            //add interact ui
            Canvas canvas = cameraHolder.CreateChild("Interaction HUD").gameObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster graphicRaycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);

            CanvasGroup uiHolder = canvas.transform.CreateChild("Holder").gameObject.AddComponent<CanvasGroup>();
            RectTransform uiHolderRect = uiHolder.gameObject.AddComponent<RectTransform>();

            uiHolderRect.pivot = new Vector2(0.5f, 0.5f);
            uiHolderRect.anchorMax = new Vector2(0.5f, 0);
            uiHolderRect.anchorMin = new Vector2(0.5f, 0);
            uiHolderRect.sizeDelta = new Vector2(202.332f, 37.489f);
            uiHolderRect.anchoredPosition = new Vector2(0, 221);

            RectTransform header = uiHolder.transform.CreateChild("Header").gameObject.AddComponent<RectTransform>();
            Image headerImage = header.gameObject.AddComponent<Image>();

            headerImage.enabled = false;

            header.anchorMax = new Vector2(0.5f, 1);
            header.anchorMin = new Vector2(0.5f, 1);
            header.sizeDelta = new Vector2(110.26f, 37.65f);
            header.anchoredPosition = new Vector2(8.3821f, -18.5f);

            Color color = Color.black;
            color.a = 230;

            headerImage.color = color;

            HorizontalLayoutGroup headerHorizontalLayoutGroup = header.gameObject.AddComponent<HorizontalLayoutGroup>();
            ContentSizeFitter headerContentSizeFitter = header.gameObject.AddComponent<ContentSizeFitter>();
            headerHorizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            headerHorizontalLayoutGroup.padding = new RectOffset(5, 8, 5, 5);
            headerHorizontalLayoutGroup.spacing = 5;
            headerHorizontalLayoutGroup.childForceExpandHeight = false;
            headerHorizontalLayoutGroup.childForceExpandWidth = true;

            RectTransform btnName = header.CreateChild("Button Name").gameObject.AddComponent<RectTransform>();
            TextMeshProUGUI btnNameText = btnName.CreateChild("Name").gameObject.AddComponent<TextMeshProUGUI>();

            btnName.anchoredPosition = new Vector2(19.315f, -18.825f);
            btnName.sizeDelta = new Vector2(28.63f, 28.5f);
            btnName.anchorMax = new Vector2(0, 1);
            btnName.anchorMin = new Vector2(0, 1);
            btnName.pivot = new Vector2(0.5f, 0.5f);

            btnNameText.fontSize = 14;

            headerImage.color = Color.black;

            VerticalLayoutGroup btnNameverticalLayoutGroup = btnNameText.gameObject.AddComponent<VerticalLayoutGroup>();
            ContentSizeFitter btnNameTextContentSizeFitter = btnNameText.gameObject.AddComponent<ContentSizeFitter>();

            btnNameverticalLayoutGroup.padding = new RectOffset(0, 0, 0, 0);
            btnNameverticalLayoutGroup.childForceExpandHeight = false;

            btnNameTextContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            btnNameTextContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform interactTextTransform = header.CreateChild("Interact Text").gameObject.AddComponent<RectTransform>();
            Image btnImage = btnName.gameObject.AddComponent<Image>();

            btnImage.color = Color.black;

            VerticalLayoutGroup btnVerticalLayoutGroup = btnName.gameObject.AddComponent<VerticalLayoutGroup>();
            ContentSizeFitter btnContentSizeFitter = btnName.gameObject.AddComponent<ContentSizeFitter>();

            btnVerticalLayoutGroup.padding = new RectOffset(10, 10, 6, 6);

            TextMeshProUGUI interactText = interactTextTransform.gameObject.AddComponent<TextMeshProUGUI>();
            ContentSizeFitter interactContentSizeFitter = interactTextTransform.gameObject.AddComponent<ContentSizeFitter>();

            headerContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            headerContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            interactText.text = "Interact";
            btnNameText.text = "F";
            

            interactText.fontSize = 19;

            btnContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            btnContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            interactContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            interactContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            btnName.anchoredPosition = new Vector2(19.315f, -18.825f);
            btnName.sizeDelta = new Vector2(26.63f, 27.65f);

            interactTextTransform.anchoredPosition = new Vector2(70.445f, -18.825f);
            interactTextTransform.sizeDelta = new Vector2(63.63f, 21.23f);

            interactables.HUDObject = canvas.gameObject;
            interactables.interactKeyText = btnNameText;
            interactables.interactActionText = interactText;

            player.gameObject.layer = 8;
            foreach (Transform transform in player.transform) transform.gameObject.layer = 8;

            Vector3 position = Vector3.zero;

            player.transform.parent = Selection.activeTransform;
            player.transform.localPosition = SceneView.lastActiveSceneView.camera.transform.position + (SceneView.lastActiveSceneView.camera.transform.forward * 2.5f);
            fpsController.sensitivity = 500;

            Selection.activeTransform = player.transform;

            Undo.RegisterCreatedObjectUndo(player, "created player");
        }
    }
}