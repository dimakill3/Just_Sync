using System;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Networking.Initializer
{
    public interface INetworkInitializer
    {
        public event Action GameLeft;
        
        public UniTask<bool> HostGame(string sceneName, Action<bool> onLoaded);
        public UniTask<bool> JoinGame(string sceneName, Action<bool> onLoaded);
        public void LeftGame();
    }
}