using _Assets.Scripts.Game.Collectables.Factory;
using _Assets.Scripts.Game.Collectables.Spawner;
using _Assets.Scripts.Game.PlayerLogic.Factory;
using _Assets.Scripts.Networking.Services;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindSceneComponents();
            BindServices();
        }

        private void BindSceneComponents()
        {
            Container
                .Bind<Camera>()
                .FromInstance(Camera.main)
                .AsTransient();
        }

        private void BindFactories()
        {
            Container
                .Bind<IPlayerFactory>()
                .To<PlayerFactory>()
                .AsSingle();
            
            Container
                .Bind<ICollectableFactory>()
                .To<CollectableFactory>()
                .AsSingle();
            
            Container
                .Bind<ICollectablePool>()
                .To<CollectablePool>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<CollectableSpawner>()
                .AsSingle();
        }

        private void BindServices()
        {
            Container
                .Bind<IObjectsInitializer>()
                .To<ObjectsInitializer>()
                .AsSingle();
        }
    }
}