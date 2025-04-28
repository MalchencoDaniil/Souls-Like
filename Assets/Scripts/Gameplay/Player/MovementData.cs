using UnityEngine;

public abstract class MovementData : MonoBehaviour
{
    [Header("Main Components")]
    public CharacterController _characterController;
    public Animator _playerAnimator;

    internal Vector3 _movementDirection, _playerVelocity;

    internal virtual bool IsGrounded() { return false; }
}