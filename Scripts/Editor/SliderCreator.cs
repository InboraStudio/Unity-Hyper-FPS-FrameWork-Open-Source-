using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Inbora.FPSFramework.UI;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace Inbora.FPSFramework
{
    public class SliderCreator : MonoBehaviour
    {
        [MenuItem(MenuItemPaths.Create + "UI/Slider")]
        public static void CreateSlider()
        {
            Canvas canvas = FPSFrameworkEditor.FindOrCreateCanves();
            GameObject holder = new GameObject("Slider");
            holder.transform.SetParent(canvas.transform);
            holder.layer = 5;

            RectTransform sliderRectTransform = holder.AddComponent<RectTransform>();
            sliderRectTransform.Reset();

            sliderRectTransform.sizeDelta = new Vector2(160, 20);

            Slider slider = holder.AddComponent<Slider>();

            Image background = new GameObject("Background").AddComponent<Image>();
            background.rectTransform.SetParent(holder.transform);
            background.rectTransform.Reset();

            background.color = new Color(0.18f, 0.18f, 0.18f, 1);
            background.rectTransform.anchorMax = new Vector2(1, 1);
            background.rectTransform.anchorMin = new Vector2(0, 0);

            background.rectTransform.sizeDelta = Vector2.one;

            RectTransform fillArea = new GameObject("Fill Area").AddComponent<RectTransform>();
            fillArea.SetParent(holder.transform);
            fillArea.Reset();

            fillArea.anchorMax = new Vector2(1, 1);
            fillArea.anchorMin = new Vector2(0, 0);

            fillArea.sizeDelta = Vector2.one;

            Image fill = new GameObject("Fill").AddComponent<Image>();
            fill.rectTransform.SetParent(fillArea.transform);
            fill.rectTransform.Reset();

            fill.color = Color.white;
            fill.rectTransform.anchorMax = new Vector2(0.5f, 1);
            fill.rectTransform.anchorMin = new Vector2(0, 0);

            fill.rectTransform.sizeDelta = Vector2.one;

            slider.fillRect = fill.rectTransform;
            slider.value = 0.5f;

            TextMeshProUGUI valueText = new GameObject("Value Text").AddComponent<TextMeshProUGUI>();
            valueText.text = "0.5";

            valueText.rectTransform.SetParent(holder.transform);
            valueText.rectTransform.Reset();

            valueText.rectTransform.anchoredPosition = new Vector2(-125f, 0);
            valueText.rectTransform.sizeDelta = new Vector2(67.587f, 20f);

            valueText.rectTransform.anchorMin = Vector2.one * 0.5f;
            valueText.rectTransform.anchorMax = Vector2.one * 0.5f;

            valueText.fontSize = 14;
            valueText.alignment = TextAlignmentOptions.Right;
            valueText.alignment = TextAlignmentOptions.Center;

            SliderToTMPro sliderToTMPro = holder.AddComponent<SliderToTMPro>();

            Selection.activeGameObject = holder;
        }
    }
}