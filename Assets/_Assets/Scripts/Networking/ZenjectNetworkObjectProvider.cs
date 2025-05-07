using Fusion;
using Zenject;

namespace _Assets.Scripts.Networking
{
    public class ZenjectNetworkObjectProvider : NetworkObjectProviderDefault
    {
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container) => 
            _container = container;

        protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
        {
            var networkObject = _container.InstantiatePrefabForComponent<NetworkObject>(prefab);
            return networkObject;
        }
    }
}