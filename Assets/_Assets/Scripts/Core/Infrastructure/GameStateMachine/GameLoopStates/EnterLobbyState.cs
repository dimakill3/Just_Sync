using _Assets.Scripts.Core.Infrastructure.SceneManagement;
using _Assets.Scripts.Core.Infrastructure.StateMachine;
using _Assets.Scripts.Core.Infrastructure.WindowManagement;
using _Assets.Scripts.Core.UI;

namespace _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates
{
    public class EnterLobbyState : GameLoopState
    {
        private const string LobbySceneName = "Lobby";

        private readonly LoadingScreen _loadingScreen;
        private readonly ISceneLoader _sceneLoader;

        public EnterLobbyState(StateMachine.StateMachine stateMachine, WindowProvider windowProvider,
            ISceneLoader sceneLoader) : base(stateMachine)
        {
            _loadingScreen = windowProvider.GetWindow<LoadingScreen>();
            _sceneLoader = sceneLoader;
        }

        public override void OnEnter()
        {
            _loadingScreen.Show();
            _sceneLoader.Load(LobbySceneName, OnLoaded, true);
        }

        public override void OnExit()
        {
        }

        private void OnLoaded()
        {
            StateMachine.Enter<InLobbyState>();
            _loadingScreen.Hide();
        }
    }
}