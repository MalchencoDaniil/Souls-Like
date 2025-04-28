using UnityEngine;
using Zenject;

public class PlayerController : MovementData, IPlayer
{
    internal bool _canRoll = false;

    private StateMachine _stateMachine;
    private CameraMode _cameraMode;
    private TargetDetection _targetDetection;

    private Player.PlayerIdle _idleState;
    private Player.PlayerRun _runState;
    private Player.PlayerRoll _rollState;
    private Player.PlayerWalk _walkState;

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
        _targetDetection = GetComponent<TargetDetection>();
        _cameraMode =GetComponent<CameraMode>();
        _idleState = GetComponent<Player.PlayerIdle>();
        _runState = GetComponent<Player.PlayerRun>();
        _rollState = GetComponent<Player.PlayerRoll>();
        _walkState = GetComponent<Player.PlayerWalk>();

        _stateMachine.Initialize(_idleState);
    }

    private void Update()
    {
        _stateMachine._currentState.UpdateState();

        Quaternion _cameraDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 _input = new Vector3(_playerInput.MovementInput().x, 0, _playerInput.MovementInput().y);
        Vector3 _movementInput = _cameraDirection * _input;

        _movementDirection = _movementInput.normalized;

        StateControl();

        if (!_canRoll)
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
        }

        _playerAnimator.SetFloat(PlayerAnimationNames._speedName, _movementDirection.sqrMagnitude);
        //Debug.Log(_stateMachine._currentState);
    }

    private void StateControl()
    {
        if (_movementDirection != Vector3.zero && !_canRoll)
        {
            Run();

            if (_cameraMode.CanTPS())
                Run();

            if (!_cameraMode.CanTPS())
                Walk();
        }

        if (_playerInput.RollInput() && IsGrounded() && !_canRoll)
        {
            Roll();
        }

        if (_movementDirection == Vector3.zero && !_canRoll)
        {
            Idle();
        }
    }

    internal override bool IsGrounded()
    {
        return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + (_characterController.height / 2) * -1, transform.position.z), _groundDistance, _whatIsGround);
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Idle()
    {
        _stateMachine.ChangeState(_idleState);
    }

    public void Roll()
    {
        _stateMachine.ChangeState(_rollState);
    }

    public void Walk()
    {
        _stateMachine.ChangeState(_walkState);
    }

    public void Run()
    {
        _stateMachine.ChangeState(_runState);;
    }
}