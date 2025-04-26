using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        private bool _canRoll = false;

        private float _currentSpeed;

        internal CharacterController _characterController;
        private PlayerInput _playerInput;

        [Header("References")]
        [SerializeField]
        private PlayerAnimator _playerAnimator;

        [SerializeField]
        private Transform _playerBody;

        [SerializeField]
        private Transform _target;

        private Vector3 _movementDirection, _currentVelocity;

        [Header("Movement Stats")]
        [SerializeField, Range(5, 8)]
        private float _movementSpeed = 4;

        [SerializeField, Range(1, 5)]
        private float _walkSpeed = 2;

        [SerializeField]
        private float _dashLength = 2;

        [SerializeField]
        private AnimationCurve _rollSpeedCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [SerializeField]
        private float _rotationSpeed = 15f;

        [SerializeField]
        private float _focusRotationSpeed = 3;

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

            if (!_canRoll)
            {
                if (_playerInput.SwitchMoveType()) SwitchCamera();
                MovementType();

                _characterController.Move(_movementDirection * _movementSpeed * Time.deltaTime);
            }

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _playerInput.MovementInput().x * _movementSpeed, 8.9f * Time.deltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _playerInput.MovementInput().y * _movementSpeed, 8.9f * Time.deltaTime);

            _playerAnimator.Move(_movementDirection);

            StartRoll();
        }

        private async void StartRoll()
        {
            if (_playerInput.RollInputEvent() && IsGrounded() && !_canRoll)
            {
                _canRoll = false;

                Vector3 _rollDirection;
                Transform _baseTransform;

                if (_canTPS)
                {
                    _rollDirection = transform.forward;
                    _baseTransform = transform;
                }
                else
                {
                    _rollDirection = CalculateRollDirection(_movementDirection);
                    _baseTransform = _playerBody.transform;
                }

                await Rolling(_rollDirection, _baseTransform);
            }
        }


        private Vector3 CalculateRollDirection(Vector3 _direction)
        {
            Vector3 _rollDirection;

            if (_direction == Vector3.zero)
            {
                _rollDirection = _focusCamera.transform.forward;
            }
            else
            {
                _rollDirection = Vector3.Lerp(_focusCamera.transform.forward, _direction, 0.5f).normalized;
            }

            return _rollDirection;
        }


        private void MovementType()
        {
            PlayerRotate();

            _movementSpeed = _canTPS ? _currentSpeed : _walkSpeed;
        }

        private async UniTask Rolling(Vector3 _direction, Transform _transform)
        {
            _canRoll = true;

            Vector3 _rollDirection = _direction.normalized;

            if (_rollDirection == Vector3.zero)
                _rollDirection = transform.forward;

            AnimationClip _rollClip = _playerAnimator._playerAnimator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == "Roll");

            _playerAnimator.Roll();

            float _rollDuration = _rollClip.length;

            float _startTime = Time.time;

            while (Time.time < _startTime + _rollDuration)
            {
                float _timeElapsed = Time.time - _startTime;
                float _normalizedTime = _timeElapsed / _rollDuration;
                float _speedMultiplier = _rollSpeedCurve.Evaluate(_normalizedTime);

                _characterController.Move(_rollDirection * _walkSpeed * _dashLength * _speedMultiplier * Time.deltaTime);
                _transform.forward = Vector3.Slerp(_transform.forward, _rollDirection, Time.deltaTime * _rotationSpeed);

                await UniTask.Yield();
            }

            _transform.forward = transform.forward;

            _canRoll = false;
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

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, _rotation.eulerAngles.y, 0), _focusRotationSpeed * Time.deltaTime) ;
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