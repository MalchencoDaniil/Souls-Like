using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerGravity : MonoBehaviour
    {
        private Vector3 _playerVelocity;

        private Movement _movement;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
        }

        private void Update()
        {
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            if (_movement.IsGrounded() && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -1;
            }
            else
            {
                _playerVelocity.y += World._instance.GRAVITY_FORCE * Time.deltaTime;
            }

            _movement._characterController.Move(_playerVelocity * Time.deltaTime);
        }
    }
}