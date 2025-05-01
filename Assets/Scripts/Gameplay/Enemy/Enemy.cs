using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("General")]
    public Transform _target;
    public NavMeshAgent _navMeshAgent;
    public HealthSystem _healthSystem;
    public Animator _enemyAnimator;

    public string _name;

    protected virtual void Idle() { }

    protected virtual void Walk() { }
}