using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CombatSystem))]
    public class PlayerAttack : State
    {
        private float _currentAnimId = 0;

        private CombatSystem _combatSystem;
        private PlayerController _playerController;

        [SerializeField, Range(0, 20)]
        private float _attackSpeed = 12;

        [SerializeField, Range(0, 2)]
        private float _attackDistance = 1;

        private void Start()
        {
            _combatSystem = GetComponent<CombatSystem>();
            _playerController = GetComponent<PlayerController>();
        }

        public async override void Enter() 
        {
            _playerController._canAttack = true;
            await Attack(_playerController._playerAnimator);
        }

        public async UniTask Attack(Animator _playerAnimator)
        {
            _combatSystem.CurrentWeapon.Attack();

            _playerAnimator.CrossFade(_combatSystem.CurrentWeapon._attackClips[(int)_currentAnimId].name, 0);

            float _endTime = Time.time + _attackDistance / _attackSpeed;
            float _canAttackResetTime = Time.time + _combatSystem.CurrentWeapon._reloadTime;

            while (Time.time < _endTime)
            {
                Vector3 _direction = transform.forward;
                _playerController._characterController.Move(_direction.normalized * _attackSpeed * Time.deltaTime);

                await UniTask.Yield();
            }

            await UniTask.Delay(Mathf.CeilToInt((_canAttackResetTime - Time.time) * 1000));

            _playerController._timeToAttack = _combatSystem.CurrentWeapon._reloadTime;

            _playerController._canAttack = false;

            _currentAnimId++;
            _currentAnimId = CheckCurrentAnimId(_currentAnimId);
        }

        private float CheckCurrentAnimId(float _id)
        {
            if (_currentAnimId >= _combatSystem.CurrentWeapon._attackClips.Length)
                return 0;

            if (_currentAnimId < 0)
                return 0;

            return _currentAnimId;
        }


        public override void Exit() { }

        public override void UpdateState() { }
    }
}