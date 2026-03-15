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
        
        public void PlaySfx(AudioClip clipToPlay, float volume = 1f)
        {
            sfxSource.volume = volume;
            sfxSource.PlayOneShot(clipToPlay);
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
