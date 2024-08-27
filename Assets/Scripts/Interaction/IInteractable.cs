namespace SimpleInventory.Interaction
{
    public interface IInteractable 
    {
        /// <summary>
        /// Used to determine whether detection is "Grab", "Use", etc...
        /// </summary>
        string InteractionActionName { get; }

        /// <summary>
        /// Used to present the name of the object
        /// </summary>
        string InteractionObjectName { get; }

        /// <summary>
        /// Contains interaction logic
        /// </summary>
        void Interact();
    }
}
