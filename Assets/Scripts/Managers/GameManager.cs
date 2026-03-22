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
    [field: SerializeField] public List<CardData> Cards { get; private set; }
    
    private AudioClip gameMusic;
    
    public int MaxHits { get; set; }
    public int MaxScore { get; private set; }
    public int HitsLeft { get; private set; }
    public TypeCard CurrentCard { get; set; }
    public CardData ActiveCard { get; private set; }
    
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
        if (!AudioManager.Instance.IsPlayingMusic() && SceneManager.GetActiveScene() == SceneManager.GetSceneByName(Levels[CurrentLevelIndex].scenes[currentScenarioIndex]))
        {
            AudioManager.Instance.ChangeMusic(gameMusic, 0.4f, 0f, 3f);
        }
    }

    public void InitializeLevel()
    {
        HitsLeft = MaxHits;
        RequiredStars = Levels[CurrentLevelIndex].requiredStars;
        ActiveCard = Cards.Find(card => card.cardType == CurrentCard);
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

    public void SpendStars(int amount)
    {
        TotalStars -= amount;
    }

    public void CalculateLevelScore()
    {
        int coinMult = ActiveCard.coinMultPerLevel[ActiveCard.upgradeLevel];
        int hitsMult = ActiveCard.hitsMultPerLevel[ActiveCard.upgradeLevel];
        int scoreMult = ActiveCard.scoreMultPerLevel[ActiveCard.upgradeLevel];

        if (ActiveCard.cardType == TypeCard.Balance && CoinsPickedOnLevel != HitsLeft)
        {
            scoreMult = 1;
        }
        
        // Calculate Stars
        MaxScore = (TotalCoins * coinMult) * ((MaxHits - UsedHitsToPass) * hitsMult) * scoreMult;
        LevelScore = (CoinsPickedOnLevel * coinMult) * (HitsLeft * hitsMult) * scoreMult;
        Debug.Log($"MaxScore: {MaxScore}");
        if (LevelScore >= MaxScore * 0.8f) Stars = 3;
        else if (LevelScore >= MaxScore * 0.5f) Stars = 2;
        else if (LevelScore >= MaxScore * 0.3f) Stars = 1;
        else Stars = 0;
        TotalStarsOnLevel += Stars;
        TotalStars += Stars;
        
        Summary();
    }

    private void ResetSceneStats()
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

    public void ResetAll()
    {
        ResetSceneStats();
        ResetLevelStars();
        TotalStars = 0;
        CurrentLevelIndex = 0;
        currentScenarioIndex = 0;
        foreach (CardData card in Cards)
        {
            card.upgradeLevel = 0;
        }
    }
    
    public void ResetLevel()
    {
        ResetSceneStats();
        currentScenarioIndex++;
        if (currentScenarioIndex >= Levels[CurrentLevelIndex].scenes.Length)
        {
            currentScenarioIndex = 0;
            CurrentLevelIndex++;
            // Check stars. Change level or Lose
            if (TotalStarsOnLevel >= RequiredStars)
            {
                AudioManager.Instance.ChangeMusic(null,1f,0,0);
                if (CurrentLevelIndex == Levels.Count)
                {
                    SceneManager.LoadScene("WinScene");
                }
                else SceneManager.LoadScene("LevelPassed");
            }
            else
            {
                AudioManager.Instance.ChangeMusic(null,1f,0,0);
                SceneManager.LoadScene("LostScene");
            }
        }
        else
        {
            SceneManager.LoadScene(Levels[CurrentLevelIndex].scenes[currentScenarioIndex]);
        }
        
    }

    private void Summary()
    {
        UIManager.Instance.ShowSummary();
    }
}
