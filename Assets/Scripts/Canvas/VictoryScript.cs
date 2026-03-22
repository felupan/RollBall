using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Canvas
{
    public class VictoryScript : MonoBehaviour
    {
        [SerializeField] private AudioClip victorySound;
    
        private IEnumerator Start()
        {
            AudioManager.Instance.PlaySfx(victorySound,0.5f);
            yield return new WaitForSeconds(6f);
            GameManager.Instance.ResetAll();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
