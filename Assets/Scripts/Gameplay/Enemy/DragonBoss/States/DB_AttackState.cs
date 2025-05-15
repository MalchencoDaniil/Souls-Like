using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class DB_AttackState : State
{
    private float _currentAnimId = 0;

    [SerializeField]
    private float _reloadAttackTime = 6f;

    private DragonBoss _enemy;

    [SerializeField]
    private float _rotationSpeed = 15f;

    [SerializeField]
    private float _attackRange = 2;

    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private float _attackDamage = 20;

    [Space(15)]
    [SerializeField]
    private AnimationClip[] _attackClips;

    [SerializeField]
    private Vector3 _attackOffset;

    [SerializeField]
    private List<Transform> _attackPoints;

    private void Awake()
    {
        _enemy = GetComponent<DragonBoss>();
    }

    public async override void Enter()
    {
        _enemy._canAttack = true;
        await Attack(_enemy._enemyAnimator);
    }

    public async UniTask Attack(Animator _enemyAnimator)
    {
        _enemyAnimator.SetTrigger(PlayerAnimationNames._attackName);
        _enemyAnimator.SetFloat(PlayerAnimationNames._attackIDName, _currentAnimId);

        float _waitingTime = Time.time + 2;

        while (Time.time < _waitingTime)
        {
            Vector3 _direction = _enemy._target.position + _attackOffset - transform.position;
            _direction.y = 0;

            Quaternion _targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

            await UniTask.Yield();
        }

        _enemy._timeToAttack = _reloadAttackTime;

        _enemy._canAttack = false;

        _currentAnimId++;
        _currentAnimId = CheckCurrentAnimId(_currentAnimId);
    }

    private float CheckCurrentAnimId(float _id)
    {
        if (_currentAnimId >= _attackClips.Length)
            return 0;

        if (_currentAnimId < 0)
            return 0;

        return _currentAnimId;
    }

    public override void Exit() { }

    public override void UpdateState() { }

    public void CheckHit()
    {
        Debug.Log("CheckHit");

        foreach (Transform attackPoint in _attackPoints)
        {
            Collider[] _hitEnemys = Physics.OverlapSphere(attackPoint.position, _attackRange, _playerLayer);

            foreach (Collider _enemyCollider in _hitEnemys)
            {
                HealthSystem _player = _enemyCollider.gameObject.GetComponent<HealthSystem>();

                if (_player != null)
                {
                    _player.TakeDamage(_attackDamage);
                }
            }
        }
    }
}