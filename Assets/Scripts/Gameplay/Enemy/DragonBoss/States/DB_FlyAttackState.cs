using UnityEngine;

public class DB_FlyAttackState : State
{
    private DragonBoss _enemy;

    private int _currentShootCount;
    private float _timeToBulletShots;

    [Header("Shoot Settings")]
    [SerializeField]
    private int _shootCount = 3;

    [SerializeField]
    private float _shootReloadTime = 2;

    [SerializeField]
    private Transform _shootPosition;

    [Header("Fireball Settings")]
    [SerializeField]
    private DB_Fireball _fireball;

    [SerializeField]
    private float _fireballSpeed = 12;

    [SerializeField]
    private float _fireballDamage = 12;

    private Vector3[] _previousVelocities = new Vector3[5];
    private int _velocityIndex = 0;

    private void Awake()
    {
        _enemy = GetComponent<DragonBoss>();
    }

    public override void Enter()
    {
        _currentShootCount = _shootCount;
    }

    public override void UpdateState()
    {
        _timeToBulletShots -= Time.deltaTime;

        Vector3 _direction = _enemy._target.position - transform.position;
        _direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);

        _previousVelocities[_velocityIndex] = _enemy._target.GetComponent<CharacterController>().velocity;
        _velocityIndex = (_velocityIndex + 1) % _previousVelocities.Length;

        if (_timeToBulletShots <= 0)
        {
            _timeToBulletShots = _shootReloadTime;

            Vector3 averageVelocity = Vector3.zero;
            foreach (var vel in _previousVelocities)
            {
                averageVelocity += vel;
            }
            averageVelocity /= _previousVelocities.Length;

            float projectileFlightTime = Vector3.Distance(_shootPosition.position, _enemy._target.position) / _fireballSpeed;
            Vector3 _predictedTargetPosition = _enemy._target.position + averageVelocity * projectileFlightTime;

            Vector3 _directionToTarget = _predictedTargetPosition - _shootPosition.position;

            Quaternion _fireballRotation = Quaternion.LookRotation(_directionToTarget);

            DB_Fireball _fireballSpawned = Instantiate(_fireball, _shootPosition.position, _fireballRotation);
            _fireballSpawned.Initialize(_directionToTarget, _fireballSpeed, _fireballDamage);

            _currentShootCount--;

            if (!_enemy._canFly)
                _enemy._enemyAnimator.SetTrigger("Shoot");
        }

        if (_currentShootCount == 0 && _enemy._canFly)
            _enemy.Fly();
    }
}