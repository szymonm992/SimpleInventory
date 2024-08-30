using UnityEngine;

namespace SimpleInventory.Inventory
{
    [System.Serializable]
    public class IngredientPair 
    {
        public int Amount => amount;
        public ItemBase Item => item;

        [SerializeField] private ItemBase item;
        [SerializeField] private int amount;

        public IngredientPair(ItemBase item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
