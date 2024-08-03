using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;

namespace Inbora.FPSFramework.Examples
{

    [CreateAssetMenu(fileName = "New Settings Preset", menuName = "Inbora/FPS Framework/Settings Preset")]
    public class DefaultSettingsPreset : SettingsPreset
    {

        private const string JsonFileName = "Settings";
        protected DefaultSettingsPresetData data = new DefaultSettingsPresetData();

        public UniversalRenderPipelineAsset GetUniversalRenderPipelineAsset()
        {
            return ((UniversalRenderPipelineAsset)QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()));
        }

        public override void Save()
        {
            SaveSystem.Save(data, JsonFileName);
        }

        public override void Load()
        {
            DefaultSettingsPresetData data = SaveSystem.Load<DefaultSettingsPresetData>(JsonFileName);
            
            if (data == null) data = new DefaultSettingsPresetData();

            SetDisplayMode(data.displayMode);
            SetDisplayResolution(data.displayResolution);
            SetFrameLimit(data.frameLimit);
            SetVerticalSync(data.verticalSync);
            SetRenderScale(data.renderScale);
            SetMSAA(data.msaa);
            SetTextureQuality(data.textureQuality);
            SetTextureFiltering(data.textureFiltering);
            SetShadowResolution(data.shadowResolution);
            SetShadowDistance(data.shadowDistance);
            SetShadowCascade(data.shadowCascade);
            SetSoftShadow(data.softShadows);
            SetHDR(data.hdr);
            SetGradingMode(data.colorGrading);
            SetPostProcssing(data.postProcessing);
            SetSensitivityMultiplier(data.sensitivity);
            SetXSensitivityMultiplier(data.xSensitivity);
            SetYSensitivityMultiplier(data.ySensitivity);
        }

        public void SetDisplayMode(int value)
        {
            data.displayMode = value;
            Screen.fullScreenMode = (FullScreenMode)value;
        }

