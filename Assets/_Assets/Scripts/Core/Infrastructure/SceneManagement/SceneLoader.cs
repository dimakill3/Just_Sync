using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Assets.Scripts.Core.Infrastructure.SceneManagement
{
    public class SceneLoader : ISceneLoader
    {
        public event Action SceneLoaded;

        public void Load(string name, Action onLoaded = null, bool isForce = false) =>
            LoadScene(name, onLoaded, isForce).Forget();

        private async UniTask LoadScene(string name, Action onLoaded = null, bool isForce = false)
        {
            if (isForce == false && SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            var sceneLoading = SceneManager.LoadSceneAsync(name);

            if (sceneLoading == null)
                return;
            
            while (!sceneLoading.isDone)
                await UniTask.WaitForEndOfFrame();

            SceneLoaded?.Invoke();
            onLoaded?.Invoke();
        }
    }
}