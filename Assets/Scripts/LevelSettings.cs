using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [SerializeField] private int maxHits;
    [SerializeField] private int totalCoins;
    [SerializeField] private int usedHitsToPass;
    [SerializeField] private TypeCard levelCard;

    [SerializeField] private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.MaxHits = maxHits;
        GameManager.Instance.TotalCoins = totalCoins;
        GameManager.Instance.CurrentCard = levelCard;
        GameManager.Instance.MainCamera = mainCamera;
        GameManager.Instance.UsedHitsToPass = usedHitsToPass;
        GameManager.Instance.InitializeLevel();
    }
}
