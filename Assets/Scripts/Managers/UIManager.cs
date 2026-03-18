using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Managers;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [field: SerializeField] public TMP_Text HitsText { get; set; }
    [field: SerializeField] public TMP_Text CoinText { get; set; }
    [field: SerializeField] public TMP_Text CardText { get; set; }
    [field: SerializeField] public GameObject gameInterface { get; set; }
    [SerializeField] private GameObject summaryPanel;
    [SerializeField] private GameObject textsGroup;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text summaryHitsText;
    [SerializeField] private TMP_Text summaryCoinText;
    [SerializeField] private TMP_Text summaryScoreText;
    [Header("Sounds")]
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioClip textAppearSound;
    
    private GameObject summaryBackground;
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

    private void Start()
    {
        summaryBackground = summaryPanel.transform.Find("DarkBackground").gameObject;
    }

    public void ShowSummary()
    {
        StartCoroutine(SummaryCorutine());
    }
    
    public void ClickNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    private IEnumerator SummaryCorutine()
    {
        yield return Fade(2f, 1);

        yield return ShowTexts(textsGroup.transform, true);
        yield return ShowValueTexts(true);
        
        nextButton.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(textAppearSound, 0.4f);
        yield return nextButton.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
    }

    private IEnumerator ShowTexts(Transform texts, bool state)
    {
        foreach (Transform text in texts)
        {
            text.gameObject.SetActive(state);
            if (state)
            {
                AudioManager.Instance.PlaySfx(textAppearSound, 0.4f);
                yield return text.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
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
                AudioManager.Instance.PlaySfx(textAppearSound, 0.4f);
                yield return item.text.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
                yield return StartCoroutine(ScoreEffects(item.text, item.value, 0.1f));
            }
            else
            {
                item.text.SetText("0");
            }
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
        GameManager.Instance.ResetLevel();
        yield return ShowTexts(textsGroup.transform, false);
        yield return ShowValueTexts(false);
        nextButton.gameObject.SetActive(false);

        yield return Fade(1.4f, 2);
        summaryBackground.SetActive(false);
        gameInterface.SetActive(true);
    }
    
    private IEnumerator ScoreEffects(TMP_Text text, int number, float speed)
    {
        for (int i = 1; i <= number; i++)
        {
            text.SetText($"{i}");
            AudioManager.Instance.PlaySfx(scoreSound, 0.5f, 1f + (i/15f));
            yield return new WaitForSeconds(speed);
        }
    }
}
