using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using Inbora.FPSFramework.UI;
using TMPro;

namespace Inbora.FPSFramework
{
    public class CarouselSelectorCreator : MonoBehaviour
    {
        [MenuItem(MenuItemPaths.Create + "UI/Carousel Selector")]
        public static void CreateCarouselSelector()
        {
            Canvas canvas = FPSFrameworkEditor.FindOrCreateCanves();
            GameObject holder = new GameObject("Carousel Selector");
            holder.transform.SetParent(canvas.transform);

            RectTransform holderRectTransform = holder.gameObject.AddComponent<RectTransform>();
            holderRectTransform.SetParent(Selection.activeTransform);
            
            holderRectTransform.Reset();
            holderRectTransform.localScale = Vector3.one;

            holderRectTransform.sizeDelta = new Vector2(160, 45);



            Image holderBackground = holder.AddComponent<Image>();
            holderBackground.color = new Color(0, 0, 0, 0.39f);


            CarouselSelector dropdownSelector = holder.AddComponent<CarouselSelector>();
            //dropdownSelector.normalSelectionColor = new Color(0, 0, 0, 0.6f);
            //dropdownSelector.selectedSelectionColor = Color.white;
            dropdownSelector.AddOptions(new string[] { "Option A", "Option B"});


            RectTransform selections = new GameObject("Selections").AddComponent<RectTransform>();
            selections.SetParent(holder.transform);
            selections.Reset();

            HorizontalLayoutGroup selectionsHorizontalLayoutGroup = selections.gameObject.AddComponent<HorizontalLayoutGroup>();
            selectionsHorizontalLayoutGroup.spacing = 5;
            selectionsHorizontalLayoutGroup.childAlignment = TextAnchor.UpperRight;
            selectionsHorizontalLayoutGroup.childControlWidth = true;

            selections.anchorMin = new Vector2(0, 0);
            selections.anchorMax = new Vector2(1, 0);

            selections.anchoredPosition = new Vector3(0, 1.392f, 0);
            selections.sizeDelta = new Vector2(0, 2.784f);

            Image selected = new GameObject("Option").AddComponent<Image>();
            selected.rectTransform.SetParent(selections);
            selected.rectTransform.sizeDelta = new Vector2(160, 2.378f);

            selected.rectTransform.localScale = Vector2.one;


            TextMeshProUGUI label = new GameObject("Label").AddComponent<TextMeshProUGUI>();
            label.rectTransform.SetParent(holder.transform);
            label.rectTransform.Reset();
            label.rectTransform.localScale = Vector3.one;

            label.rectTransform.anchoredPosition = new Vector2(44.123f, 1.25f);

            label.rectTransform.sizeDelta = new Vector2(88.245f, 42.5f);

            label.rectTransform.anchorMax = new Vector2(0, 0.5f);
            label.rectTransform.anchorMin = new Vector2(0, 0.5f);

            label.text = "Option A";
            label.fontSize = 18;
            label.alignment = TextAlignmentOptions.Right;


            Button rightButton = new GameObject("Right Button").AddComponent<Button>();
            RectTransform rBRect = rightButton.gameObject.AddComponent<RectTransform>();
            rBRect.SetParent(holder.transform);
            rBRect.Reset();
            rBRect.localScale = Vector3.one;

            ColorBlock rbCB = new ColorBlock();
            

            rbCB.normalColor = new Color(0.56f, 0.56f, 0.56f, 1);
            rbCB.highlightedColor = new Color(0.18f, 0.18f, 0.18f, 1);
            rbCB.pressedColor = Color.gray;
            rbCB.selectedColor = Color.white;
            rbCB.colorMultiplier = 1;

            rbCB.fadeDuration = 0;

            rightButton.colors = rbCB;

            rBRect.anchoredPosition = new Vector2(-20.00002f, 1.525879e-05f);
            rBRect.sizeDelta = new Vector2(20, 20);

            rBRect.anchorMax = new Vector2(1, 0.5f);
            rBRect.anchorMin = new Vector2(1, 0.5f);
            rightButton.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            Image rBImage = rightButton.gameObject.AddComponent<Image>();
            rightButton.targetGraphic = rBImage;



            Button leftButton = new GameObject("Left Button").AddComponent<Button>();
            RectTransform lBRect = leftButton.gameObject.AddComponent<RectTransform>();
            lBRect.SetParent(holder.transform);
            lBRect.Reset();
            lBRect.localScale = Vector3.one;

            ColorBlock lbCB = new ColorBlock();


            lbCB.normalColor = new Color(0.56f, 0.56f, 0.56f, 1);
            lbCB.highlightedColor = new Color(0.18f, 0.18f, 0.18f, 1);
            lbCB.pressedColor = Color.gray;
            lbCB.selectedColor = Color.white;
            lbCB.colorMultiplier = 1;

            lbCB.fadeDuration = 0;

            leftButton.colors = lbCB;

            lBRect.anchoredPosition = new Vector2(-48.6f, 1.525879e-05f);
            lBRect.sizeDelta = new Vector2(20, 20);

            lBRect.anchorMax = new Vector2(1, 0.5f);
            lBRect.anchorMin = new Vector2(1, 0.5f);
            leftButton.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

            Image lBImage = leftButton.gameObject.AddComponent<Image>();
            leftButton.targetGraphic = lBImage;


            dropdownSelector.label = label;
            dropdownSelector.rightButton = rightButton;
            dropdownSelector.leftButton = leftButton;
            //dropdownSelector.selectionGraphics = selected;
            //dropdownSelector.selectionHolder = selections;

            Selection.activeTransform = holderRectTransform;
        }
    }
}
