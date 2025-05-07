using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Game.InputLogic;
using _Assets.Scripts.Game.Utils;
using _Assets.Scripts.Networking.Services;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Game.PlayerLogic.Factory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameConfig _gameConfig;
        private readonly NetworkRunner _networkRunner;

        [Inject]
        public PlayerFactory(DiContainer diContainer, IAssetProvider assetProvider, GameConfig gameConfig,
            IInputService inputService, Camera camera, NetworkRunner networkRunner)
        {
            _assetProvider = assetProvider;
            _gameConfig = gameConfig;
            _networkRunner = networkRunner;
        }

        public async UniTask<Player> CreatePlayer(PlayerRef player)
        {
            if (!_networkRunner.IsRunning)
                return null;

            var prefab = await _assetProvider.Load<GameObject>(_gameConfig.PlayerConfig.AddressableId);
            var networkPlayer = prefab.GetComponent<Player>();
            
            var createdPlayer = _networkRunner.Spawn(
                networkPlayer,
                inputAuthority: player,
                position: MapUtil.GetRandomMapPosition(),
                rotation: Quaternion.identity);
            
            _networkRunner.SetPlayerObject(player, createdPlayer.Object);
            
            return createdPlayer;
        }
    }
}