using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Game.InputLogic;
using _Assets.Scripts.Game.PlayerLogic;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Networking.Services
{
    public class ObjectsInitializer : IObjectsInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly IInputService _inputService;
        private readonly Camera _camera;
        private readonly NetworkRunner _networkRunner;

        [Inject]
        public ObjectsInitializer(GameConfig gameConfig, IInputService inputService, Camera camera, NetworkRunner networkRunner)
        {
            _gameConfig = gameConfig;
            _inputService = inputService;
            _camera = camera;
            _networkRunner = networkRunner;
        }

        public void InitializePlayer(Player player)
        {
            if (player.Object.InputAuthority == _networkRunner.LocalPlayer)
            {
                player.InitializeOnLocal(_gameConfig.PlayerConfig, _camera.transform, _inputService);
                if (_networkRunner.IsServer)
                    player.InitializeOnOthers(_gameConfig.PlayerConfig);
            }
            else
                player.InitializeOnOthers(_gameConfig.PlayerConfig);
        }
    }
}