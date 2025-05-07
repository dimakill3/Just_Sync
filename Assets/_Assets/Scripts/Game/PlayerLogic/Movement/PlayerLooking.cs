using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic.Movement
{
    public class PlayerLooking : NetworkBehaviour
    {
        [SerializeField] private Transform cameraAnchor;
        [SerializeField] private NetworkCharacterController characterController;
        
        private float _xRotation;
        private float _mouseSensitivity;
        private Transform _cameraTransform;
        
        public void Initialize(float mouseSensitivity, Transform cameraTransform)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _mouseSensitivity = mouseSensitivity;
            _cameraTransform = cameraTransform;
            
            if (_cameraTransform != null && cameraAnchor != null && Object.InputAuthority == Runner.LocalPlayer)
            {
                _cameraTransform.SetParent(cameraAnchor, false);
                _cameraTransform.localPosition = Vector3.zero;
                _cameraTransform.localRotation = Quaternion.identity;
            }
        }

        public override void FixedUpdateNetwork()
        {
            if(!Object.HasInputAuthority)
                return;
            
            if (!Object.HasInputAuthority || !GetInput<NetworkInputData>(out var inputData))
                return;

            Look(inputData.LookInput);
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            
            if (_cameraTransform != null && _cameraTransform.parent != null)
                _cameraTransform.SetParent(null);
        }

        private void Look(Vector2 input)
        {
            float mouseX = input.x * _mouseSensitivity;
            float mouseY = input.y * _mouseSensitivity;
            
            characterController.transform.Rotate(Vector3.up * mouseX);
            
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -85f, 85f);
            cameraAnchor.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
}