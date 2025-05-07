using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Core.Infrastructure.Mono;
using _Assets.Scripts.Core.Infrastructure.SceneManagement;
using _Assets.Scripts.Core.Infrastructure.WindowManagement;
using _Assets.Scripts.Game.InputLogic;
using UnityEngine;
using Zenject;
using EventProvider = _Assets.Scripts.Core.Infrastructure.EventManagement.EventProvider;

namespace _Assets.Scripts.Game.Installers
{
    public class ServicesInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private WindowProvider windowProvider;
        [SerializeField] private MonoService monoService;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<ServicesInstaller>()
                .FromInstance(this)
                .AsSingle();
            
            Container
                .Bind<IInputService>()
                .To<PcInputService>()
                .AsSingle();
         
            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
         
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();

            Container
                .Bind<IEventProvider>()
                .To<EventProvider>()
                .AsSingle();

            Container
                .Bind<MonoService>()
                .FromInstance(monoService)
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<WindowProvider>().FromInstance(windowProvider)
                .AsSingle()
                .NonLazy();
        }

        public void Initialize() =>
            windowProvider.Initialize();
    }
}