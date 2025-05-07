using _Assets.Scripts.Core.Infrastructure.EntryPoint;
using Zenject;

namespace _Assets.Scripts.Core.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootstrapper>()
                .AsSingle();
        }
    }
}