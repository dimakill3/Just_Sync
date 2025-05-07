using System.IO;
using System.Linq;
using _Assets.Scripts.Game.Configs;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Assets.Scripts.Core.Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [ValueDropdown("GetBuildScenes")]
        public string StartLevelScene;
        public PlayerConfig PlayerConfig;
        public CollectableConfig[] CollectableConfigs;
        public SpawnConfig SpawnConfig;
        public InputConfig InputConfig;

#if UNITY_EDITOR
        private static string[] GetBuildScenes()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => Path.GetFileNameWithoutExtension(scene.path))
                .ToArray();
        }
#endif
    }
}