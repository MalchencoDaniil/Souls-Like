using UnityEngine;

public class DB_IdleState : State
{
    private DragonBoss _enemy;

    [SerializeField]
    private float _rotationSpeed = 5f;

    private void Awake()
    {
        _enemy = GetComponent<DragonBoss>();
    }

    public override void UpdateState()
    {
        Vector3 _direction = _enemy._target.position - transform.position;
        _direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}