using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Inbora.FPSFramework
{
    [ExecuteAlways, RequireComponent(typeof(RectTransform)), AddComponentMenu("Inbora/FPS Framework/UI/ImageSizeFitter")]
    public class ImageSizeFitter : MonoBehaviour
    {
        public RawImage RawImage { get; set; }
        public Image Image { get; set; }
        public Texture Texture { get; set; }
        private RectTransform RectTransform;

        private void Start()
        {
            RawImage = GetComponent<RawImage>();
            Image = GetComponent<Image>();

            if (RawImage)
            {
                RectTransform = RawImage.rectTransform;
            }

            if (Image)
            {
                RectTransform = Image.rectTransform;
            }
        }

        private void Update()
        {
            if (!Texture)
            {
                if (RawImage)
                {
                    Texture = RawImage.texture;
                }

                if (Image)
                {
                    Texture = Image.sprite ? Image.sprite.texture : null;
                }
            }

            if (!Image && !RawImage && !Application.isPlaying)
            {
                Image = GetComponent<Image>();
                RawImage = GetComponent<RawImage>();
            }


            if (RectTransform && Texture)
            {
                float percentage = (RectTransform.sizeDelta.magnitude / new Vector2(Texture.width, Texture.height).magnitude);

                RectTransform.sizeDelta = new Vector2(Texture.width, Texture.height) * percentage;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ImageSizeFitter))]
    public class ImageSizeFitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ImageSizeFitter sizeFitter = (ImageSizeFitter)target;

            if (!sizeFitter.Image && !sizeFitter.RawImage)
            {
                EditorGUILayout.HelpBox($"Require component Image or RawImage doesn't exist on game object ({sizeFitter.gameObject.name}) please add Image or RawImage to the game object.", MessageType.Error);
            }

            if (!sizeFitter.Texture && sizeFitter.Image)
            {
                EditorGUILayout.HelpBox($"Sprite can't be null on game object ({sizeFitter.gameObject.name}) please add Sprite to the Image in order to fit size.", MessageType.Warning);
            }

            if (!sizeFitter.Texture && sizeFitter.RawImage)
            {
                EditorGUILayout.HelpBox($"Texture can't be null on game object ({sizeFitter.gameObject.name}) please add Texture to the Raw Image in order to fit size.", MessageType.Warning);
            }
        }
    }
#endif
}