        public void SetDisplayResolution(int value)
        {
            data.displayResolution = value;
            List<Resolution> resolutions = FPSFrameworkUtility.GetResolutions().ToList();

            Resolution resolution = resolutions[value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFrameLimit(float value)
        {
            data.frameLimit = value;
            int _value = (int)value;
            Application.targetFrameRate = _value;
        }

        public void SetVerticalSync(int value)
        {
            data.verticalSync = value;
            int resultValue = 0;

            //TODO: Improve this method
            if (value == 0) resultValue = 2;
            if (value == 1) resultValue = 1;
            if (value == 2) resultValue = 0;

            QualitySettings.vSyncCount = resultValue;
        }

        public void SetRenderScale(float value)
        {
            data.renderScale = value;
            GetUniversalRenderPipelineAsset().renderScale = value / 100;
        }

        public void SetMSAA(int value)
        {
            data.msaa = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();

            int count = 0;
            if (value == 0) count = 8;
            if (value == 1) count = 4;
            if (value == 2) count = 2;
            if (value == 3) count = 1;

            asset.msaaSampleCount = count;
        }

        public void SetTextureQuality(int value)
        {
            data.textureQuality = value;
            QualitySettings.globalTextureMipmapLimit = value;
        }

        public void SetTextureFiltering(int value)
        {
            data.textureFiltering = value;
            int resultValue = 0;

            //TODO: Improve this method
            if (value == 0) resultValue = 2;
            if (value == 1) resultValue = 1;
            if (value == 2) resultValue = 0;

            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)resultValue;
        }

        public void SetShadowResolution(int value)
        {
            data.shadowResolution = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo mainLightShadowResolutionFieldInfo = asset.GetType().GetField("m_MainLightShadowmapResolution", flags);
            FieldInfo additionalLightShadowResolutionFieldInfo = asset.GetType().GetField("m_AdditionalLightsShadowmapResolution", flags);
            FieldInfo mainLightShadowsSupportedFieldInfo = asset.GetType().GetField("m_MainLightShadowsSupported", flags);
            FieldInfo additionalLightShadowsSupportedFieldInfo = asset.GetType().GetField("m_AdditionalLightShadowsSupported", flags);

            int resolution = 0;
            bool shadowSupported = true;

            if (value == 0) resolution = 4096;
            if (value == 1) resolution = 2048;
            if (value == 2) resolution = 1024;
            if (value == 3) resolution = 512;
            if (value == 4) resolution = 256;
            if (value == 5 || value > 5)
            {
                shadowSupported = false;
                resolution = 256;
            }
            mainLightShadowResolutionFieldInfo.SetValue(asset, resolution);
            additionalLightShadowResolutionFieldInfo.SetValue(asset, resolution);

            mainLightShadowsSupportedFieldInfo.SetValue(asset, shadowSupported);
            additionalLightShadowsSupportedFieldInfo.SetValue(asset, shadowSupported);
        }

        public void SetShadowDistance(int value)
        {
            data.shadowDistance = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();

            float distance = 0;
            if (value == 0) distance = 200;
            if (value == 1) distance = 150;
            if (value == 2) distance = 100;
            if (value == 3) distance = 50;
            if (value == 4) distance = 30;

            asset.shadowDistance = distance;
        }

        public void SetShadowCascade(int value)
        {
            data.shadowCascade = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo shadowCascadeCountFieldInfo = asset.GetType().GetField("m_ShadowCascadeCount", flags);

            int count = 0;
            if (value == 0) count = 4;
            if (value == 1) count = 3;
            if (value == 2) count = 2;
            if (value == 3) count = 1;

            shadowCascadeCountFieldInfo.SetValue(asset, count);
        }

        public void SetSoftShadow(int value)
        {
            data.softShadows = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo softShadowFieldInfo = asset.GetType().GetField("m_SoftShadowsSupported", flags);

            bool softShadow = value == 0;

            softShadowFieldInfo.SetValue(asset, softShadow);

        }

        public void SetHDR(int value)
        {
            data.hdr = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();
            bool HDR = value == 0;

            asset.supportsHDR = HDR;
        }

        public void SetGradingMode(int value)
        {
            data.colorGrading = value;
            UniversalRenderPipelineAsset asset = GetUniversalRenderPipelineAsset();
            int gradingModeResultValue = 0;
            int gradingLutSizeResultValue = 0;
            if (value == 0)
            {
                gradingModeResultValue = 1;
                gradingLutSizeResultValue = 65;
            }

            if (value == 1)
            {
                gradingModeResultValue = 0;
                gradingLutSizeResultValue = 16;
            }

            asset.colorGradingMode = (ColorGradingMode)gradingModeResultValue;
            asset.colorGradingLutSize = gradingLutSizeResultValue;
        }

        public void SetPostProcssing(int value)
        {
            data.postProcessing = value;
            Volume[] volumes = FindObjectsOfType<Volume>();
            Volume globalVolume = null;

            foreach (Volume volume in volumes)
            {
                if (volume.isGlobal) globalVolume = volume;

                volume.enabled = value != 5;
            }

            float weight = 1;
            if (value == 0) weight = 1;
            if (value == 1) weight = 0.7f;
            if (value == 2) weight = 0.5f;
            if (value == 3) weight = 0.3f;
            if (value == 4) weight = 0.1f;
            if (value == 5) weight = 0;

            if (globalVolume) globalVolume.weight = weight;
        }

        public void SetSensitivityMultiplier(float value)
        {
            data.sensitivity = value;
            FPSFrameworkUtility.sensitivityMultiplier = value / 100;
        }

        public void SetXSensitivityMultiplier(float value)
        {
            data.xSensitivity = value;
            FPSFrameworkUtility.xSensitivityMultiplier = value / 100;
        }

        public void SetYSensitivityMultiplier(float value)
        {
            data.ySensitivity = value;
            FPSFrameworkUtility.ySensitivityMultiplier = value / 100;
        }

        public void SetFieldOfView(float value)
        {
            data.fieldOfView = value;
            FPSFrameworkUtility.fieldOfView = value;
        }

        public void SetWeaponFieldOfView(float value)
        {
            data.weaponFieldOfView = value;
            FPSFrameworkUtility.weaponFieldOfView = value;
        }

        [Serializable]
        public class DefaultSettingsPresetData
        {
            public int displayMode = 0;
            public int displayResolution = 0;
            public float frameLimit = 240;
            public int verticalSync = 0;
            public float renderScale = 100;
            public int msaa = 8;
            public int textureQuality = 0;
            public int textureFiltering = 0;
            public int shadowResolution = 0;
            public int shadowDistance = 0;
            public int shadowCascade = 0;
            public int softShadows = 0;
            public int hdr = 0;
            public int colorGrading = 0;
            public int postProcessing = 0;
            public float sensitivity = 100f;
            public float xSensitivity = 100f;
            public float ySensitivity = 100f;
            public float fieldOfView = 60;
            public float weaponFieldOfView = 50;
        }
    }
}