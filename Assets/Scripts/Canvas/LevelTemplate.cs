using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Canvas
{
    public class LevelTemplate : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text objectiveText;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private AudioClip levelAppearSound;
        private LevelData currentLevel;
        private int indexLevel;

        private void Start()
        {
            indexLevel = GameManager.Instance.CurrentLevelIndex;
            currentLevel = GameManager.Instance.Levels[indexLevel];
            levelText.SetText(currentLevel.levelName);
            objectiveText.SetText($"Objective: GET {currentLevel.requiredStars} STARS");
            AudioManager.Instance.PlaySfx(levelAppearSound, 0.5f);
            StartCoroutine(ShowLevel());
        }

        private IEnumerator ShowLevel()
        {
            yield return new WaitForSeconds(4f);
            SceneManager.LoadScene(currentLevel.scenes[0]);
            if (!AudioManager.Instance.IsPlayingMusic())
            {
                AudioManager.Instance.ChangeMusic(currentLevel.levelMusic, 0.4f, 0);
            }
        }
    }
}
