using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerRun : State
    {
        private PlayerController _playerController;

        [SerializeField]
        private float _runSpeed = 6;

        [SerializeField]
        private float _rotationSpeed = 8;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
        }

        public override void Enter()
        {
            _playerController._currentSpeed = _runSpeed;
        }

        public override void UpdateState() 
        {
            transform.forward = Vector3.Slerp(transform.forward, _playerController._movementDirection, Time.deltaTime * _rotationSpeed);

            _playerController._characterController.Move(_playerController._movementDirection * _playerController._currentSpeed * Time.deltaTime);
        }
    }
}