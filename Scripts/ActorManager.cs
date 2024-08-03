using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    /// <summary>
    /// this class is responsible for actor statics and respawning
    /// </summary>
    [AddComponentMenu("Inbora/FPS Framework/Player/Actor Manager")]
    [DisallowMultipleComponent()]
    public class ActorManager : MonoBehaviour
    {
        /// <summary>
        /// the actor which is meant to be managed
        /// </summary>
        public Actor actor { get; set; }

        [Header("Respawn")]
        [Tooltip("If on actor will respawn on death")]
        public bool respawnable;
        [Tooltip("If on actor will spawn on awake from the assigned actor or the children actor")]
        public bool spawnOnAwake;
        [Tooltip("The prefab which will be instantiated when spawn function is called.")]
        public Actor actorPrefab;
        [Tooltip("Which team will the actor respwan at this is set from the Spwan manager in the scene")]
        public int teamID;

        public List<Weapon> defaultItems;
        
        [Space, Tooltip("If on the actor will pick a random name from (Random Names) list")]
        public bool randomizeName;
        public List<string> randomNames = new List<string>(1);


        [Header("Statics")]
        [Tooltip("How much the actor has killed")]
        public int kills;
        [Tooltip("How much the actor got killed")]
        public int deaths;

        public bool isLocallyMine
        {
            get
            {
                return actor?.characterManager?.character != null;
            }
        }

        /// <summary>
        /// name of the actor
        /// </summary>
        public string ActorName { get; set; }
        public SpawnManager SpwanManager { get; set; }


        private void Start()
        {
            if (!actorPrefab) actorPrefab = GetComponentInChildren<Actor>();

            SpwanManager = SpawnManager.Instance;

            UIManager.Instance?.SetName(ActorName);

            if (randomizeName && randomNames.Count <= 0)
            {
                randomizeName = false;
                ActorName = $"Actor {Random.Range(0, 1000)}";

                Debug.Log($"Actor Manager for {name} doesn't have any random names random namer will be disabled");
            }

            if (randomizeName)
            {
                ActorName = randomNames[Random.Range(0, randomNames.Count - 1)];
            }

            if (spawnOnAwake)
            {
                foreach (Transform t in transform) t.gameObject.SetActive(false);
                Spwan();
            }
        }
        /// <summary>
        /// respawn an actor with a delay
        /// </summary>
        /// <param name="delay">the delay to respawn</param>
        public void Respwan(float delay)
        {
            Invoke(nameof(SpwanActor), delay);
        }

        /// <summary>
        /// adds a new actor this is used to avoid using IEnumerators
        /// </summary>
        private void SpwanActor()
        {
            Spwan();
        }

        /// <summary>
        /// spawns an actor with no delay
        /// </summary>
        public Actor Spwan()
        {
            int index = Random.Range(0, SpwanManager.sides[teamID].points.Length);

            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            pos = SpwanManager.sides[teamID].points[index].position;
            rot = SpwanManager.sides[teamID].points[index].rotation;

            pos += Random.insideUnitSphere * SpwanManager.spawnRadius * SpwanManager.transform.lossyScale.magnitude;
            pos.y = SpwanManager.sides[teamID].points[index].position.y;

            Actor newActor = Instantiate(actorPrefab, pos, rot, transform);

            if (randomizeName)
            {
                newActor.actorName = ActorName;
            }

            newActor.gameObject.SetActive(true);

            return newActor;
        }
    }
}