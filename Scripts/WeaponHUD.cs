using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using ColorUtility = UnityEngine.ColorUtility;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Weapon HUD")]
    public class WeaponHUD : MonoBehaviour
    {
        public bool _reloadAlert;
        public bool _lowAmmoAlert;
        public bool _noAmmoAlert;
        public bool _ammoCount;
        public bool _gernadesCount;
        public bool _ammoName;
        public float fadeSpeed = 10;
        public TextMeshProUGUI WeaponName;
        public TextMeshProUGUI ammoName;
        public TextMeshProUGUI AmmoReserve;
        public TextMeshProUGUI AmmoAmount;
        public CanvasGroup reloadAlert;
        public CanvasGroup fireModeAlert;
        public Crosshair defaultCrosshair;

        public TextObject ammoAlerts;

        [Space]
        public Color lowAmmo = Color.yellow;
        public Color noAmmo = Color.red;

        private Color defaultAmmoReserveColor;
        private Color defaultAmmoAmountColor;

        public Firearm target { get; set; }
        public Crosshair Crosshair { get; set; }

        private void Start()
        {
            defaultAmmoReserveColor = AmmoReserve.color;
            defaultAmmoAmountColor = AmmoAmount.color;

            reloadAlert.alpha = 0;

            target.events.OnFireModeChange.AddListener(ShowFireModeAlert);

            AmmoReserve.gameObject.SetActive(_ammoCount);
            AmmoAmount.gameObject.SetActive(_ammoCount);
            ammoName.gameObject.SetActive(_ammoName);

            if (defaultCrosshair)
                Crosshair = Instantiate(defaultCrosshair, transform);
        }

        public void UpdateUI()
        {
            WeaponName.text = target.preset.Name;
            ammoName.text = target.AmmoProfile.data.Name;

            AmmoReserve.text = target.remainingAmmoCount.ToString();
            AmmoAmount.text = target.AmmoProfile.amount.ToString();

            if (target.remainingAmmoCount <= target.magazineCapacity * 0.3f && target.remainingAmmoCount > 0)
            {
                if (_lowAmmoAlert) ammoAlerts.FadeIn("LOW AMMO", lowAmmo, fadeSpeed);
                AmmoReserve.color = Color.Lerp(AmmoReserve.color, noAmmo, Time.deltaTime * fadeSpeed);
            }

            if (target.AmmoProfile.amount <= target.magazineCapacity)
            {
                AmmoAmount.color = Color.Lerp(AmmoAmount.color, noAmmo, Time.deltaTime * fadeSpeed);
            }
            else
            {
                AmmoAmount.color = Color.Lerp(AmmoAmount.color, defaultAmmoAmountColor, Time.deltaTime * fadeSpeed);
            }

            if (target.remainingAmmoCount <= target.magazineCapacity * 0.3f && target.remainingAmmoCount > 0 && target.AmmoProfile.amount > 0 && _reloadAlert)
            {
                if (_reloadAlert) reloadAlert.alpha = Mathf.Lerp(reloadAlert.alpha, 1, Time.deltaTime * fadeSpeed);
            }

            if (target.remainingAmmoCount <= 0 && _noAmmoAlert)
            {
                if (_noAmmoAlert) ammoAlerts.FadeIn("NO AMMO", noAmmo, fadeSpeed);
                AmmoReserve.color = Color.Lerp(AmmoReserve.color, noAmmo, Time.deltaTime * fadeSpeed);
            }

            if (target.remainingAmmoCount > target.magazineCapacity * 0.3f)
            {
                ammoAlerts.FadeOut(fadeSpeed);
                reloadAlert.alpha = Mathf.Lerp(reloadAlert.alpha, 0, Time.deltaTime * fadeSpeed);
                AmmoReserve.color = Color.Lerp(AmmoReserve.color, defaultAmmoReserveColor, Time.deltaTime * fadeSpeed);
            }

            if (fireModeAlert.alpha >= 1)
            {
                if (!IsInvoking(nameof(HideFireModeAlert)))
                    Invoke(nameof(HideFireModeAlert), 1.5f);
            }
        }

        public void ShowFireModeAlert()
        {
            string text = $"Fire Mode: {target.currentSelectiveFireMode}";
            
            fireModeAlert.alpha = 1;
            fireModeAlert.SearchFor<TextMeshProUGUI>().text = text;
        }

        private void HideFireModeAlert()
        {
            fireModeAlert.alpha = 0;
        }

        private void HideAmmoName()
        {
            ammoName.color = Color.clear;
        }
    }

    [System.Serializable]
    public class TextObject
    {
        public TextMeshProUGUI text;
        public CanvasGroup canvasGroup;

        public void FadeIn(string text, float speed)
        {
            this.text.text = text;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * speed);
        }

        public void FadeIn(string text, Color color, float speed)
        {
            this.text.text = text;
            this.text.color = color;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * speed);
        }

        public void FadeOut(float speed)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * speed);
        }
    }
}