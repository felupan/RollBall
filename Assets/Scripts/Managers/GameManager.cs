using System;
using System.Collections;
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
    [field: SerializeField] public int TotalScore { get; private set; }
    [field: SerializeField] public int UsedHitsOnLevel { get; private set; }
    [field: SerializeField] public int CoinsPickedOnLevel { get; private set; }

    [SerializeField] private AudioClip gameMusic;
    
    public int MaxHits { get; set; }
    public int HitsLeft { get; private set; }
    public int CurrentLevel { get; set; }
    public TypeCard CurrentCard { get; set; }
    [field: SerializeField] public Camera MainCamera { get; set; }
    
    private Quaternion cameraInitialRotation;
    [SerializeField] private Quaternion cameraSummaryRotation;
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
        AudioManager.Instance.ChangeMusic(gameMusic, 0.4f, 0f, 2f);
    }

    public void InitializeLevel()
    {
        HitsLeft = MaxHits;
        cameraInitialRotation = MainCamera.transform.rotation;
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
        
        LevelScore = (CoinsPickedOnLevel * coinMult) * (HitsLeft * hitsMult) * levelScoreMult;
        TotalScore += LevelScore;
        Summary();
    }

    public void ResetLevel()
    {
        // If the Gm doesn't destroy, everytime we change levels we have to assign the level camera
        CoinsPickedOnLevel = 0;
        UsedHitsOnLevel = 0;
        LevelScore = 0;
        SceneManager.LoadScene("Level" + (CurrentLevel + 1));
        EndSummary();
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
