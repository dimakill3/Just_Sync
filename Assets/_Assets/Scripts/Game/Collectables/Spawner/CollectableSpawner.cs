using System;
using System.Collections.Generic;
using System.Threading;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Game.Collectables.Factory;
using _Assets.Scripts.Game.Configs;
using _Assets.Scripts.Game.Utils;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Assets.Scripts.Game.Collectables.Spawner
{
    public class CollectableSpawner : INetworkRunnerCallbacks, IDisposable, IInitializable
    {
        private readonly GameConfig _gameConfig;
        private readonly ICollectablePool _collectablePool;
        private readonly NetworkRunner _networkRunner;
        private readonly SpawnConfig _spawnConfig;

        private int _currentCollectablesCount;
        private CancellationTokenSource _cancellationTokenSource;
        
        public CollectableSpawner(GameConfig gameConfig, ICollectablePool collectablePool, NetworkRunner networkRunner)
        {
            _gameConfig = gameConfig;
            _collectablePool = collectablePool;
            _networkRunner = networkRunner;
            _spawnConfig = gameConfig.SpawnConfig;
        }

        public void Initialize() =>
            _networkRunner.AddCallbacks(this);

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;

            StartSpawning();
        }

        public void Dispose()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }

            _collectablePool.Dispose();
            _networkRunner.RemoveCallbacks(this);
        }

        public void StartSpawning()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Spawning(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid Spawning(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_currentCollectablesCount < _spawnConfig.MaxCollectablesOnMap)
                {
                    SpawnCollectable();
                    _currentCollectablesCount++;
                }
                else
                    break;

                await UniTask.WaitForFixedUpdate();
            }
        }

        private async void SpawnCollectable()
        {
            var selectedConfig = GetRandomCollectableConfig();
            var spawnPosition = MapUtil.GetRandomMapPosition();

            var collectable = await _collectablePool.GetCollectable(selectedConfig);
            collectable.gameObject.transform.position = spawnPosition;
            collectable.Collected += HandleCollectableCollected;
        }

        private void HandleCollectableCollected(Collectable collectable)
        {
            collectable.Collected -= HandleCollectableCollected;
            _currentCollectablesCount--;
        }

        private CollectableConfig GetRandomCollectableConfig() =>
            _gameConfig.CollectableConfigs[Random.Range(0, _gameConfig.CollectableConfigs.Length)];

        #region Unused Callbacks
        
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){}
        public void OnInput(NetworkRunner runner, NetworkInput input){}
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
        public void OnConnectedToServer(NetworkRunner runner){}
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
        public void OnSceneLoadDone(NetworkRunner runner){}
        public void OnSceneLoadStart(NetworkRunner runner) {}
        
        #endregion
    }
}