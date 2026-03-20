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
        
        private List<string> texts = new List<string>();
        
        private string copyInstructionsText;
        private string text1;
        private string text2;
        private string text3;
        private string text4;
        private string text5;
        private string text6;
        private string text7;
        private string text8;
        private string text9;
        private string text10;

        private void Awake()
        {
            instructionsText.SetText("");
            text1 = "Drag the ball to apply a force to it";
            text2 = "Further you move your mouse, more power";
            text3 = "Score the ball into the basket";
            text4 = "Each LEVEL has 3 scenarios";
            text5 = "Your score is determined multiplying HITS LEFT by COINS";
            text6 = "In each scenario you can get from 3 to 0 STARS";
            text7 = "This are needed to pass the LEVEL";
            text8 = "The higher your score, more STARS";
            text9 = "You get to chose a Potential Card that will BONUS your stats";
            text10 = "Get the specified STARS to pass the LEVEL";
            texts.Add(text1);
            texts.Add(text2);
            texts.Add(text3);
            texts.Add(text4);
            texts.Add(text5);
            texts.Add(text6);
            texts.Add(text7);
            texts.Add(text8);
            texts.Add(text9);
            texts.Add(text10);
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
