using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Health System/UI/Health Display")]
    public class HealthDisplay : MonoBehaviour
    {
        public bool lerp;
        public float smoothness = 5;

        public Slider slider;
        public Slider backgroundSlider;
        public TextMeshProUGUI actorNameText;

        private float health;


        private void Update()
        {
            slider.value = slider.value = lerp ? health : slider.value = health;
            if (backgroundSlider) backgroundSlider.value = backgroundSlider.value = lerp ? Mathf.Lerp(backgroundSlider.value, health, Time.deltaTime * smoothness) : backgroundSlider.value = health;
        }

        public void UpdateCard(float health, string actorName, bool lerpHealth)
        {
            actorNameText.text = actorName;

            if (lerpHealth) 
            {
                UpdateHealth(health);
            }
            else
            {
                UpdateHealthNoLerp(health);
            }
        }

        private void UpdateHealth(float health)
        {
            this.health = health;
        }

        private void UpdateHealthNoLerp(float health)
        {
            this.health = health;
            slider.value = health;
            if (backgroundSlider) backgroundSlider.value = health;
        }
    }
}