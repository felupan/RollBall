using System;
using System.Collections;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [field: SerializeField] public TMP_Text HitsText { get; set; }
    [field: SerializeField] public TMP_Text CoinText { get; set; }
    [field: SerializeField] public TMP_Text CardText { get; set; }
    [field: SerializeField] public GameObject gameInterface { get; set; }
    [SerializeField] private GameObject summaryPanel;

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

    private IEnumerator SummaryCorutine()
    {
        GameObject background = summaryPanel.transform.Find("DarkBackground").gameObject;
        background.SetActive(true);
        yield return background.GetComponent<Image>().DOFade(0.7f, 2f).WaitForCompletion();
        
        foreach (Transform child in summaryPanel.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
