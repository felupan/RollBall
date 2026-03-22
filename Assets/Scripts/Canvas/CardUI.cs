using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text coinMultText;
    [SerializeField] private TMP_Text hitsMultText;
    [SerializeField] private TMP_Text scoreMultText;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text upgradeLevelText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private AudioClip upgradeSound;

    private CardData cardData;
    private CardShop cardShop;

    public void Initialize(CardData data, CardShop shop)
    {
        cardData = data;
        cardShop = shop;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int upgradeLevel = cardData.upgradeLevel;
        int displayLevel = Mathf.Min(upgradeLevel, cardData.coinMultPerLevel.Length - 1);
        
        cardNameText.SetText(cardData.cardName);
        coinMultText.SetText($"COIN: {cardData.coinMultPerLevel[displayLevel]}");
        hitsMultText.SetText($"HITS: {cardData.hitsMultPerLevel[displayLevel]}");
        scoreMultText.SetText($"SCORE: {cardData.scoreMultPerLevel[displayLevel]}");
        
        if (CheckMaxLevel(upgradeLevel)) return;
        upgradeLevelText.SetText($"Lvl. {upgradeLevel}");
        upgradeCostText.SetText($"COST: {cardData.upgradeCostPerLevel[upgradeLevel]}");
    }

    public void OnUpgradeButton()
    {
        int cost = cardData.upgradeCostPerLevel[cardData.upgradeLevel];
        if (GameManager.Instance.TotalStars < cost) return;
        GameManager.Instance.SpendStars(cost);
        AudioManager.Instance.PlaySfx(upgradeSound, 0.5f);
        cardData.upgradeLevel++;
        cardShop.UpdateStars();
        UpdateUI();
    }

    private bool CheckMaxLevel(int upgradeLevel)
    {
        if (upgradeLevel >= cardData.coinMultPerLevel.Length - 1)
        {
            upgradeCostText.SetText("MAX");
            upgradeLevelText.SetText("MAX");
            return true;
        }
        return false;
    }
}
