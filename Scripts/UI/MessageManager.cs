using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework.UI
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Message Manager")]
    public class MessageManager : MonoBehaviour
    {
        public Message message;

        private void Awake()
        {
            message?.gameObject.SetActive(false);
        }

        /// <summary>
        /// Adds a message to this message manager
        /// </summary>
        /// <param name="content"></param>
        public void AddMessage(string content)
        {
            Message newMessage = Instantiate(message, transform);
            newMessage.gameObject.SetActive(true);
            newMessage.SetMessage(content);
        }
    }
}