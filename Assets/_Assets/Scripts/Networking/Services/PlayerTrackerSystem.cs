using System;
using System.Collections.Generic;
using _Assets.Scripts.Game.PlayerLogic;
using _Assets.Scripts.Game.PlayerLogic.Factory;
using _Assets.Scripts.Networking.Initializer;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Zenject;

namespace _Assets.Scripts.Networking.Services
{
    public class PlayerTrackerSystem : NetworkBehaviour, INetworkRunnerCallbacks
    {
        private IPlayerFactory _playerFactory;
        private NetworkRunner _networkRunner;
        private INetworkInitializer _initializer;
        private IObjectsInitializer _objectsInitializer;

        [Networked, Capacity(30)] public NetworkDictionary<PlayerRef, Player> Players { get; }
        
        [Inject]
        public void Construct(IPlayerFactory playerFactory, NetworkRunner networkNetworkRunner,
            INetworkInitializer initializer, IObjectsInitializer objectsInitializer)
        {
            _playerFactory = playerFactory;
            _networkRunner = networkNetworkRunner;
            _initializer = initializer;
            _objectsInitializer = objectsInitializer;
        }

        private void Start() =>
            _networkRunner.AddCallbacks(this);

        public void OnDestroy() =>
            _networkRunner.RemoveCallbacks(this);

        public async void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;
            
            var playerObject = await _playerFactory.CreatePlayer(player);
            Players.Add(player, playerObject);
            RPC_PlayerJoined(player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;
            
            if (player != runner.LocalPlayer)
            {
                var playerObject = Players[player];
                if (playerObject != null)
                {
                    playerObject.OnDeath -= HandlePlayerDeath;
                    runner.Despawn(playerObject.Object);
                    Players.Remove(player);
                }
            }
            else
                RPC_HostLeft();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) =>
            _initializer.LeftGame();
        
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) =>
            _initializer.LeftGame();

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private async void RPC_PlayerJoined(PlayerRef player)
        {
            var playerObject = Players[player];
            while (playerObject == null)
            {
                await UniTask.Yield();
                playerObject = Players[player];
            }
            
            _objectsInitializer.InitializePlayer(playerObject);

            if (_networkRunner.IsServer)
                playerObject.OnDeath += HandlePlayerDeath;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_HostLeft() =>
            _initializer.LeftGame();

        private void HandlePlayerDeath(PlayerRef player)
        {
            if (!_networkRunner.IsServer)
                return;

            var playerObject = Players[player];
            if (playerObject == null)
                return;
            
            playerObject.RPC_Respawn();
        }
        
        #region Unused Callbacks

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        
        #endregion
    }
}