using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    private Gameplay.Player.CombatSystem _combatSystem;

    [SerializeField]
    private Weapon _startWeapon;

    private void Awake()
    {
        _combatSystem = FindObjectOfType<Gameplay.Player.CombatSystem>();
        //_combatSystem.SetWeapon(_startWeapon);
    }
}