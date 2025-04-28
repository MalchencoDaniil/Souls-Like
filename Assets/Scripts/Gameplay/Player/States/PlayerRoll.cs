using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using Zenject;

namespace Player
{
    public class PlayerRoll : State
    {
        private PlayerController _playerController;
        private CameraMode _cameraMode;

        [SerializeField]
        private Transform _playerBody;

        [SerializeField]
        private float _dashLength = 2;

        [Inject]
        private PlayerInput _playerInput;

        [SerializeField]
        private AnimationCurve _rollSpeedCurve = AnimationCurve.Linear(0, 1, 1, 1);

        private void Start()
        {
            _cameraMode = GetComponent<CameraMode>();
            _playerController = GetComponent<PlayerController>();
        }

        public async override void Enter() 
        {
            Vector3 _rollDirection;
            Transform _baseTransform;

            if (_cameraMode.CanTPS())
            {
                _rollDirection = transform.forward;
                _baseTransform = transform;
            }
            else
            {
                _rollDirection = CalculateRollDirection(_playerController._movementDirection);

                if (_playerInput.MovementInput().y < 0)
                {
                    _playerController._canRoll = false;
                    return;
                }

                _baseTransform = _playerBody.transform;
            }

            await Rolling(_rollDirection, _baseTransform);
        }

        private Vector3 CalculateRollDirection(Vector3 _direction)
        {
            Vector3 _rollDirection;

            if (_direction == Vector3.zero)
            {
                _rollDirection = _cameraMode._focusCamera.transform.forward;
            }
            else
            {
                _rollDirection = Vector3.Lerp(_cameraMode._focusCamera.transform.forward, _direction, 0.5f).normalized;
            }

            return _rollDirection;
        }

        private async UniTask Rolling(Vector3 _direction, Transform _transform)
        {
            _playerController._canRoll = true;

            Vector3 _rollDirection = _direction.normalized;

            if (_rollDirection == Vector3.zero)
                _rollDirection = transform.forward;

            AnimationClip _rollClip = _playerController._playerAnimator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == "Roll");

            _playerController._playerAnimator.SetTrigger(PlayerAnimationNames._rolling);

            float _rollDuration = _rollClip.length;

            float _startTime = Time.time;

            while (Time.time < _startTime + _rollDuration)
            {
                float _timeElapsed = Time.time - _startTime;
                float _normalizedTime = _timeElapsed / _rollDuration;
                float _speedMultiplier = _rollSpeedCurve.Evaluate(_normalizedTime);

                _playerController._characterController.Move(_rollDirection * 3 * _dashLength * _speedMultiplier * Time.deltaTime);
                _transform.forward = Vector3.Slerp(_transform.forward, _rollDirection, Time.deltaTime * 10);

                await UniTask.Yield();
            }

            _transform.forward = transform.forward;

            _playerController._canRoll = false;
        }
    }
}