using UnityEngine;

public class DB_Fireball : MonoBehaviour
{
    private Vector3 _movementDirection;
    private float _speed;
    private float _damage;

    private Rigidbody _rb;

    public void Initialize(Vector3 direction, float speed, float damage)
    {
        this._speed = speed;
        this._damage = damage;
        _movementDirection = direction.normalized;

        _rb = GetComponent<Rigidbody>();

        _rb.velocity = _movementDirection * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            HealthSystem health = other.GetComponent<HealthSystem>();

            if (health != null)
            {
                health.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}