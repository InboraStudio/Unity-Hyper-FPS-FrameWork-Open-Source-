using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    /// <summary>
    /// base class for all weapons
    /// </summary>
    public abstract class Weapon : Item
    {
        /// <summary>
        /// State of weapon firing mode
        /// </summary>
        public enum FireMode
        {
            Auto = 0,
            SemiAuto = 1,
            Selective = 2
        }

        /// <summary>
        /// What to shot.
        /// </summary>
        public enum ShootingMechanism
        {
            Hitscan,
            Projectiles
        }

        /// <summary>
        /// Unit for fire rate.
        /// </summary>
        public enum FireRateUnit
        {
            RoundPerMinute = 60,
            RoundPerSecond = 1
        }

        /// <summary>
        /// Weapon bolt type.
        /// </summary>
        public enum RechargingType
        {
            GasPowerd = 0,
            Manual = 1
        }

        /// <summary>
        /// Type of reload. Manual needs animation events in order to function properly.
        /// </summary>
        public enum ReloadType
        {
            Default = 0,
            Scripted = 1
        }
    }
}