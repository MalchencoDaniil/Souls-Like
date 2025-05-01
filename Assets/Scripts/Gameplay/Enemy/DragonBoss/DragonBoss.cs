using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DragonBoss : Enemy
{
    internal bool _canAttack = false;
    internal float _timeToAttack = 0;

    private StateMachine _stateMachine;

    [Header("UI Settings")]
    [SerializeField]
    private Text _bossName;

    [SerializeField]
    private Transform _bossHealthBar;

    private DB_IdleState _idleState;
    private DB_AttackState _attackState;

    public void BossInitialize()
    {
        gameObject.SetActive(true);
        _bossName.text = _name;
        _bossHealthBar.gameObject.SetActive(true);
    }

    private void Awake()
    {
        _stateMachine = new StateMachine();
    }

    private void Start()
    {
        _idleState = GetComponent<DB_IdleState>();
        _attackState = GetComponent<DB_AttackState>();

        _timeToAttack = 10;

        _stateMachine.Initialize(_idleState);
    }

    private void Update()
    {
        _timeToAttack -= Time.deltaTime;

        if (_timeToAttack <= 0 && !_canAttack)
            _stateMachine.ChangeState(_attackState);
        else if (!_canAttack && _timeToAttack > 0)
        {
            _stateMachine.ChangeState(_idleState);
        }


        _stateMachine._currentState.UpdateState();
    }
}