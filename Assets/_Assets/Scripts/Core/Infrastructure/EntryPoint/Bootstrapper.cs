using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates;
using _Assets.Scripts.Core.Infrastructure.SceneManagement;
using _Assets.Scripts.Core.Infrastructure.WindowManagement;
using _Assets.Scripts.Networking.Initializer;
using IInitializable = Zenject.IInitializable;

namespace _Assets.Scripts.Core.Infrastructure.EntryPoint
{
    public class Bootstrapper : IInitializable
    {
        private StateMachine.StateMachine _stateMachine;
        
        private readonly ISceneLoader _sceneLoader;
        private readonly WindowProvider _windowProvider;
        private readonly GameConfig _gameConfig;
        private readonly INetworkInitializer _networkInitializer;
        private readonly IEventProvider _eventProvider;

        public Bootstrapper(ISceneLoader sceneLoader, WindowProvider windowProvider,GameConfig gameConfig,
            INetworkInitializer networkInitializer, IEventProvider eventProvider)
        {
            _sceneLoader = sceneLoader;
            _windowProvider = windowProvider;
            _gameConfig = gameConfig;
            _networkInitializer = networkInitializer;
            _eventProvider = eventProvider;
        }

        public void Initialize()
        {
            InitializeStateMachine();
            StartGame();
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine.StateMachine();
            _stateMachine.AddState(new EnterLobbyState(_stateMachine, _windowProvider, _sceneLoader));
            _stateMachine.AddState(new InLobbyState(_stateMachine, _windowProvider, _gameConfig, _networkInitializer, _eventProvider));
        }

        private void StartGame() =>
            _stateMachine.Enter<EnterLobbyState>();
    }
}