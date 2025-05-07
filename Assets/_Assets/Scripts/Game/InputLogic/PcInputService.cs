using System;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Core.Infrastructure.Mono;
using UnityEngine;

namespace _Assets.Scripts.Game.InputLogic
{
    public class PcInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string MouseHorizontal = "Mouse X";
        private const string MouseVertical = "Mouse Y";

        private readonly MonoService _monoService;
        private readonly InputConfig _inputConfig;

        public event Action<Vector2> MoveInput;
        public event Action<Vector2> LookInput;
        public event Action<bool> JumpInput;
        
        public PcInputService(MonoService monoService, GameConfig gameConfig)
        {
            _monoService = monoService;
            _inputConfig = gameConfig.InputConfig;
            _monoService.OnTick += Update;
        }

        public void Dispose() =>
            _monoService.OnTick -= Update;

        private void Update()
        {
            UpdateMovement();
            UpdateLook();
            UpdateKeys();
        }

        private void UpdateMovement() =>
            MoveInput?.Invoke(new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)));
        
        private void UpdateLook() =>
            LookInput?.Invoke(new Vector2(Input.GetAxis(MouseHorizontal), Input.GetAxis(MouseVertical)));

        private void UpdateKeys() =>
            JumpInput?.Invoke(Input.GetKeyDown(_inputConfig.JumpKey));
    }
}