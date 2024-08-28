using SimpleInventory.Interaction;
using UnityEngine;
using Zenject;

namespace SimpleInventory.Inventory
{
    public class PhysicalItem : InteractableBase
    {
        public override string InteractionActionName => interactionName;
        public override string InteractionObjectName => itemDefinition.Name;
       
        [SerializeField] private string interactionName = "Grab";
        [SerializeField] private ItemBase itemDefinition;

        private InventoryController inventoryController;

        public override void Interact()
        {
            inventoryController.TryAddItem(itemDefinition, 1);
            Destroy(this.gameObject);
        }

        protected override void Initialize()
        {
            //TODO: Should be refactored with proper spawning system (kind of complicated to spawn dynamic and make work objects in Zenject)
            inventoryController = FindObjectOfType<InventoryController>();
        }
    }
}
