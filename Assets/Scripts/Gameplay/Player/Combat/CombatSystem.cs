using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class CombatSystem : MonoBehaviour
    {
        //private float _currentAnimId = 0;

        //private Weapon _currentWeapon;
        //private float _timeToReload;

        //[Inject]
        //private PlayerInput _playerInput;

        //[SerializeField]
        //private PlayerAnimator _playerAnimator;

        //public void SetWeapon(Weapon _weapon)
        //{
        //    if (_currentWeapon != null)
        //        _currentWeapon.gameObject.SetActive(false);

        //    _currentWeapon = _weapon;
        //    _currentWeapon.gameObject.SetActive(true);
        //}

        //private void Update()
        //{
        //    _timeToReload -= Time.deltaTime;

        //    if (_playerInput.AttackInput() && _timeToReload <= 0f)
        //        Attack();
        //}

        //private void Attack()
        //{
        //    _currentWeapon.Attack();
        //    _playerAnimator.AttackAnim(CheckCurrentAnimId(_currentAnimId));

        //    _timeToReload = _currentWeapon._reloadTime;

        //    _currentAnimId++;
        //}

        //private float CheckCurrentAnimId(float _id)
        //{
        //    if (_currentAnimId > _currentWeapon._attackAnimCount)
        //        return 0;

        //    if (_currentAnimId <= 0)
        //        return 0;

        //    return _currentAnimId;
        //}
    }
}