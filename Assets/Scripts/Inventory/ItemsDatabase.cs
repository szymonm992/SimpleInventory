using UnityEngine;

namespace SimpleInventory.Inventory
{
    [CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Inventory/ItemsDatabase", order = 0)]
    public class ItemsDatabase : ScriptableObject
    {
        public IItem Empty => emptyItem;

        [SerializeField] private ItemBase[] items;
        [SerializeField] private ItemBase emptyItem;
    }
}
