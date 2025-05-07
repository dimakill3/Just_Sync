using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic.Factory
{
    public interface IPlayerFactory
    {
        public UniTask<Player> CreatePlayer(PlayerRef player);
    }
}