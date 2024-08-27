using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInventory.Inventory
{
    public class SlotController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color unhoverColor;

        private Slot slot;
        private bool isHovering;

        public void Initialize(Slot slot)
        {
            this.slot = slot;
            slot.ItemChangedEvent += OnItemChanged;
            slot.AmountChangedEvent += OnAmountChanged;
            slot.DisposeEvent += OnDispose;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Context menu open");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetHoverState(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetHoverState(false);
        }

        private void OnDispose()
        {
            slot.ItemChangedEvent -= OnItemChanged;
            slot.AmountChangedEvent -= OnAmountChanged;
            slot.DisposeEvent -= OnDispose;
        }

        private void Clear()
        {
            iconImage.color = new(Color.white.r, Color.white.g, Color.white.b, 0);
            iconImage.sprite = null;
            amountText.text = "";
        }

        private void OnAmountChanged(int newAmount)
        {
            SetAmount(newAmount);
        }

        private void OnItemChanged(IItem item, int itemAmount)
        {
            if (item == null)
            {
                Clear();
            }
            else
            {
                SetItem(item, itemAmount);
            }
        }

        private void SetItem(IItem item, int amount)
        {
            iconImage.sprite = item.Icon;
            iconImage.color = Color.white;
            amountText.text = amount.ToString();
        }

        private void SetAmount(int newAmount)
        {
            amountText.text = newAmount.ToString();
        }

        private void SetHoverState(bool isHovering)
        {
            if (this.isHovering != isHovering)
            {
                this.isHovering = isHovering;
                slotImage.color = isHovering ? hoverColor : unhoverColor;
            }
        }
    }
}
