using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private CombatSystem _combatSystem;

    private void Start()
    {
        _combatSystem = FindObjectOfType<CombatSystem>();
    }

    public void Check()
    {
        _combatSystem.CheckHit();
    }
}