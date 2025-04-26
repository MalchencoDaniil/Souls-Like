using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    public Transform _target;
    public NavMeshAgent _navMeshAgent;
    public Animator _enemyAnimator;

    protected virtual void Idle() { }

    protected virtual void Walk() { }
}