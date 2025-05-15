using UnityEngine;

public class DB_IdleState : State
{
    private DragonBoss _enemy;

    private void Awake()
    {
        _enemy = GetComponent<DragonBoss>();
    }

    public override void UpdateState()
    {

    }
}