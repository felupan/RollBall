using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    
    private string currentTitleText;
    private string copyTitleText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        copyTitleText = titleText.text;   
        PlayTitle();
    }

    // Update is called once per frame
    void Update()
    {
        
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
