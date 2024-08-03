using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    /// <summary>
    /// This class will have almost all of the data yout need to interact with the hits from projectile or explosion
    /// </summary>
    public class HitInfo
    {
        public Ray ray { get; set; }
        public RaycastHit raycastHit { get; set; }

        /// <summary>
        /// If hit was done from a projectile this will be the source. Use this to get data about the hit.
        /// </summary>
        public Projectile projectile;

        /// <summary>
        /// If hit was done from an explosive this will be the source. Use this to get data about the hit.
        /// </summary>
        public Explosive explosive;

        public HitInfo(Projectile projectile, Ray ray, RaycastHit raycastHit)
        {
            this.ray = ray;
            this.raycastHit = raycastHit;
            this.projectile = projectile;
        }

        public HitInfo(Explosive explosive, Ray ray, RaycastHit raycastHit)
        {
            this.ray = ray;
            this.raycastHit = raycastHit;
            this.explosive = explosive;
        }
    }
}