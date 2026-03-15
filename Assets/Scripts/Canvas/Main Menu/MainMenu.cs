using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Managers;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image background;
    [SerializeField] private Image titleBorder;
    [SerializeField] private Button[] buttons;
    
    [Header("Sounds")] 
    [SerializeField] private AudioClip boomSound;
    [SerializeField] private AudioClip whipSound;
    [SerializeField] private AudioClip buttonAppearSound;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip clickSound;
    
    private string currentTitleText;
    private string copyTitleText;
    
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

    private void PlayIntroSequence()
    {
        StartCoroutine(PlayIntroCorrutine());
    }
    
    IEnumerator PlayIntroCorrutine()
    {
        Color c = background.color;
        c.a = 1f;
        background.color = c;
        yield return background.DOFade(0.7f, 1f);
        yield return PlayTitleCoroutine();
        titleBorder.enabled = true;
        AudioManager.Instance.PlaySfx(whipSound, 0.4f);
        titleBorder.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
        yield return new WaitForSeconds(1f);
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx(buttonAppearSound, 0.4f);
            yield return button.transform.DOShakePosition(0.5f).WaitForCompletion();
        }
        //We play the Main Menu music
        AudioManager.Instance.ChangeMusic(menuMusic, 0.3f, 1f, 5f);
    }

    public void Play()
    {
        AudioManager.Instance.PlaySfx(clickSound);
        SceneManager.LoadScene("Level0");
    }

    public void Quit()
    {
        AudioManager.Instance.PlaySfx(clickSound);
        Application.Quit();
    }

    IEnumerator PlayTitleCoroutine()
    {
        List<string> words = new List<string>(copyTitleText.Split(" "));
    
        foreach (var word in words)
        {
            currentTitleText += word + " ";
            titleText.SetText(currentTitleText);
            AudioManager.Instance.PlaySfx(boomSound, 0.4f);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
