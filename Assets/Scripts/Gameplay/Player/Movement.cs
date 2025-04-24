using Cinemachine;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        private float _currentSpeed;

        internal CharacterController _characterController;
        private PlayerInput _playerInput;

        [Header("References")]
        [SerializeField]
        private PlayerAnimator _playerAnimator;

        [SerializeField]
        private Transform _target;

        private Vector3 _movementDirection, _currentVelocity;

        [Header("Movement Stats")]
        [SerializeField, Range(5, 8)]
        private float _movementSpeed = 4;

        [SerializeField]
        private float _rotationSpeed = 15f;

        [Header("Ground Detection")]
        public float _groundDistance = 0.1f;
        public LayerMask _whatIsGround;

        private float _tpsCameraAngle;
        private float _focusCameraAngle;

        [Header("Movement Type")]
        [SerializeField] 
        private CinemachineFreeLook _tpsCamera;

        [SerializeField] 
        private CinemachineVirtualCamera _focusCamera;

        private bool _canTPS = true;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

            _currentSpeed = _movementSpeed;

            _tpsCameraAngle = _tpsCamera.transform.eulerAngles.x;
            _focusCameraAngle = _focusCamera.transform.eulerAngles.x;

            _tpsCamera.gameObject.SetActive(_canTPS);
            _focusCamera.gameObject.SetActive(!_canTPS);
        }

        public bool IsGrounded()
        {
            return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + (_characterController.height / 2) * -1, transform.position.z), _groundDistance, _whatIsGround);
        }

        private void Update()
        {
            Vector3 _movementInput = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(_playerInput.MovementInput().x, 0, _playerInput.MovementInput().y);
            _movementDirection = _movementInput.normalized;

            if (_playerInput.SwitchMoveType()) SwitchCamera();

            MovementType();

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _playerInput.MovementInput().x * _movementSpeed, 8.9f * Time.deltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _playerInput.MovementInput().y * _movementSpeed, 8.9f * Time.deltaTime);

            _playerAnimator.Move(_movementDirection);

            _characterController.Move(_movementDirection * _movementSpeed * Time.deltaTime);
        }

        private void MovementType()
        {
            PlayerRotate();

            _movementSpeed = _canTPS ? _currentSpeed : _currentSpeed / 2;
        }

        private void PlayerRotate()
        {
            if (_movementDirection != Vector3.zero && _canTPS)
            {
                transform.forward = Vector3.Slerp(transform.forward, _movementDirection, Time.deltaTime * _rotationSpeed);
            }

            if (!_canTPS)
            {
                _playerAnimator.VelocityWalkBlend(_currentVelocity);

                Vector3 _direction = _target.position - transform.position;
                _direction.y = 0;
                Quaternion _rotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Euler(0, _rotation.eulerAngles.y, 0);
            }
        }

        private void SwitchCamera()
        {
            _canTPS = !_canTPS;

            _tpsCamera.gameObject.SetActive(_canTPS);
            _focusCamera.gameObject.SetActive(!_canTPS);

            if (_canTPS) _playerAnimator.ExitFocusAnim();

            if (!_canTPS)
            {
                _playerAnimator.EnterFocusAnim();

                _tpsCamera.Follow = _focusCamera.Follow;
                _tpsCamera.LookAt = _focusCamera.LookAt;
            }
        }
    }
}