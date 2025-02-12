using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleInventory.GUI;
using UnityEngine.EventSystems;

namespace SimpleInventory.Inventory
{
    public class CraftSlotController : ReactiveSlot
    {
        [SerializeField] private TextMeshProUGUI craftableName;
        [SerializeField] private TextMeshProUGUI craftableDescription;
        [SerializeField] private TextMeshProUGUI craftableChance;

        private InventoryController inventoryController;
        private Recipe recipe;

        public void Initialize(Recipe recipe, InventoryController inventoryController)
        {
            this.recipe = recipe;
            this.inventoryController = inventoryController;

            UpdateVisuals();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (!inventoryController.TryCraft(recipe))
            {
                Debug.Log("Craft failed!");
            }
        }

        private void UpdateVisuals()
        {
            craftableName.text = recipe.CraftTarget.Name;
            craftableDescription.text = recipe.CraftTarget.Description;
            craftableChance.text = $"Craft chance {recipe.CraftChance}%";
        }
    }
}
