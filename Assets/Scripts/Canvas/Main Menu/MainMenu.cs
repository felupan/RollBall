using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvas.Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Image background;
        [SerializeField] private Image fadeOut;
        [SerializeField] private Image titleBorder;
        [SerializeField] private Button[] buttons;
    
        [SerializeField] private GameObject howToPlay;
        [SerializeField] private GameObject mainMenu;
    
        [Header("Sounds")] 
        [SerializeField] private AudioClip boomSound;
        [SerializeField] private AudioClip whipSound;
        [SerializeField] private AudioClip buttonAppearSound;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip clickSound;
    
        private string currentTitleText;
        private string copyTitleText;

        void Start()
        {
            copyTitleText = titleText.text;
            titleText.SetText("");
            titleBorder.enabled = false;
            fadeOut.gameObject.SetActive(false);
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            PlayIntroSequence();
        }

        private void PlayIntroSequence()
        {
            StartCoroutine(PlayIntroCorrutine());
        }
    
        IEnumerator PlayIntroCorrutine()
        {
            Color c = background.color;
            c.a = 1f;
            background.color = c;
            yield return background.DOFade(0.7f, 1f);
            yield return PlayTitleCoroutine();
            titleBorder.enabled = true;
            AudioManager.Instance.PlaySfx(whipSound, 0.4f);
            titleBorder.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
            yield return new WaitForSeconds(1f);
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
                AudioManager.Instance.PlaySfx(buttonAppearSound, 0.4f);
                yield return button.transform.DOShakePosition(0.5f).WaitForCompletion();
            }
            //We play the Main Menu music
            AudioManager.Instance.ChangeMusic(menuMusic, 0.3f, 0f, 5f);
        }

        public void Play()
        {
            AudioManager.Instance.PlaySfx(clickSound);
            StartCoroutine(FadeOutCoroutine(2f));
        }

        public void Quit()
        {
            AudioManager.Instance.PlaySfx(clickSound);
            Application.Quit();
        }

        IEnumerator PlayTitleCoroutine()
        {
            List<string> words = new List<string>(copyTitleText.Split(" "));
    
            foreach (var word in words)
            {
                currentTitleText += word + " ";
                titleText.SetText(currentTitleText);
                AudioManager.Instance.PlaySfx(boomSound, 0.4f);
                yield return new WaitForSeconds(0.3f);
            }
        }

        IEnumerator FadeOutCoroutine(float duration)
        {
            fadeOut.gameObject.SetActive(true);
            AudioManager.Instance.ChangeMusic(null,1f,2f,0f);
            yield return fadeOut.DOFade(1f, duration).WaitForCompletion();
            mainMenu.SetActive(false);
            Color c = fadeOut.color;
            c.a = 0f;
            fadeOut.color = c;
            fadeOut.gameObject.SetActive(false);
            howToPlay.GetComponent<HowToPlay>().StartHowToPlay();
        }
    }
}
