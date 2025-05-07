using System;
using _Assets.Scripts.Game.PlayerLogic;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Assets.Scripts.Networking.Services
{
    public interface IPlayersService : IDisposable
    {
        public void AddPlayer(PlayerRef playerRef, Player newPlayer);
        public Player GetPlayer(PlayerRef player);
        public UniTask<Player> GetLocalPlayer();
        void Remove(PlayerRef player);
    }
}