using Cysharp.Threading.Tasks;
using UnityEngine;

public class DB_FlyState : State
{
    private enum FlyState
    {
        Fly,
        Landing
    }

    private DragonBoss _enemy;

    private FlyState _flyState = FlyState.Landing;

    [SerializeField]
    private Vector3 _startFlyOffset = new Vector3(0, 2, 0);

    [SerializeField]
    private Transform[] _flyTargets;

    [SerializeField]
    private Transform[] _landingPoints;

    [SerializeField]
    private float _flySpeed = 6;

    private void Awake()
    {
        _enemy = GetComponent<DragonBoss>();
    }

    public async override void Enter()
    {
        _enemy._canFly = true;

        _flyState = SwitchFlyState();

        await Fly();
    }

    private FlyState SwitchFlyState()
    {
        if (_flyState == FlyState.Landing)
            return FlyState.Fly;
        
        return FlyState.Landing;
    }

    public async UniTask Fly()
    {
        if (_flyState == FlyState.Fly)
        {
            _enemy._enemyAnimator.SetTrigger("Fly");

            Vector3 _startFlyPosition = transform.position + _startFlyOffset;

            float _startTime = Time.time + 1.5f;

            await UniTask.WaitForSeconds(0.5f);

            while (Time.time < _startTime)
            {
                Debug.Log("Start Fly");
                transform.position = Vector3.Slerp(transform.position, _startFlyPosition, 3 * Time.deltaTime);
                await UniTask.Yield();
            }

            int _randomTargetId = Random.Range(0, _flyTargets.Length);
            Transform _currentTarget = _flyTargets[_randomTargetId];

            float _distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position);

            while (_distanceToTarget > 1f)
            {
                Vector3 _direction = _enemy._target.position - transform.position;
                _direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _flySpeed * Time.deltaTime);

                _distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position);
                await UniTask.Yield();
            }

            _enemy.OnFlyAttack.Invoke();
        }
        else
        {
            int _randomTargetId = Random.Range(0, _landingPoints.Length);
            Transform _currentTarget = _landingPoints[_randomTargetId];

            float _distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position + _startFlyOffset);

            while (_distanceToTarget > 2f)
            {
                Vector3 _direction = _currentTarget.position - transform.position;
                _direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _flySpeed * Time.deltaTime);

                _distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position + _startFlyOffset);
                await UniTask.Yield();
            }

            _enemy._enemyAnimator.SetTrigger("FlyEnd");

            float _startTime = Time.time + 1.5f;

            while (Time.time < _startTime)
            {
                transform.position = Vector3.Slerp(transform.position, _currentTarget.position, 3 * Time.deltaTime);
                await UniTask.Yield();
            }

            _enemy._timeToFly = _enemy._timeWaitFly;
            _enemy._canFly = false;
        }
    }
}