using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Inbora.FPSFramework.Examples;

namespace Inbora.FPSFramework
{
    public static class FPSFrameworkUtility
    {
        /// <summary>
        /// Returns the max refresh rate.
        /// </summary>
        /// <returns></returns>
        public static int GetRefreshRate()
        {
            int result = 0;
            Resolution[] resolutions = Screen.resolutions;

            foreach(Resolution res in resolutions)
            {
                if (res.refreshRate > result) result = res.refreshRate;
            }

            return result;
        }

        /// <summary>
        /// returns an array of resolutions in reverse
        /// </summary>
        /// <returns></returns>
        public static Resolution[] GetResolutions()
        {
            List<Resolution> resolutions = new List<Resolution>();

            foreach(Resolution res in Screen.resolutions)
            {
                if (res.width >= 800 && res.height >= 600 && res.refreshRate >= GetRefreshRate())
                {
                    resolutions.Add(res);
                }
            }

            resolutions.Reverse();
            return resolutions.ToArray();
        }

        /// <summary>
        /// returns false if the game isn't paused or there's no pause menu in the scene
        /// </summary>
        /// <returns></returns>
        public static bool IsPaused()
        {
            if (PauseMenu.Instance && PauseMenu.Instance.paused)
                return true;

            return false;
        }

        public static float sensitivityMultiplier { get; set; } = 100;
        public static float xSensitivityMultiplier { get; set; } = 100;
        public static float ySensitivityMultiplier { get; set; } = 100;
        public static float fieldOfView { get; set; } = 60;
        public static float weaponFieldOfView { get; set; } = 50;

    }
}