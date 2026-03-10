using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    [field: SerializeField] public TMP_Text HitsText { get; set; }
    [field: SerializeField] public TMP_Text CoinText { get; set; }
    [field: SerializeField] public TMP_Text CardText { get; set; }
    [field: SerializeField] public TMP_Text ScoreText { get; set; }

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
}
