using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private Weapon _currentWeapon;

    public Weapon CurrentWeapon => _currentWeapon;

    [SerializeField]
    private LayerMask _enemyLayer;

    public void SetWeapon(Weapon _weapon)
    {
        if (_currentWeapon != null)
            _currentWeapon.gameObject.SetActive(false);

        _currentWeapon = _weapon;
        _currentWeapon.gameObject.SetActive(true);
    }

    public void CheckHit()
    {
        Collider[] _hitEnemys = Physics.OverlapSphere(_currentWeapon._attackPoint.position, _currentWeapon._attackRange, _enemyLayer);

        foreach (Collider _enemyCollider in _hitEnemys)
        {
            Enemy _enemy = _enemyCollider.gameObject.GetComponent<Enemy>();
            _enemy._healthSystem.TakeDamage(_currentWeapon._damage);

            //_weaponAnimator.CursorHit();

            ///_playerAudioSource.PlayOneShot(_smashClips[Random.Range(0, _smashClips.Count)]);
        }
    }
}