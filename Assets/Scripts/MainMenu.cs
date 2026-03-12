using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image background;
    [SerializeField] private Image titleBorder;
    [SerializeField] private Button[] buttons;
    [SerializeField] private AudioSource typingSound;
    
    private string currentTitleText;
    private string copyTitleText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        copyTitleText = titleText.text;   
        PlayIntroSequence();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayIntroSequence()
    {
        Color c = background.color;
        c.a = 0f;
        background.color = c;
        yield return background.DOFade(0.7f, 2f).WaitForCompletion();
    }

    public void Play()
    {
        SceneManager.LoadScene("Level0");
    }

    public void Quit()
    {
        Application.Quit();
    }

    void PlayTitle()
    {
        StartCoroutine(PlayTitleCoroutine());
    }

    IEnumerator PlayTitleCoroutine()
    {
        List<string> words = new List<string>(copyTitleText.Split(" "));
    
        foreach (var word in words)
        {
            currentTitleText += word + " ";
            titleText.SetText(currentTitleText);
            yield return new WaitForSeconds(0.3f);
        }
    }
    
}
