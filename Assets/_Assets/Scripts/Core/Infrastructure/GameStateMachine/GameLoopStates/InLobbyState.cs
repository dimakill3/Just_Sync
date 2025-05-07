using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Core.Infrastructure.StateMachine;
using _Assets.Scripts.Core.Infrastructure.WindowManagement;
using _Assets.Scripts.Game.Events;
using _Assets.Scripts.Game.UI;
using _Assets.Scripts.Networking.Initializer;

namespace _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates
{
    public class InLobbyState : GameLoopState
    {
        private MainMenu _mainMenu;
        private readonly WindowProvider _windowProvider;
        private readonly GameConfig _gameConfig;
        private readonly INetworkInitializer _networkInitializer;
        private readonly IEventProvider _eventProvider;

        public InLobbyState(StateMachine.StateMachine stateMachine, WindowProvider windowProvider,
            GameConfig gameConfig, INetworkInitializer networkInitializer, IEventProvider eventProvider) : base(stateMachine)
        {
            _windowProvider = windowProvider;
            _gameConfig = gameConfig;
            _networkInitializer = networkInitializer;
            _eventProvider = eventProvider;
        }

        public override void OnEnter()
        {
            _mainMenu = _windowProvider.GetWindow<MainMenu>();
            _mainMenu.HostButtonClicked += HostStart;
            _mainMenu.JoinHostButtonClicked += ClientStart;
        }

        public override void OnExit() =>
            ClearSubscription();

        private void HostStart()
        {
            _networkInitializer.GameLeft += OnGameLeft;
            _networkInitializer.HostGame(_gameConfig.StartLevelScene, OnLoaded);
        }

        private void ClientStart()
        {
            _networkInitializer.GameLeft += OnGameLeft;
            _networkInitializer.JoinGame(_gameConfig.StartLevelScene, OnLoaded);
        }

        private void OnLoaded(bool result)
        {
            if (result)
                ClearSubscription();
            else
                _eventProvider.Invoke(new ConnectionErrorEvent());
        }

        private void ClearSubscription()
        {
            _mainMenu.HostButtonClicked -= HostStart;
            _mainMenu.JoinHostButtonClicked -= ClientStart;
        }

        private void OnGameLeft()
        {
            _networkInitializer.GameLeft -= OnGameLeft;
            
            StateMachine.Enter<EnterLobbyState>();
        }
    }
}