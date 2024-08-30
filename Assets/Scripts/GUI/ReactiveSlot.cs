using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInventory.GUI
{
    public class ReactiveSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Color hoverColor;
        [SerializeField] protected Color unhoverColor;
        [SerializeField] protected Image slotImage;

        protected bool isHovering;

        public virtual void OnPointerClick(PointerEventData eventData) { }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            SetHoverState(true);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            SetHoverState(false);
        }

        protected virtual void SetHoverState(bool isHovering)
        {
            if (this.isHovering != isHovering)
            {
                this.isHovering = isHovering;
                slotImage.color = isHovering ? hoverColor : unhoverColor;
            }
        }
    }
}
