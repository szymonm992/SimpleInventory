using UnityEngine;

namespace SimpleInventory.Inputs
{
    public interface IPlayerInputsProvider 
    {
        /// <summary>
        /// Provides movement vector
        /// </summary>
        Vector2 Movement { get; }

        /// <summary>
        /// Provides mouse delta
        /// </summary>
        Vector2 MouseDelta { get; }

        /// <summary>
        /// Provides interaction button state
        /// </summary>
        bool Interact { get; }

        /// <summary>
        /// Provides inventory button state
        /// </summary>
        bool InventoryKey { get; }

        /// <summary>
        /// Contains logic on object dispose
        /// </summary>
        void Dispose();
    }
}
