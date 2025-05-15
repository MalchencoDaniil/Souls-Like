using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DragonBoss : Enemy
{
    internal bool _canAttack = false, _canFly = false;
    internal float _timeToAttack = 0, _timeToFly = 0;

    public UnityEvent OnFlyAttack;

    private StateMachine _stateMachine;


    [SerializeField]
    public float _timeWaitFly = 10;

    [Header("UI Settings")]
    [SerializeField]
    private Text _bossName;

    [SerializeField]
    private Transform _bossHealthBar;

    private DB_IdleState _idleState;
    private DB_AttackState _attackState;
    private DB_FlyState _flyState;
    private DB_FlyAttackState _flyAttackState;
    
    public void BossInitialize()
    {
        gameObject.SetActive(true);
        _bossName.text = _name;
        _bossHealthBar.gameObject.SetActive(true);
    }

    private void Awake()
    {
        _stateMachine = new StateMachine();

        _flyAttackState = GetComponent<DB_FlyAttackState>();
        _idleState = GetComponent<DB_IdleState>();
        _attackState = GetComponent<DB_AttackState>();
        _flyState = GetComponent<DB_FlyState>();

        OnFlyAttack.AddListener(FlyAttack);

        _timeToFly = _timeWaitFly;
        _timeToAttack = 4;

        _stateMachine.Initialize(_idleState);
    }

    private void Update()
    {
        _timeToAttack -= Time.deltaTime;
        _timeToFly -= Time.deltaTime;

        float _distanceToTarget = Vector3.Distance(transform.position, _target.position);

        if (_distanceToTarget <= 10)
        {
            if (_timeToAttack <= 0 && !_canFly && !_canAttack)
            {
                _stateMachine.ChangeState(_attackState);
            }

            if (!_canAttack && !_canFly && _timeToAttack > 0 && _timeToFly > 0)
            {
                _stateMachine.ChangeState(_idleState);
            }
        }
        
        if (_timeToAttack <= 0 && _distanceToTarget > 10 && !_canAttack && !_canFly)
        {
            _stateMachine.ChangeState(_flyAttackState);
        }

        if (_timeToFly <= 0 && !_canFly && !_canAttack)
        {
            Debug.Log("Fly");
            Fly();
        }

        _stateMachine._currentState.UpdateState();
    }

    public void FlyAttack()
    {
        _stateMachine.ChangeState(_flyAttackState);
    }

    public void Fly()
    {
        _stateMachine.ChangeState(_flyState);
    }
}