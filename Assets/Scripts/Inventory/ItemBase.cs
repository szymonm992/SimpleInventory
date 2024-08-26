using UnityEngine;

namespace SimpleInventory.Inventory
{
    public abstract class ItemBase : IItem
    {
        public string Name => name;
        public string Description => description;
        public Texture2D Icon => icon;
        public GameObject PhysicalPrefab => physicalPrefab;

        [Header("Basic proeprties")]
        [SerializeField] protected string name;
        [SerializeField] protected string description;
        [SerializeField] protected Texture2D icon;
        [SerializeField] protected GameObject physicalPrefab;
    }
}
