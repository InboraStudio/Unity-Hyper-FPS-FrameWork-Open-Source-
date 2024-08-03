using UnityEngine;

namespace Inbora.FPSFramework
{
    [CreateAssetMenu(fileName = "New Spray Pattern", menuName = "Inbora/FPS Framework/Weapons/Spray Pattern")]
    public class SprayPattern : ScriptableObject
    {
        public float amount = 1;
        public bool random = true;

        [Space]
        public AnimationCurve vertical = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(1, 0));

        public AnimationCurve horizontal = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(1, 0));

        /// <summary>
        /// returns random direction from a direction
        /// </summary>
        /// <param name="firearm"></param>
        /// <param name="direction">Where to randomize spray pattern</param>
        /// <returns></returns>
        public Vector3 GetPattern(Firearm firearm, Vector3 direction)
        {
            Vector3 targetSpread = Vector3.zero;

            if (random) targetSpread += Random.insideUnitSphere;
            else
            {
                targetSpread.x += vertical.Evaluate( firearm.GetReservePercentage());
                targetSpread.y += horizontal.Evaluate(firearm.GetReservePercentage());
            }

            return Vector3.Slerp(direction, targetSpread, GetAmount(firearm) / 180f);
        }

        public float GetPatternMagnitude(Firearm firearm, Vector3 direction)
        {
            return GetPattern(firearm, direction).magnitude;
        }

        public float GetAmount(Firearm firearm)
        {
            float finalAmount = amount;
            finalAmount *= firearm.attachmentsManager.spread;

            return finalAmount;
        }
    }
}