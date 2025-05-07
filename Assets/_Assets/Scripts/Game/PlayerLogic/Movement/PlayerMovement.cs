using System;
using _Assets.Scripts.Game.CharacterBaseLogic.Movement;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic.Movement
{
    public class PlayerMovement : NetworkBehaviour, IGroundLander
    {
        public event Action<float> Landed;
        
        [SerializeField] private NetworkCharacterController characterController;
        
        private float _moveSpeed;
        private float _jumpHeight;
        
        private Vector3 _velocity;
        private bool _wasGroundedLastFrame;
        private bool _isGrounded;
        private float _highestYWhileFalling;

        public void Initialize(float moveSpeed, float jumpHeight)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
            
            _velocity = Vector3.zero;
            _isGrounded = characterController.Grounded;
            _wasGroundedLastFrame = _isGrounded;
            _highestYWhileFalling = transform.position.y;
        }

        public override void FixedUpdateNetwork()
        {
            UpdateGroundState();
            ApplyGravity();
            
            if (Object.HasInputAuthority && GetInput<NetworkInputData>(out var inputData))
            {
                ProcessMovementInput(inputData.MoveInput);
                ProcessJumpInput(inputData.JumpInput);
            }

            characterController.Move(new Vector3(_velocity.x * _moveSpeed, _velocity.y, _velocity.z * _moveSpeed) * Runner.DeltaTime);
            LogDebugInfo();
        }

        public void Warp(Vector3 getRandomMapPosition) =>
            characterController.transform.position = getRandomMapPosition;

        private void UpdateGroundState()
        {
            _wasGroundedLastFrame = _isGrounded;
            _isGrounded = characterController.Grounded;
            
            if (!_wasGroundedLastFrame && _isGrounded)
            {
                float fallHeight = _highestYWhileFalling - transform.position.y;
                if (fallHeight > 0.1f)
                    Landed?.Invoke(fallHeight);
                
                _velocity.y = 0f;
            }
            
            if (!_isGrounded)
            {
                if (_velocity.y <= 0) 
                    _highestYWhileFalling = Mathf.Max(_highestYWhileFalling, transform.position.y);
            }
            else
            {
                _highestYWhileFalling = transform.position.y;
                
                if (_velocity.y is < 0 or > 0)
                    _velocity.y = 0f;
            }
        }

        private void ApplyGravity()
        {
            if (!_isGrounded)
                _velocity.y += characterController.gravity * Runner.DeltaTime;
            
            _velocity.y = Mathf.Max(_velocity.y, characterController.gravity);
        }

        private void ProcessMovementInput(Vector2 inputData)
        {
            Vector3 inputDirection = transform.right * inputData.x + transform.forward * inputData.y;

            if (inputDirection.sqrMagnitude > 1f)
                inputDirection.Normalize();

            //Vector3 movement = inputDirection * _moveSpeed * Runner.DeltaTime;
            _velocity.x = inputDirection.x;
            _velocity.z = inputDirection.z;
        }

        private void ProcessJumpInput(bool jumpInput)
        {
            if (jumpInput && _isGrounded)
                Jump();
        }

        private void Jump() =>
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * characterController.gravity);

        private void LogDebugInfo() =>
            Debug.Log($"Velocity: {_velocity}, IsGrounded: {_isGrounded}");
    }
}