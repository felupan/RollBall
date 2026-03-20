using System;
using System.Collections;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Canvas
{
    public class LevelCompleted : MonoBehaviour
    {
        [SerializeField] private TMP_Text starsText;
        [SerializeField] private TMP_Text totalStarsText;
        [SerializeField] private TMP_Text starsValueText;
        [SerializeField] private AudioClip textAppearSound;
        private int stars;
        private int totalStars;

        private void Start()
        {
            stars = GameManager.Instance.TotalStarsOnLevel;
            totalStars = GameManager.Instance.TotalStars;
            GameManager.Instance.ResetLevelStars();
            starsText.SetText($"You collected {stars} STARS");
            starsText.gameObject.SetActive(false);
            totalStarsText.gameObject.SetActive(false);
            StartCoroutine(ShowLevelSummary());
        }

        private IEnumerator ShowLevelSummary()
        {
            starsText.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx(textAppearSound, 0.5f);
            yield return starsText.transform.DOPunchPosition(Vector3.one, 1f, 30, 2f).WaitForCompletion();
        
            totalStarsText.gameObject.SetActive(true);
            UIManager.Instance.ScoreEffect(starsValueText, totalStars, 0.2f);
            yield return new WaitForSeconds(4f);
            SceneManager.LoadScene("LevelTemplate");
        }
    }
}
