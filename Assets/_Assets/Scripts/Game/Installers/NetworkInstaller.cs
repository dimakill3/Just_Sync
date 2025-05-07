using _Assets.Scripts.Networking;
using _Assets.Scripts.Networking.Initializer;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Game.Installers
{
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private NetworkRunner networkRunner;
        [SerializeField] private NetworkSceneManagerDefault networkSceneManager;
        [SerializeField] private ZenjectNetworkObjectProvider zenjectNetworkObjectProvider;
        
        public override void InstallBindings()
        {
            Container
                .Bind<NetworkRunner>()
                .FromInstance(networkRunner)
                .AsSingle(); 
            
            Container
                .Bind<NetworkSceneManagerDefault>()
                .FromInstance(networkSceneManager)
                .AsSingle();
            
            Container
                .Bind<ZenjectNetworkObjectProvider>()
                .FromInstance(zenjectNetworkObjectProvider)
                .AsSingle();
            
            Container
                .Bind<INetworkInitializer>()
                .To<NetworkInitializer>()
                .AsSingle()
                .NonLazy();
        }
    }
}