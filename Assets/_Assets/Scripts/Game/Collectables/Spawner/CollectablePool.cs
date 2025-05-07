using System.Collections.Generic;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Game.Collectables.Factory;
using _Assets.Scripts.Game.Configs;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.Pool;

namespace _Assets.Scripts.Game.Collectables.Spawner
{
    public class CollectablePool : ICollectablePool
    {
        private readonly ICollectableFactory _collectableFactory;
        private readonly NetworkRunner _networkRunner;
        private readonly Dictionary<CollectableType, ObjectPool<Collectable>> _pools = new();
        private readonly Dictionary<CollectableType, Collectable> _prefabPools = new();
        private readonly int _maxSize;
        
        public CollectablePool(
            ICollectableFactory collectableFactory, NetworkRunner networkRunner, GameConfig gameConfig)
        {
            _collectableFactory= collectableFactory;
            _networkRunner = networkRunner;
            _maxSize = gameConfig.SpawnConfig.MaxCollectablesOnMap;
        }

        public async UniTask<Collectable> GetCollectable(CollectableConfig config)
        {
            if (!_networkRunner.IsServer)
                return null;
            
            if (!_pools.TryGetValue(config.CollectableType, out var pool))
                pool = await InitializePool(config);
            
            return pool.Get();
        }

        public void Dispose()
        {
            foreach (var pool in _pools.Values)
                pool.Dispose();
            _pools.Clear();
        }

        private async UniTask<ObjectPool<Collectable>> InitializePool(CollectableConfig config)
        {
            var key = config.CollectableType;

            Collectable collectablePrefab;
            
            if (!_prefabPools.ContainsKey(key))
            {
                collectablePrefab = await _collectableFactory.GetCollectablePrefab(config);
                _prefabPools[key] = collectablePrefab;
            }
            
            collectablePrefab = _prefabPools[key];
            
            var pool = new ObjectPool<Collectable>(
                createFunc: () => CreateInstance(collectablePrefab, config),
                actionOnGet: c => c.gameObject.SetActive(true),
                actionOnRelease: c => c.gameObject.SetActive(false),
                actionOnDestroy: c => Despawn(c),
                collectionCheck: false,
                defaultCapacity: _maxSize,
                maxSize: _maxSize);

            _pools[config.CollectableType] = pool;
            return pool;
        }

        private Collectable CreateInstance(Collectable collectablePrefab, CollectableConfig collectableConfig)
        {
            var collectable = _collectableFactory.CreateCollectable(collectablePrefab);
            collectable.Initialize(collectableConfig);
            collectable.Collected += Release;
            return collectable;
        }

        private void Release(Collectable coll)
        {
            if (_pools.TryGetValue(coll.CollectableType, out var pool))
                pool.Release(coll);
        }

        private void Despawn(Collectable coll)
        {
            if (coll && coll.Object != null)
            {
                coll.Collected -= Release;
                _networkRunner.Despawn(coll.Object);
            }
            UnityEngine.Object.Destroy(coll.gameObject);
        }
    }
}