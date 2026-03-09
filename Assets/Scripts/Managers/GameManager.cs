using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TypeCard
{
    Collector,
    Efficiency,
    Balance
}

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public int levelScore { get; private set; }
    [field: SerializeField] public int totalScore { get; private set; }
    [field: SerializeField] public int usedHitsOnLevel { get; private set; }
    [field: SerializeField] public int coinsPickedOnLevel { get; private set; }
    
    
    public int MaxHits { get; set; }
    public int CurrentLevel { get; set; }
    public TypeCard CurrentCard { get; set; }
    
    private int hitsLeft;
    public static GameManager Instance { get; private set; }

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
        hitsLeft = MaxHits;
        UIManager.Instance.HitsText.SetText($"Hits: {hitsLeft}");
        UIManager.Instance.CoinText.SetText($"Coins: {coinsPickedOnLevel}");
    }

    public void RegisterHit()
    {
        usedHitsOnLevel++;
        hitsLeft--;
        UIManager.Instance.HitsText.SetText($"Hits: {hitsLeft}");
    }

    public void RegisterCoin()
    {
        coinsPickedOnLevel++;
        UIManager.Instance.CoinText.SetText($"Coins: {coinsPickedOnLevel}");
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
                if (coinsPickedOnLevel == hitsLeft)
                {
                    levelScoreMult = 5;
                }
                break;
        }
        
        levelScore = (coinsPickedOnLevel * coinMult) * (hitsLeft * hitsMult);
        totalScore += levelScore * levelScoreMult;
    }

    private void ResetLevel()
    {
        coinsPickedOnLevel = 0;
        usedHitsOnLevel = 0;
        levelScore = 0;
        SceneManager.LoadScene("Level" + (CurrentLevel + 1));
    }
}
