using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        internal int _xVelHash, _yVelHash;

        [SerializeField]
        internal Animator _playerAnimator;

        [Header("Paraneters Names")]
        [SerializeField]
        private string _speedName = "Speed";

        [SerializeField]
        private string _xVelocityName = "X_Velocity";

        [SerializeField]
        private string _yVelocityName = "Y_Velocity";

        [SerializeField]
        private string _focusName = "CanFocus";

        private void Start()
        {
            _xVelHash = Animator.StringToHash(_xVelocityName);
            _yVelHash = Animator.StringToHash(_yVelocityName);
        }

        internal void Move(Vector3 _movementDirection)
        {
            _playerAnimator.SetFloat(_speedName, _movementDirection.sqrMagnitude);
        }

        internal void EnterFocusAnim()
        {
            Debug.Log("EnterFocusAnim");
            _playerAnimator.SetBool(_focusName, true);
        }

        internal void ExitFocusAnim()
        {
            Debug.Log("ExitFocusAnim");
            _playerAnimator.SetBool(_focusName, false);
        }

        internal void VelocityWalkBlend(Vector3 _currentVelocity)
        {
            _playerAnimator.SetFloat(_xVelHash, _currentVelocity.x);
            _playerAnimator.SetFloat(_yVelHash, _currentVelocity.y);
        }
    }
}