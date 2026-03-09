using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinText;
    public TMP_Text CoinText
    {
        get => _coinText;
        set => value = _coinText;
    }
    
    [SerializeField] private TMP_Text _hitsText;
    public TMP_Text HitsText
    {
        get => _hitsText;
        set => value = _hitsText;
    }

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
