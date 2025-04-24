using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator _playerAnimator;

        [Header("Paraneters Names")]
        [SerializeField]
        private string _speedName = "Speed";

        internal void Move(Vector3 _movementDirection)
        {
            _playerAnimator.SetFloat(_speedName, _movementDirection.sqrMagnitude);
        }
    }
}