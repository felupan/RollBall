using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Canvas
{
    public class LevelLost : MonoBehaviour
    {
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text totalStarsText;
        [SerializeField] private TMP_Text totalStars;
        [SerializeField] private TMP_Text totalLevelText;
        [SerializeField] private TMP_Text totalLevel;

        [SerializeField] private AudioClip gameOverEffect;
        
        private void Start()
        {
            gameOverText.gameObject.SetActive(false);
            Color c = gameOverText.color;
            c.a = 0f;
            gameOverText.color = c;
            infoText.gameObject.SetActive(false);
            totalStarsText.gameObject.SetActive(false);
            totalLevelText.gameObject.SetActive(false);
            totalStars.SetText($"{GameManager.Instance.TotalStars}");
            totalLevel.SetText($"{GameManager.Instance.CurrentLevelIndex}");
            ShowTexts();
        }

        private void ShowTexts()
        {
            StartCoroutine(TextAnimations());
        }

        private IEnumerator TextAnimations()
        {
            gameOverText.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx(gameOverEffect, 0.5f);
            yield return gameOverText.DOFade(1f, 5f).WaitForCompletion();
            infoText.gameObject.SetActive(true);
            yield return infoText.transform.DOShakePosition(2f, 2f, 20).WaitForCompletion();
            totalStarsText.gameObject.SetActive(true);
            totalLevelText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            GameManager.Instance.ResetAll();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
