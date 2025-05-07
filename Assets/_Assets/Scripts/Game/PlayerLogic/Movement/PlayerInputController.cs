using System;
using System.Collections.Generic;
using _Assets.Scripts.Game.InputLogic;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic.Movement
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 MoveInput;
        public Vector2 LookInput;
        public bool JumpInput;
    }

    public class PlayerInputController : NetworkBehaviour, INetworkRunnerCallbacks
    {
        private IInputService _inputService;
        private Vector3 _movementInput;
        private Vector3 _lookInput;
        private bool _jumpInput;

        private void Start()
        {
            Runner.AddCallbacks(this);
        }

        public void OnDestroy()
        {
            Runner.RemoveCallbacks(this);
        }

        public void Initialize(IInputService inputService)
        {
            if (Object.InputAuthority == Runner.LocalPlayer)
            {
                _inputService = inputService;
                _inputService.MoveInput += MoveInput;
                _inputService.LookInput += LookInput;
                _inputService.JumpInput += JumpInput;
            }
        }

        private void MoveInput(Vector2 moveInput)
        {
            if(!Object.HasInputAuthority)
                return;

            _movementInput = moveInput;
        }

        private void LookInput(Vector2 lookInput)
        {
            if(!Object.HasInputAuthority)
                return;
            
            _lookInput = lookInput;
        }

        private void JumpInput(bool jumpInput)
        {
            if(!Object.HasInputAuthority)
                return;

            _jumpInput = jumpInput;
        }
        
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if(!Object.HasInputAuthority)
                return;
            
            var inputData = new NetworkInputData
            {
                MoveInput = _movementInput,
                LookInput = _lookInput,
                JumpInput = _jumpInput
            };
            
            input.Set(inputData);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}