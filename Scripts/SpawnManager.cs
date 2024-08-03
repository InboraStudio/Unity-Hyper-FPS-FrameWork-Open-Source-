using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Managers/Spwan Manager")]
    public class SpawnManager : MonoBehaviour
    {
        public float spawnRadius = 5;
        public float respawnDelay = 5;
        public SpwanPoint[] sides;

        public static SpawnManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            foreach (SpwanPoint point in sides)
            {
                foreach (Transform transform in point.points)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(transform.position, spawnRadius * transform.lossyScale.magnitude);
                }
            }
        }

        [System.Serializable]
        public class SpwanPoint
        {
            public Transform[] points;
        }
    }
}