using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
    
        [field: SerializeField] public TMP_Text HitsText { get; set; }
        [field: SerializeField] public TMP_Text CoinText { get; set; }
        [field: SerializeField] public TMP_Text CardText { get; set; }
        [SerializeField] private GameObject gameInterface;
        [SerializeField] private GameObject summaryPanel;
        [Header(("Summary Objects"))]
        [SerializeField] private GameObject summaryBackground;
        [SerializeField] private GameObject textsGroup;
        [SerializeField] private RawImage[] starsGroup;
        [SerializeField] private TMP_Text starCounterText;
        [SerializeField] private TMP_Text starText;
        [SerializeField] private Button nextButton;
        [SerializeField] private TMP_Text summaryHitsText;
        [SerializeField] private TMP_Text summaryCoinText;
        [SerializeField] private TMP_Text summaryScoreText;
        [Header("Sounds")]
        [SerializeField] private AudioClip scoreSound;
        [SerializeField] private AudioClip textAppearSound;
        [SerializeField] private AudioClip starSound;
        [SerializeField] private AudioClip noStarSound;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip constantFireSound;
    
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private Quaternion cameraSummaryRotation;

        private Camera mainCamera;
        private Tween shakeTween;
        private UnityEngine.Canvas canvas;
        private Quaternion cameraInitialRotation;
    
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            canvas = GetComponent<UnityEngine.Canvas>();
        }

        public void SetCamera(Camera cam)
        {
            mainCamera = cam;
            cameraInitialRotation = mainCamera.transform.rotation;
        }
    
        public void ShowSummary()
        {
            canvas.worldCamera = mainCamera;
            StartCoroutine(SummaryCorutine());
        }
    
        public void ClickNextLevel()
        {
            StartCoroutine(NextLevel());
        }

        private IEnumerator SummaryCorutine()
        {
            gameInterface.SetActive(false);
            summaryPanel.SetActive(true);
            StartCoroutine(RotateCamera(1.4f, cameraInitialRotation, cameraSummaryRotation)); 
            yield return Fade(2f, 1);

            yield return ShowTexts(textsGroup.transform, true);
            yield return ShowValueTexts(true);
            starText.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx(textAppearSound, 0.3f);
            yield return starText.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
            yield return ShowStars(true);
        
            starCounterText.gameObject.SetActive(true);
            starCounterText.SetText($"{GameManager.Instance.TotalStarsOnLevel}/{GameManager.Instance.RequiredStars}");
            AudioManager.Instance.PlaySfx(textAppearSound, 0.3f);
            yield return starCounterText.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
        
            nextButton.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx(textAppearSound, 0.3f);
            yield return nextButton.transform.DOPunchPosition(Vector3.up, 0.5f, 20, 3f).WaitForCompletion();
        }
        
        private IEnumerator RotateCamera(float duration, Quaternion initialRotation, Quaternion endRotation)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float tSmooth = Mathf.SmoothStep(0f, 1f, t); 
                mainCamera.transform.rotation = Quaternion.Slerp(initialRotation, endRotation, tSmooth);
                yield return null;
            }
            mainCamera.transform.rotation = endRotation;
        }
        
        private IEnumerator NextLevel()
        {
            // Hide UI and CLEAR text values data
            yield return HideSummary();
            GameManager.Instance.ResetLevel();
            StartCoroutine(RotateCamera(1.4f, cameraSummaryRotation, cameraInitialRotation)); 
            yield return Fade(1.3f, 2);
            summaryBackground.SetActive(false);
            gameInterface.SetActive(true);
        }

        private IEnumerator HideSummary()
        {
            // We first deactivate the entire panel so everything 
            // disappears at the same time.
            summaryPanel.SetActive(false);
            StartCoroutine(ShowValueTexts(false));
            AudioManager.Instance.StopSfxLoop();
            fireEffect.Stop();
            StartCoroutine(ShowTexts(textsGroup.transform, false)); 
            starText.gameObject.SetActive(false);
            starCounterText.gameObject.SetActive(false);
            StartCoroutine(ShowStars(false)); 
            nextButton.gameObject.SetActive(false);
            yield break;
        }
        
        #region Texts&Stars
        private IEnumerator ShowTexts(Transform texts, bool state)
        {
            foreach (Transform text in texts)
            {
                text.gameObject.SetActive(state);
                if (state)
                {
                    AudioManager.Instance.PlaySfx(textAppearSound, 0.3f);
                    yield return text.DOPunchPosition(Vector3.up, 0.5f, 30, 3f).WaitForCompletion();
                }
            }
        }

        private IEnumerator ShowValueTexts(bool state)
        {
            CardData cardData = GameManager.Instance.ActiveCard;
            int upgradeLevel = cardData.upgradeLevel;
            
            var summaryItems = new List<(TMP_Text text, int value, int mult)>
            {
                (summaryCoinText, GameManager.Instance.CoinsPickedOnLevel, cardData.coinMultPerLevel[upgradeLevel]),
                (summaryHitsText, GameManager.Instance.HitsLeft, cardData.hitsMultPerLevel[upgradeLevel]),
                (summaryScoreText, GameManager.Instance.LevelScore, cardData.scoreMultPerLevel[upgradeLevel])
            };

            foreach (var item in summaryItems)
            {
                item.text.gameObject.SetActive(state);
                if (state)
                {
                    AudioManager.Instance.PlaySfx(textAppearSound, 0.3f);
                    yield return item.text.transform.DOPunchPosition(Vector3.up, 0.5f, 30, 3f).WaitForCompletion();
                    if (item == summaryItems[summaryItems.Count - 1])
                    {
                        float limit = GameManager.Instance.MaxScore * 0.5f;
                        if (limit >= 60) limit = 60f;
                        yield return FinalScoreEffect(item.text, item.value, 0.1f, limit);
                    }
                    else
                    {
                        yield return ScoreEffects(item.text, item.value, 0.1f);
                        yield return ShowMult(item.text, item.value, item.mult);
                    }
                }
                else
                {
                    shakeTween?.Kill();
                    item.text.SetText("0");
                    item.text.color = Color.white;
                }
            }
        }

        private IEnumerator ShowMult(TMP_Text text, int value, int mult)
        {
            if (mult == 1) yield break;
            yield return new WaitForSeconds(0.5f);
            text.SetText($"{text.text} x");
            AudioManager.Instance.PlaySfx(textAppearSound, 0.4f, 1.05f);
            yield return new WaitForSeconds(0.5f);
            text.SetText($"{text.text} {mult}");
            AudioManager.Instance.PlaySfx(textAppearSound, 0.4f, 1.1f);
            //SONIDO Y TWEEN
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.PlaySfx(textAppearSound, 0.4f, 0.5f);
            yield return text.transform.DOPunchScale(-Vector3.one *0.1f, 1f, 5, 1f).WaitForCompletion();
            //COMPACTAR Y RESULTADO FINAL (value * mult)
            text.SetText($"{value * mult}");
            AudioManager.Instance.PlaySfx(textAppearSound, 0.4f, 1.2f);
            yield return text.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 5, 1f).WaitForCompletion();
        }
        
        private IEnumerator ShowStars(bool state)
        {
            if (!state)
            {
                foreach (var star in starsGroup)
                {
                    star.gameObject.SetActive(false);
                }
                yield break;
            }
            Debug.Log($"STARS: {GameManager.Instance.Stars}");
            for(int i = 0; i < starsGroup.Length; i++)
            {
                starsGroup[i].gameObject.SetActive(true);
                if (i < GameManager.Instance.Stars)
                {
                    starsGroup[i].color = Color.white;
                    AudioManager.Instance.PlaySfx(starSound, 0.5f);
                }
                else
                {
                    starsGroup[i].color = Color.black;
                    AudioManager.Instance.PlaySfx(noStarSound, 0.5f);
                }
                yield return starsGroup[i].transform.DOPunchScale(Vector3.one * 0.3f, 0.8f, 20, 3f).WaitForCompletion();
            }
        }
        #endregion
    
        #region ScoreEffects
        private IEnumerator ScoreEffects(TMP_Text text, int number, float speed)
        {
            float startTime = Time.time;
        
            for (int i = 1; i <= number; i++)
            {
                text.SetText($"{i}");
                
                float elapsed = Time.time - startTime; 
                float currentSpeed = Mathf.Max(speed * (1f - elapsed * 0.1f), 0.05f);
                float pitch = Mathf.Min(1f + elapsed * 0.1f, 2f);
                if (currentSpeed <= 0.05f)
                {
                    text.SetText($"{number}");
                    text.transform.DOPunchPosition(Vector3.up, 1f, 40, 3f);
                    yield break;
                }
           
                AudioManager.Instance.PlaySfx(scoreSound, 0.5f, pitch);
                yield return new WaitForSeconds(currentSpeed);
            }
        }

        private IEnumerator ScoreEffects2(TMP_Text text, int number, float duration, float startPitch, int startNumber = 0)
        {
            float elapsed = 0;
            float counter = 0;
    
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                int currentNumber = Mathf.RoundToInt(Mathf.Lerp(startNumber, number, progress));
                text.SetText($"{currentNumber}");
                float pitch = Mathf.Min(startPitch + progress * 0.2f, 1.3f);
                if (counter % 5 == 0)
                {
                    AudioManager.Instance.PlaySfx(scoreSound,0.4f, pitch);
                }
            
                counter++;
                yield return null;
            }
    
            text.SetText($"{number}");
            AudioManager.Instance.PlaySfx(scoreSound,0.4f);
            shakeTween = text.transform.DOShakePosition(2f, 5f, 30).SetLoops(-1);
        
        }

        private IEnumerator FinalScoreEffect(TMP_Text text, int number, float speed, float limit)
        {
            for (int i = 0; i < number; i++)
            {
                if (i < limit)
                {
                    float localProgress = (float)i / limit;
                    float currentSpeed = Mathf.Lerp(speed, Time.deltaTime * 2, localProgress);
                    float curvedProgress = Mathf.Pow(localProgress, 3f);
                    text.color = Color.Lerp(text.color, Color.softRed, curvedProgress);
                    text.SetText($"{i}");
                    float pitch = 1f + localProgress * 0.1f;
                
                    AudioManager.Instance.PlaySfx(scoreSound,0.4f, pitch);
                    yield return new WaitForSeconds(currentSpeed);
                }
                else
                {
                    AudioManager.Instance.PlaySfx(fireSound, 0.5f);
                    fireEffect.Play();
                    float duration = Mathf.Clamp((number - i) * 0.01f, Time.deltaTime, 2f);
                    yield return ScoreEffects2(text, number, duration, AudioManager.Instance.GetSfxPitch(), i);
                    AudioManager.Instance.PlaySfxLoop(constantFireSound, 0.3f);
                    break;
                }
            }
        }
        #endregion
        
        private IEnumerator Fade(float duration, int fadeType)
        {
            summaryBackground.SetActive(true);
        
            switch (fadeType)
            {
                // Fade In
                case 1:
                    yield return summaryBackground.GetComponent<Image>().DOFade(0.5f, duration).WaitForCompletion();
                    break;
                // Fade Out
                case 2:
                    yield return summaryBackground.GetComponent<Image>().DOFade(0f, duration).WaitForCompletion();
                    break;
            }

        }
    }
}
