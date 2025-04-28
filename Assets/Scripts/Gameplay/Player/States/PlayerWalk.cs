using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerWalk : State
    {
        private int _xVelHash, _yVelHash;

        private Vector3 _currentVelocity;

        private PlayerController _playerController;

        [Inject]
        private PlayerInput _playerInput;

        [SerializeField]
        private float _walkSpeed;

        [SerializeField]
        private float _rotationSpeed = 5;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();

            _xVelHash = Animator.StringToHash(PlayerAnimationNames._xVelocityName);
            _yVelHash = Animator.StringToHash(PlayerAnimationNames._yVelocityName);
        }

        public override void Enter() 
        {
            _playerController._playerAnimator.SetBool(PlayerAnimationNames._focusName, true);
            _playerController._currentSpeed = _walkSpeed;
        }

        public override void Exit() 
        {
            _playerController._playerAnimator.SetBool(PlayerAnimationNames._focusName, false);
        }

        public override void UpdateState() 
        {
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _playerInput.MovementInput().x * _walkSpeed, 8.9f * Time.deltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _playerInput.MovementInput().y * _walkSpeed, 8.9f * Time.deltaTime);

            _playerController._playerAnimator.SetFloat(_xVelHash, _currentVelocity.x);
            _playerController._playerAnimator.SetFloat(_yVelHash, _currentVelocity.y);

            Vector3 _direction = _playerController.Target.position - transform.position;
            _direction.y = 0;
            Quaternion _rotation = Quaternion.LookRotation(_direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, _rotation.eulerAngles.y, 0), _rotationSpeed * Time.deltaTime);
            _playerController._characterController.Move(_playerController._movementDirection * _playerController._currentSpeed * Time.deltaTime);
        }
    }
}