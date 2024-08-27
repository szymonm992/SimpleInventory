using SimpleInventory.Inputs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleInventory.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public event Action<bool> ToggleInventoryEvent;

        [Header("Physical")]
        [SerializeField] private Transform inventorySpawnPoint;

        [Header("GUI")]
        [SerializeField] private SlotController slotPrefab;
        [SerializeField] private ItemsDatabase itemsDatabase;
        [SerializeField] private int slotsAmount = 0;
        [SerializeField] private CanvasGroup inventoryGroup;
        [SerializeField] private GridLayoutGroup inventoryLayoutGroup;

        private readonly List<Slot> slots = new ();
        private bool isInventoryEnabled = true;
        private IPlayerInputsProvider playerInputsProvider;

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
            if (!slots[slotIndex].IsEmpty())
            {
                if (slots[slotIndex] is IGrabableItem grabable)
                {
                    SpawnGrabbableItem(grabable);
                }
                
                slots[slotIndex].Clean();
            }
        }

        private void SpawnGrabbableItem(IGrabableItem grabableItem)
        {
            //TODO: Can be replaced with proper pooling system
            var newSpawnedItem = Instantiate(grabableItem.PhysicalObjectPrefab, inventorySpawnPoint.position, inventorySpawnPoint.rotation);
            newSpawnedItem.name = grabableItem.Name;
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
                if (slots[i].IsEmpty())
                {
                    foundIndex = i;
                    return true;
                }
            }

            return false;
        }

        private void Initialize()
        {
            CreateInventory();
            SetInventoryPanelEnabled(false);

            playerInputsProvider.InventoryButtonPressedEvent += OnInventoryButtonPressed;
        }

        private void OnInventoryButtonPressed()
        {
            SetInventoryPanelEnabled(!isInventoryEnabled);
        }

        private void Awake()
        {
            Initialize();
        }

        private void CreateInventory()
        {
            for (int i = 0; i < slotsAmount; i++)
            {
                var newSlotPrefab = Instantiate(slotPrefab, inventoryLayoutGroup.transform);
                newSlotPrefab.transform.SetParent(inventoryLayoutGroup.transform, false);

                var newSlot = new Slot(itemsDatabase.Empty, 0);
                newSlotPrefab.Initialize(newSlot);

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

        private void OnDestroy()
        {
            foreach (var slot in slots)
            {
                slot.Dispose();
            }

            playerInputsProvider.InventoryButtonPressedEvent -= OnInventoryButtonPressed;
        }
    }
}
