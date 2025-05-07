using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Game.Configs;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Game.Collectables.Factory
{
    public class CollectableFactory : ICollectableFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly NetworkRunner _networkRunner;

        [Inject]
        public CollectableFactory(IAssetProvider assetProvider, NetworkRunner networkRunner)
        {
            _assetProvider = assetProvider;
            _networkRunner = networkRunner;
        }

        public async UniTask<Collectable> GetCollectablePrefab(CollectableConfig collectableConfig)
        {
            var prefab = await _assetProvider.Load<GameObject>(collectableConfig.AddressableId);
            return prefab.GetComponent<Collectable>();
        }
        
        public Collectable CreateCollectable(Collectable collectablePrefab)
        {
            if (!_networkRunner.IsRunning)
                return null;
            
            var collectable = _networkRunner.Spawn(collectablePrefab);
            return collectable;
        }
    }
}