using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(CombatSystem))]
    public class PlayerSuperAttack : State
    {
        private float _currentAnimId = 0;

        private PlayerController _playerController;
        private CombatSystem _combatSystem;

        [Inject]
        private PlayerInput _playerInput;

        private void Start()
        {
            _combatSystem = GetComponent<CombatSystem>();
            _playerController = GetComponent<PlayerController>();
        }

        public override void Enter() 
        {
            _playerController._canAttack = true;
            _playerController._playerAnimator.SetTrigger(PlayerAnimationNames._superAttackName);
            //Vector3 _direction = _cameraMode.CanTPS() ? _movementDirection : transform.forward;
            //transform.forward = _cameraMode.CanTPS() ? Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * Vector3.forward : _direction.normalized;
        }

        public override void Exit() { }

        public override async void UpdateState() 
        {
            if (!_playerInput.SupperAttackInput())
            {
                await SupperAtack();
            }
        }

        private async UniTask SupperAtack()
        {
            _playerController._playerAnimator.SetTrigger(PlayerAnimationNames._attackName);

            await UniTask.WaitForSeconds(0.5f);

            _playerController._timeToAttack = _combatSystem.CurrentWeapon._reloadTime;

            _playerController._canAttack = false;
        }
    }
}