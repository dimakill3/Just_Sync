using System;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.SceneManagement;

namespace _Assets.Scripts.Networking.Initializer
{
    public class NetworkInitializer : INetworkInitializer
    {
        private const string SessionName = "TestSession";
        
        public event Action GameLeft;

        private readonly NetworkRunner _networkRunner;
        private readonly NetworkSceneManagerDefault _networkSceneManager;
        private readonly ZenjectNetworkObjectProvider _zenjectNetworkObjectProvider;

        public NetworkInitializer(NetworkRunner networkRunner, NetworkSceneManagerDefault networkSceneManager,
            ZenjectNetworkObjectProvider zenjectNetworkObjectProvider)
        {
            _networkRunner = networkRunner;
            _networkSceneManager = networkSceneManager;
            _zenjectNetworkObjectProvider = zenjectNetworkObjectProvider;
        }

        public async UniTask<bool> HostGame(string sceneName, Action<bool> onLoaded) =>
            await StartGame(GameMode.Host, sceneName, onLoaded);

        public async UniTask<bool> JoinGame(string sceneName, Action<bool> onLoaded) =>
            await StartGame(GameMode.Client, sceneName, onLoaded);

        public void LeftGame()
        {
            if (_networkRunner.IsRunning)
                _networkRunner.Shutdown();
            
            GameLeft?.Invoke();
        }

        private async UniTask<bool> StartGame(GameMode mode, string sceneName, Action<bool> onLoaded)
        {
            _networkRunner.ProvideInput = true;

            var scene = GetScene(sceneName);
            
            if (scene == SceneRef.None)
                return false;
            
            var results = await _networkRunner.StartGame(new StartGameArgs
            {
                GameMode = mode,
                SessionName = SessionName,
                Scene = scene,
                SceneManager = _networkSceneManager,
                ObjectProvider =  _zenjectNetworkObjectProvider
            });
            
            onLoaded?.Invoke(results.Ok);
            
            return results.Ok; 
        }

        private SceneRef GetScene(string sceneName)
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            try
            {
                var buildIndex = -1;
                for (var i = 0; i < sceneCount; i++)
                {
                    var path = SceneUtility.GetScenePathByBuildIndex(i);
                    var name = System.IO.Path.GetFileNameWithoutExtension(path);

                    if (name == sceneName)
                    {
                        buildIndex = i;
                        break;
                    }
                }
            
                var scene = SceneRef.FromIndex(buildIndex);
                var sceneInfo = new NetworkSceneInfo();
                if (scene.IsValid)
                    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
                
                return scene;
            }
            catch (Exception e)
            {
                return SceneRef.None;
            }
        }
    }
}