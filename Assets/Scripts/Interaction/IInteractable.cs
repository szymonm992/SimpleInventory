namespace SimpleInventory.Interaction
{
    public interface IInteractable 
    {
        /// <summary>
        /// Used to determine whether detection is "Grab", "Use", etc...
        /// </summary>
        string InteractionName { get; }

        /// <summary>
        /// Contains interaction logic
        /// </summary>
        void Interact();
    }
}
