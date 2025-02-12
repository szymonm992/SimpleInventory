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
        public event Action<bool> ToggleCraftingEvent;

        public IItem EmptyItem => itemsDatabase.Empty;

        [Inject] private readonly ItemsDatabase itemsDatabase;
        [Inject] private readonly IPlayerInputsProvider playerInputsProvider;

        [Header("Physical")]
        [SerializeField] private Transform inventorySpawnPoint;

        [Header("GUI")]
        [SerializeField] private int slotsAmount = 0;
        [SerializeField] private SlotController slotPrefab;
        [SerializeField] private CraftSlotController craftSlotPrefab;
        [SerializeField] private Button craftingButton;
        [SerializeField] private CanvasGroup inventoryGroup;
        [SerializeField] private CanvasGroup craftingGroup;
        [SerializeField] private RectTransform craftingSlotsParent;
        [SerializeField] private GridLayoutGroup inventoryLayoutGroup;

        private readonly List<Slot> slots = new ();
        private readonly List<Recipe> currentCraftable = new ();

        private bool isInventoryEnabled = true;
        private bool isCraftingEnabled = true;

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

            UpdateCrafting();
        }

        public void DropItemFromSlot(int slotIndex)
        {
            if (!IsSlotEmpty(slotIndex))
            {
                if (slots[slotIndex].Item is IGrabableItem grabable)
                {
                    //TODO: Spawning multiple objects that use physics should consider multiple positions/offsetting spawn position by size of spawned object
                    for (int i = 0; i < slots[slotIndex].ItemsAmount; i++)
                    {
                        SpawnGrabbableItem(grabable);
                    }
                }

                CleanSlot(slotIndex);
                UpdateCrafting();
            }
        }

        public void SubstractItemAmount(int slotIndex, int amount)
        {
            if (!IsSlotEmpty(slotIndex))
            {
                if (slots[slotIndex].ItemsAmount > amount)
                {
                    slots[slotIndex].AddAmount(-amount);
                }
                else
                {
                    CleanSlot(slotIndex);
                }

                UpdateCrafting();
            }
        }

        public bool TryCraft(Recipe recipe)
        {
            //TODO: This is a simple implementation of double (secondary) valid-check
            var removalPairs = new Dictionary<int, IngredientPair>();

            foreach (var recipeItem in recipe.Ingredients)
            {
                if (TryGetItemInInventory(recipeItem.Item, out int foundIndex))
                {
                    if (slots[foundIndex].ItemsAmount >= recipeItem.Amount)
                    {
                        removalPairs.Add(foundIndex, recipeItem);
                    }
                    else
                    {
                        Debug.Log("Insufficient amount of item!");
                    }
                }
                else
                {
                    Debug.Log("No item found in ivnentory");
                }
            }

            if (removalPairs.Count == recipe.Ingredients.Length)
            {
                if (TryFindFirstEmptySlot(out _))
                {
                    foreach (var removal in removalPairs)
                    {
                        SubstractItemAmount(removal.Key, removal.Value.Amount);
                    }

                    int randomNumber = UnityEngine.Random.Range(0, 100);
                    if (randomNumber > recipe.CraftChance)
                    {
                        Debug.Log("Craft proccess successful, but craft itse;f failed due to insufficient chance!");
                    }
                    else
                    {
                        TryAddItem(recipe.CraftTarget, 1);
                    }
                    
                    SetCraftingPanelEnabled(false);
                    return true;
                }
                else
                {
                    Debug.Log("No space in inventory found to craft item");
                }
            }

            return false;
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
            craftingButton.onClick.RemoveAllListeners();
        }

        public void PointerClick(int slotIndex, PointerEventData pointerData)
        {
            //TODO: Could be replaced with displaying context menu or options menu instead of just dropping on click
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
                DropItemFromSlot(slotIndex);
            }
        }

        private void UpdateCrafting()
        {
            currentCraftable.Clear();
            var currentIngredients = new Dictionary<IItem, int>();

            for (int i = 0; i < slots.Count; i++)
            {
                if (!IsSlotEmpty(i))
                {
                    currentIngredients.Add(slots[i].Item, slots[i].ItemsAmount);
                }
            }

            foreach (var currentRecipe in itemsDatabase.CraftingRecipes)
            {
                if (currentRecipe.IsCraftable(currentIngredients))
                {
                    currentCraftable.Add(currentRecipe);
                }
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
            SetCraftingPanelEnabled(false);

            craftingButton.onClick.AddListener(OnOpenCraftingWindow);
            playerInputsProvider.InventoryButtonPressedEvent += OnInventoryButtonPressed;
        }

        private void OnOpenCraftingWindow()
        {
            CreateCraftingWindow();
            SetCraftingPanelEnabled(true);
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

        private void CreateCraftingWindow()
        {
            if (craftingSlotsParent.childCount > 0)
            {
                foreach (Transform child in craftingSlotsParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            foreach (var recipe in currentCraftable)
            {
                var newRecipePrefab = Instantiate(craftSlotPrefab, craftingSlotsParent.transform);
                newRecipePrefab.transform.SetParent(craftingSlotsParent.transform, false);

                newRecipePrefab.Initialize(recipe, this);
            }
        }

        private void SetInventoryPanelEnabled(bool newState)
        {
            if (isInventoryEnabled != newState)
            {
                isInventoryEnabled = newState;
                inventoryGroup.alpha = newState ? 1f : 0f;
                inventoryGroup.blocksRaycasts = newState;
                ToggleInventoryEvent?.Invoke(newState);

                if (!newState)
                {
                    SetCraftingPanelEnabled(false);
                }
            }
        }

        private void SetCraftingPanelEnabled(bool newState)
        {
            if (isCraftingEnabled != newState)
            {
                isCraftingEnabled = newState;
                craftingGroup.alpha = newState ? 1f : 0f;
                craftingGroup.blocksRaycasts = newState;
                ToggleCraftingEvent?.Invoke(newState);
            }
        }

        //TODO: These two methods should be implemented internally by Slot class
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
