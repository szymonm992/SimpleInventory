using SimpleInventory.Inventory;
using UnityEngine;
using Zenject;
using SimpleInventory.Inputs;

namespace SimpleInventory.Scene
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private ItemsDatabase itemsDatabase;
        [SerializeField] private InventoryController inventoryController;

        public override void InstallBindings()
        {
            Container.Bind<ItemsDatabase>().FromInstance(itemsDatabase).AsSingle();
            Container.Bind<CursorController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputsProvider>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InventoryController>().FromInstance(inventoryController).AsSingle();
        }
    }
}
