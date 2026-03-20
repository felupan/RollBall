using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum TypeCard
{
    Collector,
    Efficiency,
    Balance
}
public class GameManager : MonoBehaviour
{
    [field: SerializeField] public int LevelScore { get; private set; }
    [field: SerializeField] public int Stars { get; private set; }
    [field: SerializeField] public int UsedHitsOnLevel { get; private set; }
    [field: SerializeField] public int CoinsPickedOnLevel { get; private set; }
    [field: SerializeField] public int TotalStarsOnLevel { get; private set; }
    [field: SerializeField] public int TotalStars { get; private set; }
    [field: SerializeField] public int RequiredStars { get; private set; }
    [field: SerializeField] public int UsedHitsToPass { get; set; }
    [field: SerializeField] public int TotalCoins { get; set; }
    [field: SerializeField] public int CurrentLevelIndex { get; private set; }
    [SerializeField] private int currentScenarioIndex;
    
    [field: SerializeField] public List<LevelData> Levels { get; private set; }
    
    private AudioClip gameMusic;
    
    public int MaxHits { get; set; }
    public int HitsLeft { get; private set; }
    public TypeCard CurrentCard { get; set; }
    [field: SerializeField] public Camera MainCamera { get; set; }
    
    private Quaternion cameraInitialRotation;
    [SerializeField] private Quaternion cameraSummaryRotation;
    
    [Header("Debug Settings")]
    [SerializeField] private int debugStartLevel = 0;
    [SerializeField] private int debugStartScene = 0;
    [SerializeField] private int debugStartStars = 0;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentLevelIndex = debugStartLevel;
            currentScenarioIndex = debugStartScene;
            TotalStarsOnLevel = debugStartStars;
            TotalStars = debugStartStars;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameMusic = Levels[CurrentLevelIndex].levelMusic; 
        if (!AudioManager.Instance.IsPlayingMusic())
        {
            AudioManager.Instance.ChangeMusic(gameMusic, 0.4f, 0f, 3f);
        }
    }

    public void InitializeLevel()
    {
        HitsLeft = MaxHits;
        cameraInitialRotation = MainCamera.transform.rotation;
        RequiredStars = Levels[CurrentLevelIndex].requiredStars;
        UIManager.Instance.HitsText.SetText($"{HitsLeft}");
        UIManager.Instance.CoinText.SetText($"{CoinsPickedOnLevel}");
        UIManager.Instance.CardText.SetText($"{CurrentCard}");
    }

    public void RegisterHit()
    {
        UsedHitsOnLevel++;
        HitsLeft--;
        UIManager.Instance.HitsText.SetText($"{HitsLeft}");
    }

    public void RegisterCoin()
    {
        CoinsPickedOnLevel++;
        UIManager.Instance.CoinText.SetText($"{CoinsPickedOnLevel}");
    }

    public void CalculateLevelScore()
    {
        int coinMult = 1;
        int hitsMult = 1;
        int levelScoreMult = 1;
        
        switch (CurrentCard)
        {
            case TypeCard.Collector:
                coinMult = 3;
                break;
            case TypeCard.Efficiency:
                hitsMult = 3;
                break;
            case TypeCard.Balance:
                coinMult = 2;
                hitsMult = 2;
                if (CoinsPickedOnLevel == HitsLeft)
                {
                    levelScoreMult = 5;
                }
                break;
        }
        
        // Calculate Stars
        int maxScore = (TotalCoins * coinMult) * ((MaxHits - UsedHitsToPass) * hitsMult) * levelScoreMult;
        LevelScore = (CoinsPickedOnLevel * coinMult) * (HitsLeft * hitsMult) * levelScoreMult;
        Debug.Log($"MaxScore: {maxScore}");
        if (LevelScore >= maxScore * 0.8f) Stars = 3;
        else if (LevelScore >= maxScore * 0.5f) Stars = 2;
        else if (LevelScore >= maxScore * 0.3f) Stars = 1;
        else Stars = 0;
        TotalStarsOnLevel += Stars;
        TotalStars += Stars;
        
        Summary();
    }

    private void ResetStats()
    {
        CoinsPickedOnLevel = 0;
        UsedHitsOnLevel = 0;
        LevelScore = 0;
        Stars = 0;
    }

    public void ResetLevelStars()
    {
        TotalStarsOnLevel = 0;
    }
    
    public void ResetLevel()
    {
        ResetStats();
        currentScenarioIndex++;
        if (currentScenarioIndex >= Levels[CurrentLevelIndex].scenes.Length)
        {
            currentScenarioIndex = 0;
            CurrentLevelIndex++;
            // Check stars. Change level or Lose
            if (TotalStarsOnLevel >= RequiredStars)
            {
                SceneManager.LoadScene("LevelPassed");
            }
            else Debug.Log("You lost!");
        }
        else
        {
            SceneManager.LoadScene(Levels[CurrentLevelIndex].scenes[currentScenarioIndex]);
            EndSummary();
        }
        
    }

    private void Summary()
    {
        UIManager.Instance.gameInterface.SetActive(false);
        UIManager.Instance.ShowSummary();
        StartCoroutine(RotateCamera(1.4f, cameraInitialRotation, cameraSummaryRotation));
    }

    private IEnumerator RotateCamera(float duration, Quaternion initialRotation, Quaternion endRotation)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float tSmooth = Mathf.SmoothStep(0f, 1f, t); 
            MainCamera.transform.rotation = Quaternion.Slerp(initialRotation, endRotation, tSmooth);
            yield return null;
        }
        MainCamera.transform.rotation = endRotation;
    }
    
    private void EndSummary()
    {
        StartCoroutine(RotateCamera(1.4f, cameraSummaryRotation, cameraInitialRotation));
    }
}
