using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CST
{
    public class CardInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Card cardData;
        public bool isFromHandView;
        public Image cardImage;

        public Animation animator;


        public void Activate()
        {
            this.cardImage.sprite = cardData.cardFaceImage;
        }

        public void OnChoose()
        {
            GameManager.Instance.ChooseCard(cardData);
        }

        void IPointerEnterHandler.OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (isFromHandView) return;
            animator.clip = animator.GetClip("CardSelectedIn");
            animator.Play();
        }

        void IPointerExitHandler.OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (isFromHandView) return;
            animator.clip = animator.GetClip("CardSelectedOut");
            animator.Play();
        }
    }
}