using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Canvas.Main_Menu
{
    public class HowToPlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text instructionsText;
        [SerializeField] private TMP_Text howToPlayText;
        [SerializeField] private Image fadeOut;
        [SerializeField] private AudioClip typingSound;
        
        private List<string> texts = new List<string>
        {
            "Drag the ball to apply a force to it",
            "Further you move your mouse, more power",
            "Score the ball into the basket",
            "Each LEVEL has 3 scenarios",
            "Your score is determined multiplying HITS LEFT by COINS",
            "In each scenario you can get from 3 to 0 STARS",
            "This are needed to pass the LEVEL",
            "The higher your score, more STARS",
            "Every LEVEL has it Potencial Card",
            "They will grant you a bonus score",
            "Use the STARS to upgrade your Potencial Cards",
            "Get the specified STARS to pass the LEVEL",
        };
        
        private string copyInstructionsText;

        private void Awake()
        {
            instructionsText.SetText("");
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void StartHowToPlay()
        {
            gameObject.SetActive(true);
            StartCoroutine(HowToPlayCoroutine(2f));
        }
        public void SkipTutorial()
        {
            StopCoroutine(HowToPlayCoroutine(2f));
            StartCoroutine(FadeOutCoroutine(2f));
            SceneManager.LoadScene("LevelTemplate");
        }

        IEnumerator HowToPlayCoroutine(float duration)
        {
            foreach (string text in texts)
            {
                copyInstructionsText = text;
                List<string> words = new List<string>(copyInstructionsText.Split(" "));

                foreach (string word in words)
                {
                    instructionsText.SetText(instructionsText.text + " " + word);
                    instructionsText.transform.DOShakePosition(0.3f, 2f);
                    AudioManager.Instance.PlaySfx(typingSound, 0.4f);
                    yield return new WaitForSeconds(0.3f);
                }

                yield return new WaitForSeconds(2.5f);
                // Last text on the list, fade out
                if (texts.IndexOf(text) == texts.Count - 1)
                {
                    yield return FadeOutCoroutine(duration);
                    SceneManager.LoadScene("LevelTemplate");
                } // Clear instructionsText
                else instructionsText.SetText("");
            }
        }

        private IEnumerator FadeOutCoroutine(float duration)
        {
            fadeOut.gameObject.SetActive(true);
            yield return fadeOut.DOFade(1f, duration).WaitForCompletion();
        }
    }
}
