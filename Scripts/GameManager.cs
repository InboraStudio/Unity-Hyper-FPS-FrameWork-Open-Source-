using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Managers/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] UIManager HUD;
        [SerializeField] DeathCamera deathCamera;


        public UIManager UIManager { get; set; }
        public DeathCamera DeathCamera { get; set; }

        private void Awake()
        {
            if(HUD)
            UIManager = Instantiate(HUD);

            if(deathCamera)
            DeathCamera = Instantiate(deathCamera);
        }
    }
}