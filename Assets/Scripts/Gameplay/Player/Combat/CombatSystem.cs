using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private Weapon _currentWeapon;

    public Weapon CurrentWeapon => _currentWeapon;

    public void SetWeapon(Weapon _weapon)
    {
        if (_currentWeapon != null)
            _currentWeapon.gameObject.SetActive(false);

        _currentWeapon = _weapon;
        _currentWeapon.gameObject.SetActive(true);
    }
}