using SimpleInventory.Interaction;
using UnityEngine;

namespace SimpleInventory.Inventory
{
    public class PhysicalItem : MonoBehaviour, IInteractable
    {
        public string InteractionActionName => interactionName;
        public string InteractionObjectName => itemDefinition.Name;

        [SerializeField] private string interactionName = "Grab";
        [SerializeField] private ItemBase itemDefinition;

        public void Interact()
        {
            Debug.Log("interact");
            //TODO: This can be easily replaced with proper pooling system 
            Destroy(this);
        }
    }
}
