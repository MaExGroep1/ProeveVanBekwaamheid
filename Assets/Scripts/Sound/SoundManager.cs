using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Sound
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Main Settings")]
        [SerializeField] private AudioSource soundObject; // The AudioSource used for sound playback

        /// <summary>
        /// Plays a specific sound clip at the given spawn point.
        /// </summary>
        /// <param name="audioClip">The audio clip to be played</param>
        /// <param name="soundObjectSpawn">The transform position where the sound should be played</param>
        /// <param name="volume">The volume level for the sound</param>
        /// <param name="loop">If true, will loop the clip indefinitely</param>
        public void PlaySoundClip(AudioClip audioClip, Transform soundObjectSpawn, float volume, bool loop = false)
        {
            var clipLength = audioClip.length;
            var audioSource = Instantiate(soundObject, null);
            
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            if (loop)
            {
                audioSource.loop = true;
                return;
            }
            Destroy(audioSource.gameObject, clipLength);
        }

        /// <summary>
        /// Plays a random clip from an array of audio clips at the specified location.
        /// </summary>
        /// <param name="audioClips">Array of audio clips to choose from</param>
        /// <param name="soundObjectSpawn">The transform position where the sound should be played</param>
        /// <param name="volume">The volume level for the sound</param>
        /// <param name="loop">If true, will loop the clip indefinitely</param>
        public void PlayRandomClip(AudioClip[] audioClips, Transform soundObjectSpawn, float volume, bool loop = false)
        {
            if (audioClips.Length == 0) return;
            
            var randClip = audioClips[Random.Range(0, audioClips.Length)];
            PlaySoundClip(randClip, soundObjectSpawn, volume, loop);
        }
    }
}
