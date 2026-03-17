using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Canvas.Main_Menu
{
    public class HowToPlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text instructionsText;
        [SerializeField] private AudioClip typingSound;
        
        private List<string> texts = new List<string>();
        
        private string copyInstructionsText;
        private string text1;
        private string text2;
        private string text3;

        private void Awake()
        {
            instructionsText.SetText("");
            text1 = "Score the ball into the basket to pass the level";
            text2 = "Each Level has 3 scenarios";
            text3 = "Complete the specified objective to win";
            texts.Add(text1);
            texts.Add(text2);
            texts.Add(text3);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void StartHowToPlay()
        {
            gameObject.SetActive(true);
            StartCoroutine(HowToPlayCoroutine());
        }

        IEnumerator HowToPlayCoroutine()
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
                // Clear instructionsText
                instructionsText.SetText("");
            }
        }
    }
}
