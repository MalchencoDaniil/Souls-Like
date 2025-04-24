using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        internal CharacterController _characterController;
        private PlayerInput _playerInput;

        [Header("References")]
        [SerializeField]
        private PlayerAnimator _playerAnimator;

        private Vector3 _movementDirection;

        [Header("Movement Stats")]
        [SerializeField]
        private float _movementSpeed = 4;

        [SerializeField]
        private float _rotationSpeed = 15f;

        [Header("Ground Detection")]
        public float _groundDistance = 0.1f;
        public LayerMask _whatIsGround;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public bool IsGrounded()
        {
            return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + (_characterController.height / 2) * -1, transform.position.z), _groundDistance, _whatIsGround);
        }

        private void Update()
        {
            Debug.Log(_playerInput.MovementInput());

            Vector3 _movementInput = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(_playerInput.MovementInput().x, 0, _playerInput.MovementInput().y);
            _movementDirection = _movementInput.normalized;

            if (_movementDirection != Vector3.zero)
            {
                transform.forward = Vector3.Slerp(transform.forward, _movementDirection, Time.deltaTime * _rotationSpeed);
            }

            _playerAnimator.Move(_movementDirection);

            _characterController.Move(_movementDirection * _movementSpeed * Time.deltaTime);
        }
    }
}