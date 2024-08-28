using SimpleInventory.Inputs;
using SimpleInventory.Inventory;
using UnityEngine;
using Zenject;

namespace SimpleInventory.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        public const string PLAYER_CAMERA_ID = "PlayerCamera";
        [SerializeField] private Camera playerCamera;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().WithId(PLAYER_CAMERA_ID).FromInstance(playerCamera).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInteractor>().FromComponentInHierarchy().AsSingle();
        }
    }
}
