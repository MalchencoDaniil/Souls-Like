using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    private Vector3 _playerVelocity;

    private PlayerController _movementData;

    private void Awake()
    {
        _movementData = GetComponent<PlayerController>();
    }

    private void Update()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (_movementData.IsGrounded() && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -1;
        }
        else
        {
            _playerVelocity.y += World._instance.GRAVITY_FORCE * Time.deltaTime;
        }

        _movementData._characterController.Move(_playerVelocity * Time.deltaTime);
    }
}