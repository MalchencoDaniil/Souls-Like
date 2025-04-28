using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int _damage = 12;
    public float _attackRange = 3;
    public float _reloadTime = 1;

    public int _attackAnimCount = 1;

    public virtual void Equip(Transform hand) { }
    public virtual void Unequip() { }

    public virtual void Attack() { }
}