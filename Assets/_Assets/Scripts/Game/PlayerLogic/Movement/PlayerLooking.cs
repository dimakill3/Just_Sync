using _Assets.Scripts.Game.InputLogic;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic.Movement
{
    public class PlayerLooking : MonoBehaviour
    {
        [SerializeField] private Transform cameraAnchor;

        private float _xRotation;
        private float _mouseSensitivity;
        private Transform _cameraTransform;
        private IInputService _inputService;

        public void Initialize(float mouseSensitivity, Transform cameraTransform, IInputService inputService)
        {
            _inputService = inputService;
            Cursor.lockState = CursorLockMode.Locked;
            _mouseSensitivity = mouseSensitivity;
            _cameraTransform = cameraTransform;

            if (_cameraTransform != null && cameraAnchor != null)
            {
                _cameraTransform.SetParent(cameraAnchor, false);
                _cameraTransform.localPosition = Vector3.zero;
                _cameraTransform.localRotation = Quaternion.identity;
            }

            _inputService.LookInput += LocalLook;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;

            if (_cameraTransform != null && _cameraTransform.parent != null)
                _cameraTransform.SetParent(null);

            _inputService.LookInput -= LocalLook;
        }

        private void LocalLook(Vector2 input)
        {
            var mouseY = input.y * _mouseSensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -85f, 85f);
            cameraAnchor.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
}