using UnityEngine;

namespace SimpleInventory.Inventory
{
    public abstract class ItemBase : ScriptableObject, IItem
    {
        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;

        [Header("Basic proeprties")]
        [SerializeField] protected new string name;
        [SerializeField] protected string description;
        [SerializeField] protected Sprite icon;
    }
}
