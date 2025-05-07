using System;

namespace _Assets.Scripts.Core.Infrastructure.SceneManagement
{
    public interface ISceneLoader
    {
        event Action SceneLoaded;
        
        void Load(string name, Action onLoaded = null, bool isForce = false);
    }
}