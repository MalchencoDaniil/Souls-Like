using UnityEngine;

namespace Gameplay.Player
{
    public class Movement : MonoBehaviour
    {
        private PlayerInput _playerInput;

        [Header("References")]
        [SerializeField]
        private PlayerAnimator _playerAnimator;

        private Vector3 _movementDirection;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void Update()
        {
            Debug.Log(_playerInput.MovementInput());

            _movementDirection = _playerInput.MovementInput().normalized;

            _playerAnimator.Move(_movementDirection);
        }
    }
}