using System;
using _Assets.Scripts.Game.CharacterBaseLogic.Movement;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic.Movement
{
    public class PlayerMovement : NetworkBehaviour, IGroundLander
    {
        [SerializeField] private LayerMask groundMask;
        public event Action<float> Landed;
        
        [SerializeField] private NetworkCharacterController characterController;
        
        private float _moveSpeed;
        private float _jumpHeight;
        
        private Vector3 _velocity;
        private bool _wasGroundedLastFrame;
        private bool _isGrounded;
        private float _highestYWhileFalling;
        private bool _isInitialized = false;
        private float _mouseSensitivity;

        public void Initialize(float moveSpeed, float jumpHeight, float mouseSensitivity)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
            _mouseSensitivity = mouseSensitivity;

            Warp(transform.position);
            characterController.enabled = true;
            _velocity = Vector3.zero;
            _isGrounded = characterController.Grounded;
            _wasGroundedLastFrame = _isGrounded;
            _highestYWhileFalling = transform.position.y;
            
            _isInitialized = true;
        }

        public override void FixedUpdateNetwork()
        {
            if (!_isInitialized)
                return;
            
            //UpdateGroundState();
            //ApplyGravity();
            
            if (Object.HasStateAuthority && GetInput<NetworkInputData>(out var inputData))
            {
                ProcessRotation(inputData.YawInput);
                ProcessMovementInput(inputData.MoveInput);
                ProcessJumpInput(inputData.JumpInput);
            }

            characterController.Move(new Vector3(_velocity.x * _moveSpeed, _velocity.y, _velocity.z * _moveSpeed) * Runner.DeltaTime);
        }

        public void Warp(Vector3 getRandomMapPosition) =>
            characterController.Teleport(getRandomMapPosition);

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

        private void ProcessRotation(float inputDataYawInput) =>
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + inputDataYawInput * _mouseSensitivity, 0f);

        private void ProcessMovementInput(Vector2 inputData)
        {
            var inputDirection = transform.right * inputData.x + transform.forward * inputData.y;

            if (inputDirection.sqrMagnitude > 1f)
                inputDirection.Normalize();
            
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
    }
}