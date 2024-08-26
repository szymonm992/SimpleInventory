using UnityEngine;

namespace SimpleInventory.Inventory
{
    public interface IItem 
    {
        public string Name { get; }
        public string Description { get; }
        public Texture2D Icon { get; }
        public GameObject PhysicalPrefab { get; }
    }
}
