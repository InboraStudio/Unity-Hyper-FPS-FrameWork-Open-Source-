using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/UI Mananger")]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public bool _damageIndicator = true;
        public bool _healthDisplay = true;
        public bool _hitmarker = true;
        public bool _killFeed = true;

        [Space]
        public bool hitmarkerSFX = true;
        public bool killSFX = true;

        public DamageIndicator DamageIndicator { get; set; }
        public HealthDisplay HealthDisplay { get; set; }
        public Hitmarker Hitmarker { get; set; }
        public KillFeed KillFeed { get; set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            DamageIndicator = GetComponentInChildren<DamageIndicator>();
            HealthDisplay = GetComponentInChildren<HealthDisplay>();
            Hitmarker = GetComponentInChildren<Hitmarker>();
            KillFeed = GetComponentInChildren<KillFeed>();
        }

        private void Update()
        {
            DamageIndicator.gameObject.SetActive(_damageIndicator);
            HealthDisplay.gameObject.SetActive(_healthDisplay);
            Hitmarker.gameObject.SetActive(_hitmarker);
            KillFeed.gameObject.SetActive(_killFeed);

            Hitmarker.useSFX = hitmarkerSFX;
            KillFeed.useSFX = killSFX;
        }

        /// <summary>
        /// updates player name in player's card
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            HealthDisplay.actorNameText.text = name;
        }

        public void LoadGame(string name)
        {
            StartCoroutine(LoadingScreen.Instance.LoadSceneAsync(name));
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}