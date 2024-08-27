using UnityEngine;

namespace SimpleInventory.Inventory
{
    [CreateAssetMenu(fileName = "GrabbableItem", menuName = "Inventory/GrabbableItem", order = 1)]
    public class GrabbableItem : ItemBase, IGrabableItem
    {
        public GameObject PhysicalObjectPrefab => physicalObjectPrefab;

        [SerializeField] private GameObject physicalObjectPrefab;
    }
}
