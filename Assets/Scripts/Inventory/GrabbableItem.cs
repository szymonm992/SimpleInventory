using SimpleInventory.Interaction;
using UnityEngine;

namespace SimpleInventory.Inventory
{
    public class GrabbableItem : MonoBehaviour, IInteractable
    {
        public string InteractionName => interactionName;

        [SerializeField] private string interactionName = "Grab";

        public void Interact()
        {
            Debug.Log("interact");
            //TODO: Can be easily replaced with proper pooling system 
            Destroy(this);
        }
    }
}
