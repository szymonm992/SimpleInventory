using UnityEngine;

namespace SimpleInventory.Interaction
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        public virtual string InteractionActionName => "Interact";

        public virtual string InteractionObjectName => "DefaultName";

        public virtual void Interact()  { }

        protected virtual void Initialize() { }

        private void Awake()
        {
            Initialize();
        }
    }
}
