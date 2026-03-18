using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [SerializeField] private int maxHits;
    [SerializeField] private int levelNumber;
    [SerializeField] private TypeCard levelCard;

    [SerializeField] private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.MaxHits = maxHits;
        GameManager.Instance.CurrentLevel = levelNumber;
        GameManager.Instance.CurrentCard = levelCard;
        GameManager.Instance.MainCamera = mainCamera;
        GameManager.Instance.InitializeLevel();
    }
}
