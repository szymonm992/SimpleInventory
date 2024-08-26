using UnityEngine;

namespace SimpleInventory.Inputs
{
    public interface IPlayerInputsProvider 
    {
        Vector2 Movement { get; }
        Vector2 MouseDelta { get; }
        bool Interact { get; }
        bool InventoryKey { get; }

        void Dispose();
    }
}
