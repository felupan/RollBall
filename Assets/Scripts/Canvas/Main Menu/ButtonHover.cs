using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Canvas.Main_Menu
{
    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AudioClip hoverEnterSound;
        [SerializeField] private AudioClip hoverExitSound;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySfx(hoverEnterSound);
            transform.DOScale(1.1f,0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //AudioManager.Instance.PlaySfx(hoverExitSound);
            transform.DOScale(1f,0.5f);
        }
    }
}
