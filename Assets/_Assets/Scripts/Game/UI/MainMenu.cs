using System;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Core.Infrastructure.WindowManagement;
using _Assets.Scripts.Game.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Assets.Scripts.Game.UI
{
    public class MainMenu : BaseWindow
    {
        public event Action HostButtonClicked;
        public event Action JoinHostButtonClicked;
        
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinHostButton;
        [SerializeField] private TMP_Text errorText;
        
        private IEventProvider _eventProvider;
        private Sequence _errorAnimateSequence;

        [Inject]
        private void Construct(IEventProvider eventProvider) =>
            _eventProvider = eventProvider;

        protected override void Start()
        {
            base.Start();

            EnableInput(true);
            _eventProvider.Subscribe<ConnectionErrorEvent>(ShowConnectionError);
            hostButton.onClick.AddListener(HostButtonClick);
            joinHostButton.onClick.AddListener(JoinHostButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _eventProvider.UnSubscribe<ConnectionErrorEvent>(ShowConnectionError);
            hostButton.onClick.RemoveListener(HostButtonClick);
            joinHostButton.onClick.RemoveListener(JoinHostButtonClick);
        }

        private void ShowConnectionError(ConnectionErrorEvent connectionErrorEvent)
        {
            EnableInput(true);
            if (_errorAnimateSequence == null)
            {
                _errorAnimateSequence = DOTween.Sequence();
                _errorAnimateSequence.Append(errorText.DOFade(1f, 1f))
                    .AppendInterval(2f)
                    .Append(errorText.DOFade(0f, 1f));
            }
            
            _errorAnimateSequence.Kill();
            _errorAnimateSequence.Play();
        }

        private void HostButtonClick()
        {
            EnableInput(false);
            HostButtonClicked?.Invoke();
        }

        private void JoinHostButtonClick()
        {
            EnableInput(false);
            JoinHostButtonClicked?.Invoke();
        }

        private void EnableInput(bool isEnabled)
        {
            hostButton.enabled = isEnabled;
            joinHostButton.enabled = isEnabled;
        }
    }
}