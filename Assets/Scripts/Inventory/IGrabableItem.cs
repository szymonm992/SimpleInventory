using UnityEngine;

namespace SimpleInventory.Inventory
{
    public interface IGrabableItem : IItem
    {
        GameObject PhysicalObjectPrefab { get; }
    }
}
