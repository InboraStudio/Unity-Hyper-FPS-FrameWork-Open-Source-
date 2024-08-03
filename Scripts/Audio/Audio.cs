using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public class Audio
    {
        public AudioProfile audioProfile;
        public AudioSource audioSource;
        public GameObject target;

        /// <summary>
        /// sets up the sound with profile settings this is recommended to be in OnEnable function
        /// </summary>
        /// <param name="target">target source of sound</param>
        /// <param name="profile">target sound with settings</param>
        public virtual void Equip(GameObject target, AudioProfile profile)
        {
            if (audioSource) return;
            this.target = target;
            audioSource = target.AddComponent<AudioSource>();
            audioProfile = profile;
            Update(profile);
        }

        /// <summary>
        /// plays a sound with it's profile settings
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        public virtual void Play(AudioProfile profile)
        {
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource.clip = profile.clip;
            audioSource.Play();
        }

        /// <summary>
        /// plays a sound with it's profile settings and if sound isn't equiped equips it
        /// </summary>
        /// <param name="target">target source of sound</param>
        /// <param name="profile">target sound with settings</param>
        public virtual void Play(GameObject target, AudioProfile profile)
        {
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource.clip = profile.clip;
            audioSource?.Play();
        }

        /// <summary>
        /// plays a sound with it's profile settings
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        /// <param name="clip">target audio clip to play</param>
        public virtual void Play(AudioProfile profile, AudioClip clip)
        {
            AudioClip prevClip = profile.clip;

            profile.clip = clip;
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource?.Play();

            profile.clip = prevClip;
        }

        /// <summary>
        /// plays a sound with it's profile settings and if sound isn't equiped equips it
        /// </summary>
        /// <param name="target">target source of sound</param>
        /// <param name="profile">target sound with settings</param>
        /// <param name="clip">target audio clip to play</param>
        public virtual void Play(GameObject target, AudioProfile profile, AudioClip clip)
        {
            Equip(target, profile);

            AudioClip prevClip = profile.clip;

            profile.clip = clip;
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource.Play();

            profile.clip = prevClip;
        }

        /// <summary>
        /// plays a sound with it's profile settings
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        public virtual void PlayOneShot(AudioProfile profile)
        {
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            if (!profile.clip)
            {
                return;
            }

            audioSource?.PlayOneShot(profile.clip);
        }

        /// <summary>
        /// plays a sound with it's profile settings and if sound isn't equiped equips it
        /// </summary>
        /// <param name="target">target source of sound</param>
        /// <param name="profile">target sound with settings</param>
        public virtual void PlayOneShot(GameObject target, AudioProfile profile)
        {
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource?.PlayOneShot(profile.clip);
        }

        /// <summary>
        /// plays a sound with it's profile settings
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        /// <param name="clip">target audio clip to play</param>
        public virtual void PlayOneShot(AudioProfile profile, AudioClip clip)
        {
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource?.PlayOneShot(clip);
        }

        /// <summary>
        /// plays a sound with it's profile settings and if sound isn't equiped equips it
        /// </summary>
        /// <param name="target">target source of sound</param>
        /// <param name="profile">target sound with settings</param>
        /// <param name="clip">target audio clip to play</param>
        public virtual void PlayOneShot(GameObject target, AudioProfile profile, AudioClip clip)
        {
            Equip(target, profile);
            Update(profile);
            RandomizePitch();
            InvokeCustomEvents(audioSource.pitch);

            audioSource?.PlayOneShot(clip);
        }

        /// <summary>
        /// pauses playing target sound
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        public virtual void Pause()
        {
            audioSource?.Pause();
        }

        /// <summary>
        /// unpauses target sound
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        public virtual void Unpause()
        {
            audioSource?.UnPause();
        }

        /// <summary>
        /// stops playing target audio
        /// </summary>
        /// <param name="profile">target sound with settings</param>
        public virtual void Stop()
        {
            if (!audioSource)
                audioSource?.Stop();
        }

        /// <summary>
        /// updates pitch with its defaults
        /// </summary>
        public virtual void Update(AudioProfile audioProfile)
        {
            this.audioProfile = audioProfile;
            if (!audioSource || audioSource && !audioProfile) return;
            audioSource.clip = audioProfile.clip;
            audioSource.outputAudioMixerGroup = audioProfile.output;
            audioSource.mute = audioProfile.mute;
            audioSource.bypassEffects = audioProfile.bypassEffects;
            audioSource.bypassListenerEffects = audioProfile.bypassListenerEffects;
            audioSource.bypassReverbZones = audioProfile.bypassReverbZones;
            audioSource.playOnAwake = audioProfile.playOnAwake;
            audioSource.loop = audioProfile.loop;

            audioSource.priority = audioProfile.priority;
            audioSource.volume = audioProfile.volume;
            audioSource.pitch = audioProfile.pitch;
            audioSource.panStereo = audioProfile.stereoPan;
            audioSource.spatialBlend = audioProfile.spatialBlend;
            audioSource.reverbZoneMix = audioProfile.reverbZoneMix;

            audioSource.dopplerLevel = audioProfile.dopplerLevel;
            audioSource.spread = audioProfile.spread;
            audioSource.rolloffMode = audioProfile.volumeRolloff;
            audioSource.minDistance = audioProfile.minDistance;
            audioSource.maxDistance = audioProfile.maxDistance;
        }

        public virtual void UpdatePitch()
        {
            Equip(target, audioProfile);
            audioSource.pitch = Time.timeScale * audioProfile.pitch;
        }

        /// <summary>
        /// sets sound pitch to random in order add some sounds variety without using too much audio clips 
        /// </summary>
        public virtual void RandomizePitch()
        {
            audioSource.pitch = RandomizedPitch();
        }

        /// <summary>
        /// returns random value bettwen using time scale
        /// </summary>
        /// <returns></returns>
        public virtual float RandomizedPitch()
        {
            if (audioProfile.dymaicPitch) 
                return Random.Range((Time.timeScale * audioProfile.pitch), (Time.timeScale + audioProfile.pitchFactor) * audioProfile.pitch);

            return audioProfile.pitch;
        }

        private async void InvokeCustomEvents(float pitch = 1)
        {
            if (audioProfile == null || audioProfile.clip == null) return;

            AudioClip audioClip = audioProfile.clip;
            float clipLenght = audioClip.length / pitch;

            float currentTime = 0;

            float current = 0;
            float previous = 0;

            while(currentTime < clipLenght + 1)
            {
                currentTime += Time.deltaTime;
                current = currentTime / clipLenght;

                foreach (AudioProfile.CustomAudioEvent customAudio in audioProfile.customEvents)
                {
                    if (current > customAudio.location && previous < customAudio.location)
                    {
                        customAudio.onLocate?.Invoke();
                    }
                }

                previous = current;
                await Task.Yield();
            }
        }
    }
}