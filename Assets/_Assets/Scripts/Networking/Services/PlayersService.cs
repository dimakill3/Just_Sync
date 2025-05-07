using System.Collections.Generic;
using System.Linq;
using _Assets.Scripts.Game.PlayerLogic;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Assets.Scripts.Networking.Services
{
    public class PlayersService : IPlayersService
    {
        private readonly NetworkRunner _networkRunner;

        private Dictionary<PlayerRef, Player> _players = new();
        private Player _localPlayer;
        
        public PlayersService(NetworkRunner networkRunner) =>
            _networkRunner = networkRunner;

        public void AddPlayer(PlayerRef playerRef, Player newPlayer)
        {
            if (_players.TryAdd(playerRef, newPlayer))
            {
                if (playerRef == _networkRunner.LocalPlayer)
                    _localPlayer = newPlayer;
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.All)]
        public void SetLocalPlayer() =>
            _localPlayer = _networkRunner.GetPlayerObject(_networkRunner.LocalPlayer).GetComponent<Player>();

        public Player GetPlayer(PlayerRef player) =>
            _players.FirstOrDefault(p => p.Key == player).Value;

        public async UniTask<Player> GetLocalPlayer()
        {
            while (_localPlayer == null)
                await UniTask.Yield();

            return _localPlayer;
        }

        public void Remove(PlayerRef player) =>
            _players.Remove(player);

        public void Dispose() =>
            _players.Clear();
    }
}