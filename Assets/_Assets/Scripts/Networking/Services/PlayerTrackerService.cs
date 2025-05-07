using System;
using System.Collections.Generic;
using _Assets.Scripts.Game.PlayerLogic;
using _Assets.Scripts.Game.PlayerLogic.Factory;
using _Assets.Scripts.Networking.Initializer;
using Fusion;
using Fusion.Sockets;
using IInitializable = Zenject.IInitializable;

namespace _Assets.Scripts.Networking.Services
{
    public class PlayerTrackerService : INetworkRunnerCallbacks, IInitializable, IDisposable
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly NetworkRunner _networkRunner;
        private readonly IPlayersService _playersService;
        private readonly INetworkInitializer _initializer;
        private readonly IObjectsInitializer _objectsInitializer;

        public PlayerTrackerService(IPlayerFactory playerFactory, NetworkRunner networkNetworkRunner,
            IPlayersService playersService, INetworkInitializer initializer, IObjectsInitializer objectsInitializer)
        {
            _playerFactory = playerFactory;
            _networkRunner = networkNetworkRunner;
            _playersService = playersService;
            _initializer = initializer;
            _objectsInitializer = objectsInitializer;
        }

        public void Initialize() =>
            _networkRunner.AddCallbacks(this);

        public void Dispose() =>
            _networkRunner.RemoveCallbacks(this);

        public async void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;
            
            await _playerFactory.CreatePlayer(player);
            RPC_PlayerJoined(player);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) =>
            _initializer.LeftGame();

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;
            
            if (player != runner.LocalPlayer)
            {
                var obj = runner.GetPlayerObject(player);
                if (obj != null)
                    runner.Despawn(obj);
                
                RPC_PlayerLeft(player);
            }
            else
                RPC_HostLeft();
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) =>
            _initializer.LeftGame();

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_PlayerLeft(PlayerRef player) =>
            _playersService.Remove(player);
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_HostLeft() =>
            _initializer.LeftGame();

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_PlayerJoined(PlayerRef player)
        {
            var networkObj = _networkRunner.GetPlayerObject(player);
            if (networkObj == null)
                return;

            var playerComponent = networkObj.GetComponent<Player>();
            _objectsInitializer.InitializePlayer(playerComponent);
            _playersService.AddPlayer(player, playerComponent);

            if (_networkRunner.IsServer)
            {
                playerComponent.OnDeath -= HandlePlayerDeath;
                playerComponent.OnDeath += HandlePlayerDeath;
            }
        }

        private void HandlePlayerDeath(PlayerRef player)
        {
            if (!_networkRunner.IsServer)
                return;

            var playerObj = _networkRunner.GetPlayerObject(player);
            if (playerObj == null)
                return;

            var playerComponent = playerObj.GetComponent<Player>();
            playerComponent.RPC_Respawn();
        }
        
        #region INetworkRunnerCallbacks

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        
        #endregion
    }
}