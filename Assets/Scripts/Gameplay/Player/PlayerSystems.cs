using UnityEngine;
using UnityEngine.Events;

public class PlayerSystems : MonoBehaviour
{
    private float _maxStamina;

    [SerializeField]
    private float _stamina = 100;

    public float CurrentStamina => _stamina;


    public UnityEvent OnTakeStamina;

    private void Start()
    {
        _maxStamina = _stamina;
    }

    public void AddStamina(float _amount)
    {
        _stamina += _amount;
        _stamina = CheckStamina();

        OnTakeStamina?.Invoke();
    }

    public void TakeStamina(float _damage)
    {
        _stamina -= _damage;
        _stamina = CheckStamina();

        OnTakeStamina?.Invoke();
    }

    private float CheckStamina()
    {
        if (_stamina <= 0)
            return 0;

        if (_stamina >= _maxStamina)
            return _maxStamina;

        return _stamina;
    }

    public void Death()
    {

    }
}