using SimpleInventory.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInventory.Inventory
{
    public class SlotController : ReactiveSlot
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;

        private Slot slot;
        private int slotIndex;
        private InventoryController inventoryController;

        public void Initialize(int index, Slot slot, InventoryController inventoryController)
        {
            this.slotIndex = index;
            this.slot = slot;
            this.inventoryController = inventoryController;

            slot.ItemChangedEvent += OnItemChanged;
            slot.AmountChangedEvent += OnAmountChanged;
            slot.DisposeEvent += OnDispose;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            inventoryController.PointerClick(slotIndex, eventData);
        }

        private void OnDispose()
        {
            slot.ItemChangedEvent -= OnItemChanged;
            slot.AmountChangedEvent -= OnAmountChanged;
            slot.DisposeEvent -= OnDispose;
        }

        private void Clear()
        {
            iconImage.color = new (Color.white.r, Color.white.g, Color.white.b, 0f);
            iconImage.sprite = null;
            amountText.text = string.Empty;
        }

        private void OnAmountChanged(int newAmount)
        {
            SetAmount(newAmount);
        }

        private void OnItemChanged(IItem item, int itemAmount)
        {
            if (item == inventoryController.EmptyItem)
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
    }
}
