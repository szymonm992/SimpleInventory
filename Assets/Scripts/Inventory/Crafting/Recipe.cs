using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleInventory.Inventory
{
    [CreateAssetMenu(fileName = "CraftRecipe", menuName = "Inventory/CraftRecipe", order = 2)]
    public class Recipe : ScriptableObject
    {
        public int CraftChance => craftChance;
        public ItemBase CraftTarget => craftTarget;
        public IngredientPair[] Ingredients => ingredients;

        [Range(0, 100)]
        [SerializeField] private int craftChance;
        [SerializeField] private ItemBase craftTarget;
        [SerializeField] private IngredientPair[] ingredients;

        public bool IsCraftable(Dictionary<IItem, int> inventoryIngredients)
        {
            Debug.Log(inventoryIngredients.Count);

            if (inventoryIngredients.Any())
            {
                foreach (var requiredIngredient in ingredients)
                {
                    if (!inventoryIngredients.ContainsKey(requiredIngredient.Item) || inventoryIngredients[requiredIngredient.Item] < requiredIngredient.Amount)
                    {
                        return false;
                    }
                }
                
            }

            return true;
        }
    }
}
