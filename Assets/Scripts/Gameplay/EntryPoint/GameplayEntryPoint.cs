using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    private CombatSystem _combatSystem;

    [SerializeField]
    private Weapon _startWeapon;

    private void Awake()
    {
        _combatSystem = FindObjectOfType<CombatSystem>();
        _combatSystem.SetWeapon(_startWeapon);
    }
}