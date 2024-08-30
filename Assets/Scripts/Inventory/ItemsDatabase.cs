using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory.Inventory
{
    [CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Inventory/ItemsDatabase", order = 0)]
    public class ItemsDatabase : ScriptableObject
    {
        public IItem Empty => emptyItem;
        public IReadOnlyList<Recipe> CraftingRecipes => recipes;

        [SerializeField] private ItemBase[] items;
        [SerializeField] private Recipe[] recipes;
        [SerializeField] private ItemBase emptyItem;
    }
}
