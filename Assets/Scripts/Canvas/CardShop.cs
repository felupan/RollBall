using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardShop : MonoBehaviour
{
    [SerializeField] private CardUI[] cards;
    [SerializeField] private TMP_Text starsText;
    [SerializeField] private AudioClip shopMusic;

    private void Start()
    {
        AudioManager.Instance.ChangeMusic(shopMusic,0.3f,0,2f);
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Initialize(GameManager.Instance.Cards[i], this);
        }
        starsText.SetText($"{GameManager.Instance.TotalStars}");
    }

    public void UpdateStars()
    {
        starsText.SetText($"{GameManager.Instance.TotalStars}");
    }
    
    public void SkipButton()
    {
        AudioManager.Instance.ChangeMusic(null, 1f,0,0);
        SceneManager.LoadScene("LevelTemplate");
    }
    
}
