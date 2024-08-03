using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Inbora.FPSFramework.UI;
using UnityEngine;

namespace Inbora.FPSFramework.Examples
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Settings Menu/Default Setting UI Element Manager")]
    public class DefaultSettingUIElementManager : MonoBehaviour
    {
        public SettingTarget target;

        private DefaultSettingsPreset.DefaultSettingsPresetData presetData;
        private Slider slider;
        private Dropdown dropdown;
        private CarouselSelector carouselSelector;

        private void Start()
        {
            slider = transform.SearchFor<Slider>();
            dropdown = transform.SearchFor<Dropdown>();
            carouselSelector = transform.SearchFor<CarouselSelector>();

            presetData = SaveSystem.Load<DefaultSettingsPreset.DefaultSettingsPresetData>("Settings");
            if (presetData == null) return;

            int intValue = 0;
            float floatValue = 0;

            switch (target)
            {
                case SettingTarget.DisplayMode:
                    intValue = presetData.displayMode;
                    break;

                case SettingTarget.DisplayResolution:
                    intValue = presetData.displayResolution;
                    break;

                case SettingTarget.FrameLimit:
                    floatValue = presetData.frameLimit;
                    break;

                case SettingTarget.VerticalSync:
                    intValue = presetData.verticalSync;
                    break;

                case SettingTarget.RenderScale:
                    floatValue = presetData.renderScale;
                    break;

                case SettingTarget.MSAA:
                    intValue = presetData.msaa;
                    break;

                case SettingTarget.TextureQuality:
                    intValue = presetData.textureQuality;
                    break;

                case SettingTarget.TextureFiltering:
                    intValue = presetData.textureFiltering;
                    break;

                case SettingTarget.ShadowResolution:
                    intValue = presetData.shadowResolution;
                    break;

                case SettingTarget.ShadowDistance:
                    intValue = presetData.shadowDistance;
                    break;

                case SettingTarget.ShadowCascade:
                    intValue = presetData.shadowCascade;
                    break;

                case SettingTarget.SoftShadow:
                    intValue = presetData.softShadows;
                    break;

                case SettingTarget.HDR:
                    intValue = presetData.hdr;
                    break;

                case SettingTarget.ColorGradingMode:
                    intValue = presetData.colorGrading;
                    break;

                case SettingTarget.PostProcessing:
                    intValue = presetData.postProcessing;
                    break;

                case SettingTarget.Sensitivity:
                    floatValue = presetData.sensitivity;
                    break;

                case SettingTarget.XSensitivity:
                    floatValue = presetData.xSensitivity;
                    break;

                case SettingTarget.YSensitivity:
                    floatValue = presetData.ySensitivity;
                    break;

                case SettingTarget.FieldOfView:
                    floatValue = presetData.fieldOfView;
                    break;

                case SettingTarget.WeaponFieldOfView:
                    floatValue = presetData.weaponFieldOfView;
                    break;
            }

            if (slider) slider.value = floatValue;
            if (dropdown) dropdown.value = intValue;
            if (carouselSelector) carouselSelector.value = intValue;
        }

        public enum SettingTarget
        {
            DisplayMode,
            DisplayResolution,
            FrameLimit,
            VerticalSync,
            RenderScale,
            MSAA,
            TextureQuality,
            TextureFiltering,
            ShadowResolution,
            ShadowDistance,
            ShadowCascade,
            SoftShadow,
            HDR,
            ColorGradingMode,
            PostProcessing,
            Sensitivity,
            XSensitivity,
            YSensitivity,
            FieldOfView,
            WeaponFieldOfView
        }
    }
}