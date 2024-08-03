using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Inbora.FPSFramework.UI;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Get All Screen Resolution")]
    public class GetAllScreenResolution : MonoBehaviour
    {
        private Dropdown dropdown;
        private CarouselSelector carouselSelector;

        private void Start()
        {
            RegenrateList();
        }

        [ContextMenu("Regenrate List")]
        private void RegenrateList()
        {
            dropdown = GetComponent<Dropdown>();
            carouselSelector = GetComponent<CarouselSelector>();

            List<string> carouselSelectorOptions = new List<string>();
            List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();
            List<Resolution> resolutions = Screen.resolutions.ToList();

            dropdown?.ClearOptions();
            carouselSelector?.ClearOptions();

            foreach (Resolution resolution in FPSFrameworkUtility.GetResolutions())
            {
                string resText = $"{resolution.width}x{resolution.height} {resolution.refreshRate}Hz";

                carouselSelectorOptions.Add(resText);
                dropdownOptions.Add(new Dropdown.OptionData() { text = resText });
            }

            dropdown?.AddOptions(dropdownOptions);
            carouselSelector?.AddOptions(carouselSelectorOptions.ToArray());

            int currentResIndex = 0;
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].height == Screen.currentResolution.height
                    && resolutions[i].width == Screen.currentResolution.width
                    && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                {
                    currentResIndex = i;
                }
            }

            if (dropdown)
                dropdown.value = currentResIndex;
            if (carouselSelector)
                carouselSelector.value = currentResIndex;
        }
    }
}