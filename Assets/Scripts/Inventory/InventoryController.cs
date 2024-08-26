using SimpleInventory.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private int inventorySize = 20;
        [SerializeField] private Transform inventorySpawnPoint;

        private List<Slot> slots;

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
                SpawnPhysicalItem(slots[slotIndex].Item);
                slots[slotIndex].Clean();
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void SpawnPhysicalItem(IItem item)
        {
            var newSpawnedItem = MonoBehaviour.Instantiate(item.PhysicalPrefab, inventorySpawnPoint.position, inventorySpawnPoint.rotation);
            newSpawnedItem.name = item.Name;
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
            slots = new List<Slot>(inventorySize);
        }
    }
}
