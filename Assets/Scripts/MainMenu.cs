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
        titleText.SetText("");
        titleBorder.enabled = false;
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        PlayIntroSequence();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayIntroSequence()
    {
        StartCoroutine(PlayIntroCorrutine());
    }
    
    IEnumerator PlayIntroCorrutine()
    {
        Color c = background.color;
        c.a = 1f;
        background.color = c;
        yield return background.DOFade(0.7f, 2f).WaitForCompletion();
        yield return PlayTitleCoroutine();
        titleBorder.enabled = true;
        titleBorder.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
        yield return titleBorder.DOFade(1f, 0.2f).WaitForCompletion();
    }

    public void Play()
    {
        SceneManager.LoadScene("Level0");
    }

    public void Quit()
    {
        Application.Quit();
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
