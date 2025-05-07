using _Assets.Scripts.Core.Infrastructure.Configs;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Game.Installers
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        
        public override void InstallBindings() =>
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
    }
}