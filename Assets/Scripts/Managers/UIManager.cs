using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Managers;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [field: SerializeField] public TMP_Text HitsText { get; set; }
    [field: SerializeField] public TMP_Text CoinText { get; set; }
    [field: SerializeField] public TMP_Text CardText { get; set; }
    [field: SerializeField] public GameObject gameInterface { get; set; }
    [SerializeField] private GameObject summaryPanel;
    [SerializeField] private GameObject summaryBackground;
    [SerializeField] private GameObject textsGroup;
    [SerializeField] private RawImage[] starsGroup;
    [SerializeField] private TMP_Text starCounterText;
    [SerializeField] private TMP_Text starText;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text summaryHitsText;
    [SerializeField] private TMP_Text summaryCoinText;
    [SerializeField] private TMP_Text summaryScoreText;
    [Header("Sounds")]
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioClip textAppearSound;
    [SerializeField] private AudioClip starSound;
    [SerializeField] private AudioClip noStarSound;
    
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void ShowSummary()
    {
        StartCoroutine(SummaryCorutine());
    }
    
    public void ClickNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    public void ScoreEffect(TMP_Text text, int number, float speed)
    {
        StartCoroutine(ScoreEffects(text, number, speed));
    }

    private IEnumerator SummaryCorutine()
    {
        yield return Fade(2f, 1);

        yield return ShowTexts(textsGroup.transform, true);
        yield return ShowValueTexts(true);
        starText.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(textAppearSound, 0.5f);
        yield return starText.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
        yield return ShowStars(true);
        
        starCounterText.gameObject.SetActive(true);
        starCounterText.SetText($"{GameManager.Instance.TotalStarsOnLevel}/{GameManager.Instance.RequiredStars}");
        yield return starCounterText.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
        AudioManager.Instance.PlaySfx(textAppearSound, 0.5f);
        
        nextButton.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(textAppearSound, 0.5f);
        yield return nextButton.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
    }

    private IEnumerator ShowTexts(Transform texts, bool state)
    {
        foreach (Transform text in texts)
        {
            text.gameObject.SetActive(state);
            if (state)
            {
                AudioManager.Instance.PlaySfx(textAppearSound, 0.5f);
                yield return text.DOPunchPosition(Vector3.up, 0.5f, 30, 3f).WaitForCompletion();
            }
        }
    }

    private IEnumerator ShowValueTexts(bool state)
    {
        var summaryItems = new List<(TMP_Text text, int value)>
        {
            (summaryCoinText, GameManager.Instance.CoinsPickedOnLevel),
            (summaryHitsText, GameManager.Instance.HitsLeft),
            (summaryScoreText, GameManager.Instance.LevelScore)
        };

        foreach (var item in summaryItems)
        {
            item.text.gameObject.SetActive(state);
            if (state)
            {
                AudioManager.Instance.PlaySfx(textAppearSound, 0.5f);
                yield return item.text.transform.DOPunchPosition(Vector3.up, 0.5f, 30, 3f).WaitForCompletion();
                yield return FinalScoreEffect(item.text, item.value, 2f, 0.1f, 80);
            }
            else
            {
                item.text.SetText("0");
            }
        }
    }

    private IEnumerator ShowStars(bool state)
    {
        if (!state)
        {
            foreach (var star in starsGroup)
            {
                star.gameObject.SetActive(false);
            }
            yield break;
        }
        Debug.Log($"STARS: {GameManager.Instance.Stars}");
        for(int i = 0; i < starsGroup.Length; i++)
        {
            starsGroup[i].gameObject.SetActive(true);
            if (i < GameManager.Instance.Stars)
            {
                starsGroup[i].color = Color.white;
                AudioManager.Instance.PlaySfx(starSound, 0.5f);
            }
            else
            {
                starsGroup[i].color = Color.black;
                AudioManager.Instance.PlaySfx(noStarSound, 0.5f);
            }
            yield return starsGroup[i].transform.DOPunchScale(Vector3.one * 0.3f, 0.8f, 20, 3f).WaitForCompletion();
        }
    }
    
    private IEnumerator Fade(float duration, int fadeType)
    {
        summaryBackground.SetActive(true);
        
        switch (fadeType)
        {
            // Fade In
            case 1:
                yield return summaryBackground.GetComponent<Image>().DOFade(0.5f, duration).WaitForCompletion();
                break;
            // Fade Out
            case 2:
                yield return summaryBackground.GetComponent<Image>().DOFade(0f, duration).WaitForCompletion();
                break;
        }
    }

    private IEnumerator NextLevel()
    {
        // Hide UI and CLEAR text values data
        yield return HideSummary();
        GameManager.Instance.ResetLevel();

        yield return Fade(1.3f, 2);
        summaryBackground.SetActive(false);
        gameInterface.SetActive(true);
    }

    private IEnumerator HideSummary()
    {
        yield return ShowValueTexts(false);
        yield return ShowTexts(textsGroup.transform, false);
        starText.gameObject.SetActive(false);
        starCounterText.gameObject.SetActive(false);
        yield return ShowStars(false);
        nextButton.gameObject.SetActive(false);
    }
    
    private IEnumerator ScoreEffects(TMP_Text text, int number, float speed)
    {
        float startTime = Time.time;
        
        for (int i = 1; i <= number; i++)
        {
            text.SetText($"{i}");
            
            // 1. Depends on time elapsed
            float elapsed = Time.time - startTime; 
            float currentSpeed = Mathf.Max(speed * (1f - elapsed * 0.1f), 0.05f);
            float pitch = Mathf.Min(1f + elapsed * 0.1f, 2f);
            if (currentSpeed <= 0.05f)
            {
                text.SetText($"{number}");
                text.transform.DOPunchPosition(Vector3.up, 1f, 40, 3f);
                yield break;
            }
            
            // 2. Depends on how big the number is
            // float progress = (float)i / number;
            // float currentSpeed = speed * (1f - progress * 0.4f);
            // float pitch = 1f + progress * 0.5f;
           
            AudioManager.Instance.PlaySfx(scoreSound, 0.5f, pitch);
            yield return new WaitForSeconds(currentSpeed);
        }
    }

    private IEnumerator ScoreEffects2(TMP_Text text, int number, float duration, float pitch, int startNumber = 0)
    {
        float elapsed = 0;
        float counter = 0;
    
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            int currentNumber = Mathf.RoundToInt(Mathf.Lerp(startNumber, number, progress));
            text.SetText($"{currentNumber}");
            pitch = Mathf.Min(pitch + progress * 0.05f, 1.5f);
            if (counter % 5 == 0)
            {
                AudioManager.Instance.PlaySfx(scoreSound,0.4f, pitch);
            }
            
            counter++;
            yield return null;
        }
    
        text.SetText($"{number}");
        AudioManager.Instance.PlaySfx(scoreSound,0.4f);
        yield return text.transform.DOPunchPosition(Vector3.up, 1f, 40, 3f).WaitForCompletion();
    }

    private IEnumerator FinalScoreEffect(TMP_Text text, int number, float duration, float speed, int limit)
    {
        float decimalPitch = 0;
        float pitch = 1f;
        for (int i = 0; i < number; i++)
        {
            if (i < limit)
            {
                float progress = (float)i / number;
                float localProgress = (float)i / limit;
                float currentSpeed = Mathf.Lerp(speed, Time.deltaTime, localProgress);
                text.SetText($"{i}");
                decimalPitch = localProgress * 0.15f;
                pitch = 1f + decimalPitch;
                
                AudioManager.Instance.PlaySfx(scoreSound,0.4f, pitch);
                yield return new WaitForSeconds(currentSpeed);
            }
            else
            {
                yield return ScoreEffects2(text, number, duration, pitch, i);
                yield break;
            }
        }
    }
}
