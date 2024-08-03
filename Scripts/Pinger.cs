using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Pinger")]
    public class Pinger : MonoBehaviour
    {
        public InputAction inputAction;
        public LayerMask pingableLayers = -1;
        public Ping ping;
        public Canvas canvas;
        public float range = 100;
        public float pingLifetime = 15;
        public float maxPings = 5;

        public List<Ping> pings = new List<Ping>();
        private Audio pingAudio = new Audio();

        private void Start()
        {
            pingAudio.Equip(gameObject, null);
            inputAction.Enable();
            inputAction.performed += context => LookAndPing();
        }

        private void LookAndPing()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, pingableLayers))
            {
                Ping newPing = Instantiate(ping, canvas.transform);
                newPing.GetComponent<FloatingRect>().position = hit.point;
                pings.Add(newPing);

                if (ping.soundEffect)
                    PlayPingSoundEffect(newPing);
                
                OnPinged(newPing);
            }
        }

        public virtual void OnPinged(Ping ping)
        {

        }

        public virtual void PlayPingSoundEffect(Ping ping)
        {
            pingAudio.PlayOneShot(ping.soundEffect);
        }
    }
}
