using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        public static AudioManager Instance { get; private set; }
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public bool IsPlayingMusic()
        {
            if (musicSource.clip == null) return false;
            else return true;
        }
        
        public void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            sfxSource.volume = volume;
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip);
        }

        public void PlaySfxLoop(AudioClip clip, float volume = 1f)
        {
            sfxSource.clip = clip;
            sfxSource.loop = true;
            sfxSource.volume = volume;
            sfxSource.Play();
        }

        public void StopSfxLoop()
        {
            sfxSource.loop = false;
            sfxSource.Stop();
        }

        public void SetSfxPitch(float pitch = 1f)
        {
            sfxSource.pitch = pitch;
        }

        public void ChangeMusic(AudioClip music, float volume = 1f, float fadeOutDuration = 1f, float fadeInDuration = 3f)
        {
            StartCoroutine(ChangeMusicCoroutine(music, volume, fadeOutDuration, fadeInDuration));
        }

        IEnumerator ChangeMusicCoroutine(AudioClip music, float volume, float fadeOutDuration, float fadeInDuration)
        {
            // Fade out de la música actual
            yield return musicSource.DOFade(0f, fadeOutDuration).WaitForCompletion();
    
            // Cambiar el clip
            musicSource.Stop();
            musicSource.clip = music;
            musicSource.Play();
    
            // Fade in de la nueva música
            yield return musicSource.DOFade(volume, fadeInDuration).WaitForCompletion();
        }
    }
}
