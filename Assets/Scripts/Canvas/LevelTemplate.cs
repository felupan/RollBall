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
        private LevelData currentLevel;
        private int indexLevel;
        private int stars;

        private void Start()
        {
            currentLevel = GameManager.Instance.Levels[indexLevel];
            levelText.SetText(currentLevel.levelName);
            objectiveText.SetText($"Objective: GET {currentLevel.requiredStars} STARS");
            StartCoroutine(ShowLevel());
        }

        private IEnumerator ShowLevel()
        {
            yield return new WaitForSeconds(4f);
            AudioManager.Instance.ChangeMusic(gameMusic, 0.4f, 0f, 3f);
            SceneManager.LoadScene(currentLevel.scenes[0]);
        }
    }
}
