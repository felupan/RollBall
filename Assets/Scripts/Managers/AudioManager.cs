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
        
        public void PlaySfx(AudioClip clipToPlay)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }

        public void ChangeMusic(AudioClip music)
        {
            StartCoroutine(ChangeMusicCoroutine(music));
        }

        IEnumerator ChangeMusicCoroutine(AudioClip music)
        {
            // Fade out de la música actual
            yield return musicSource.DOFade(0f, 1f).WaitForCompletion();
    
            // Cambiar el clip
            musicSource.Stop();
            musicSource.clip = music;
            musicSource.Play();
    
            // Fade in de la nueva música
            yield return musicSource.DOFade(0.7f, 3f).WaitForCompletion();
        }
    }
}
