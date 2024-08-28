using SimpleInventory.Inputs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace SimpleInventory.Inventory
{
    public class InventoryController : MonoBehaviour, IInitializable, IDisposable
    {
        public event Action<bool> ToggleInventoryEvent;

        public IItem EmptyItem => itemsDatabase.Empty;

        [Inject] private readonly ItemsDatabase itemsDatabase;
        [Inject] private readonly IPlayerInputsProvider playerInputsProvider;

        [Header("Physical")]
        [SerializeField] private Transform inventorySpawnPoint;

        [Header("GUI")]
        [SerializeField] private SlotController slotPrefab;
        [SerializeField] private int slotsAmount = 0;
        [SerializeField] private CanvasGroup inventoryGroup;
        [SerializeField] private GridLayoutGroup inventoryLayoutGroup;

        private readonly List<Slot> slots = new ();
        private bool isInventoryEnabled = true;

        public void TryAddItem(IItem item, int amount)
        {
            if (TryGetItemInInventory(item, out int foundIndex))
            {
                slots[foundIndex].AddAmount(amount);
            }
            else
            {
                if (TryFindFirstEmptySlot(out int firstEmptyIndex))
                {
                    slots[firstEmptyIndex].SetItem(item, amount);
                }
                else
                {
                    Debug.LogError($"Inventory is full and item {item.Name} cannot be added!");
                }
            }
        }

        public void RemoveItemFromSlot(int slotIndex)
        {
            if (!IsSlotEmpty(slotIndex))
            {
                if (slots[slotIndex].Item is IGrabableItem grabable)
                {
                    for (int i = 0; i < slots[slotIndex].ItemsAmount; i++)
                    {
                        SpawnGrabbableItem(grabable);
                    }
                }

                CleanSlot(slotIndex);
            }
        }

        public void Initialize()
        {
            InitializeInternal();
        }

        public void Dispose()
        {
            foreach (var slot in slots)
            {
                slot.Dispose();
            }

            playerInputsProvider.InventoryButtonPressedEvent -= OnInventoryButtonPressed;
        }

        public void PointerClick(int slotIndex, PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                TryDropItem(slotIndex);
            }
        }

        private void SpawnGrabbableItem(IGrabableItem grabbableItem)
        {
            //TODO: Can be replaced with proper pooling system
            var newSpawnedItem = Instantiate(grabbableItem.PhysicalObjectPrefab, inventorySpawnPoint.position, inventorySpawnPoint.rotation);
            newSpawnedItem.name = grabbableItem.Name;
        }

        private void TryDropItem(int slotIndex)
        {
            if (!IsSlotEmpty(slotIndex))
            {
                RemoveItemFromSlot(slotIndex);
            }
        }

        private bool TryGetItemInInventory(IItem item, out int foundIndex)
        {
            foundIndex = -1;

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Item == item)
                {
                    foundIndex = i;
                    return true;
                }
            }

            return false;
        }

        private bool TryFindFirstEmptySlot(out int foundIndex)
        {
            foundIndex = -1;

            for (int i = 0; i < slots.Count; i++)
            {
                if (IsSlotEmpty(i))
                {
                    foundIndex = i;
                    return true;
                }
            }

            return false;
        }

        private void InitializeInternal()
        {
            CreateInventory();
            SetInventoryPanelEnabled(false);

            playerInputsProvider.InventoryButtonPressedEvent += OnInventoryButtonPressed;
        }

        private void OnInventoryButtonPressed()
        {
            SetInventoryPanelEnabled(!isInventoryEnabled);
        }

        private void CreateInventory()
        {
            for (int i = 0; i < slotsAmount; i++)
            {
                var newSlotPrefab = Instantiate(slotPrefab, inventoryLayoutGroup.transform);
                newSlotPrefab.transform.SetParent(inventoryLayoutGroup.transform, false);

                var newSlot = new Slot(itemsDatabase.Empty, 0);
                newSlotPrefab.Initialize(i, newSlot, this);

                slots.Add(newSlot);
            }
        }

        private void SetInventoryPanelEnabled(bool newState)
        {
            if (isInventoryEnabled != newState)
            {
                isInventoryEnabled = newState;
                inventoryGroup.alpha = newState ? 1f : 0f;
                ToggleInventoryEvent?.Invoke(newState);
            }
        }

        private bool IsSlotEmpty(int index)
        {
            return slots[index].Item == itemsDatabase.Empty;
        }

        private void CleanSlot(int index)
        {
            slots[index].SetItem(itemsDatabase.Empty, 0);
        }
    }
}
