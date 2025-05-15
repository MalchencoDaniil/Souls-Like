using UnityEngine;
using Zenject;

public class PlayerController : MovementData, IPlayer
{
    internal bool _canRoll = false, _canAttack = false;

    internal float _timeToAttack = 0;

    private StateMachine _stateMachine;
    private CameraMode _cameraMode;
    private TargetDetection _targetDetection;
    private PlayerSystems _playerSystems;

    private Player.PlayerIdle _idleState;
    private Player.PlayerRun _runState;
    private Player.PlayerRoll _rollState;
    private Player.PlayerWalk _walkState;
    private Player.PlayerAttack _attackState;
    private Player.PlayerSuperAttack _superAttackState;

    private Transform _currentTarget;
    public Transform Target => _currentTarget;

    internal float _currentSpeed;

    [Inject]
    private PlayerInput _playerInput;

    [Header("Ground Detection")]
    private float _groundDistance = 0.1f;
    public LayerMask _whatIsGround;

    private void Awake()
    {
        _stateMachine = new StateMachine();
    }

    private void Start()
    {
        _playerSystems = GetComponent<PlayerSystems>();
        _targetDetection = GetComponent<TargetDetection>();
        _cameraMode =GetComponent<CameraMode>();

        InitializePlayerStates();

        _stateMachine.Initialize(_idleState);
    }

    private void InitializePlayerStates()
    {
        _idleState = GetComponent<Player.PlayerIdle>();
        _superAttackState = GetComponent<Player.PlayerSuperAttack>();
        _runState = GetComponent<Player.PlayerRun>();
        _rollState = GetComponent<Player.PlayerRoll>();
        _walkState = GetComponent<Player.PlayerWalk>();
        _attackState = GetComponent<Player.PlayerAttack>();
    }

    private void Update()
    {
        _stateMachine._currentState.UpdateState();

        Quaternion _cameraDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 _input = new Vector3(_playerInput.MovementInput().x, 0, _playerInput.MovementInput().y);
        Vector3 _movementInput = _cameraDirection * _input;

        _movementDirection = _movementInput.normalized;

        if (!_canRoll && !_canAttack)
        {
            if (_playerInput.FocusInput() && !_cameraMode.CanTPS())
            {
                _cameraMode.SwitchCamera(_playerAnimator);
                return;
            }

            if (_playerInput.FocusInput())
            {
                _currentTarget = _targetDetection.FindClosestTarget();

                if (_currentTarget != null)
                   _cameraMode.SwitchCamera(_playerAnimator);
            }

            StateControl();
        }

        if (!_canAttack && !_canRoll)
            _playerSystems.AddStamina(0.05f);

        _timeToAttack -= Time.deltaTime;

        _playerAnimator.SetFloat(PlayerAnimationNames._speedName, _movementDirection.sqrMagnitude);
    }

    private void StateControl()
    {
        if (_movementDirection != Vector3.zero)
        {
            Run();

            if (_cameraMode.CanTPS())
                Run();

            if (!_cameraMode.CanTPS())
                Walk();
        }

        if (_playerInput.RollInput() && IsGrounded() && _playerSystems.CurrentStamina >= 10)
        {
            _playerSystems.TakeStamina(10);
            Roll();
        }

        if (_movementDirection == Vector3.zero)
        {
            Idle();
        }

        if (_timeToAttack <= 0 && _playerInput.AttackInput() && _playerSystems.CurrentStamina >= 10)
        {
            _playerSystems.TakeStamina(10);

            Attack();
            
            transform.forward = _cameraMode.CanTPS() ? Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * Vector3.forward : transform.forward;
        }

        //if (_playerInput.SupperAttackInput() && _timeToAttack <= 0)
        //    SupperAttack();
    }

    internal override bool IsGrounded()
    {
        return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + (_characterController.height / 2) * -1, transform.position.z), _groundDistance, _whatIsGround);
    }

    public void Attack() => _stateMachine.ChangeState(_attackState);
    private void SupperAttack() => _stateMachine.ChangeState(_superAttackState);
    public void Idle() => _stateMachine.ChangeState(_idleState);
    public void Roll() => _stateMachine.ChangeState(_rollState);
    public void Walk() => _stateMachine.ChangeState(_walkState);
    public void Run() => _stateMachine.ChangeState(_runState);
}