using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Game.InputLogic;
using _Assets.Scripts.Game.PlayerLogic;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Networking.Services
{
    public class ObjectsInitializer : IObjectsInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly IInputService _inputService;
        private readonly Camera _camera;

        [Inject]
        public ObjectsInitializer(GameConfig gameConfig, IInputService inputService, Camera camera)
        {
            _gameConfig = gameConfig;
            _inputService = inputService;
            _camera = camera;
        }

        public void InitializePlayer(Player player) =>
            player.Initialize(_gameConfig, _camera.transform, _inputService);
    }
}