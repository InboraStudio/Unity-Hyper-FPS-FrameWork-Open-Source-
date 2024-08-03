using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Inbora.FPSFramework
{
    [ExecuteAlways, AddComponentMenu("Inbora/FPS Framework/UI/Get Name")]
    public class GetName : MonoBehaviour
    {
        public Transform target;
        private TextMeshProUGUI text;
        private Text legecyText;

        private void Start()
        {
            if (!target) target = transform.parent;
            text = GetComponent<TextMeshProUGUI>();
            legecyText = GetComponent<Text>();
        }

        private void Update()
        {
            if (text != null)
                text.text = target.name;

            if (legecyText != null)
                legecyText.text = target.name;
        }
    }
}