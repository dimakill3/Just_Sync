using System.Collections.Generic;
using System.Linq;
using _Assets.Scripts.Core.Infrastructure.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Assets.Scripts.Core.Infrastructure.WindowManagement
{
    public class WindowProvider : MonoBehaviour
    {
        private List<BaseWindow> _windows = new();
        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _sceneLoader.SceneLoaded += HandleSceneLoaded;
        }
        
        public void Initialize() =>
            _windows.AddRange(GetComponentsInChildren<BaseWindow>(includeInactive: true));

        private void OnDestroy() =>
            _sceneLoader.SceneLoaded -= HandleSceneLoaded;

        private void HandleSceneLoaded()
        {
            _windows.Clear();
            CollectWindowsFromCurrentScene();
        }

        private void CollectWindowsFromCurrentScene()
        {
            _windows.AddRange(GetComponentsInChildren<BaseWindow>(includeInactive: true));
            
            CollectWindowsFromScene(SceneManager.GetActiveScene());
        }
        
        private void CollectWindowsFromScene(Scene scene)
        {
            var canvases = scene.GetRootGameObjects().Where(obj => obj.GetComponentsInChildren<Canvas>() != null);
            
            foreach (var canvas in canvases)
            {
                var sceneWindows = canvas.GetComponentsInChildren<BaseWindow>(true);
                
                foreach (var window in sceneWindows)
                    if (!_windows.Contains(window))
                        _windows.Add(window);
            }
        }

        public T GetWindow<T>() where T : BaseWindow
        {
            foreach (var window in _windows)
                if (window is T needWindow)
                    return needWindow;

            return default;
        }

        public T ShowWindow<T>(bool animated = true) where T : BaseWindow
        {
            var needWindow = GetWindow<T>();
            needWindow.Show(animated);
            return needWindow;
        }

        public T HideWindow<T>(bool animated = true) where T : BaseWindow
        {
            foreach (var window in _windows)
                if (window is T needWindow)
                {
                    window.Hide(animated);
                    return needWindow;
                }

            return default;
        }

        public void HideAllWindows(bool animated = false)
        {
            foreach (var window in _windows)
                window.Hide(animated);
        }

        public void WarmUpWindows()
        {
            foreach (var window in _windows)
            {
                window.Show(false);
                window.Hide(false);
            }
        }
    }
}