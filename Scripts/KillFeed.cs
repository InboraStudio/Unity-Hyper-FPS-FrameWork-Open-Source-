using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Kill Feed")]
    public class KillFeed : MonoBehaviour
    {
        public KillTag counter;
        public KillTag Tag;
        public Transform tagsHolder;
        public KillTag skull;
        public RectTransform skullsHolder;
        public bool useSFX;
        public AudioClip killSFX;
        public Color headshotColor = Color.red;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = killSFX;

            Tag.gameObject.SetActive(false);
            skull.gameObject.SetActive(false);
        }

        public void Show(Actor killer, string killed, bool headshot)
        {
            counter.Show(killer, killed);

            KillTag newTag = Instantiate(Tag, tagsHolder);
            KillTag newSkull = Instantiate(skull, skullsHolder);
            RawImage newSkullImage = newSkull.GetComponent<RawImage>();

            newSkullImage.color = headshot && newSkull.updateImageColors ? headshotColor : newSkullImage.color;
            newTag.message.color = headshot && newTag.updateImageColors ? headshotColor : newTag.message.color;


            newSkull.gameObject.SetActive(true);
            newTag.gameObject.SetActive(true);
            newTag.Show(killer, killed);
            newSkull.Show(killer, killed);

            if (killSFX && useSFX)
                audioSource.Play();
        }
    }
}