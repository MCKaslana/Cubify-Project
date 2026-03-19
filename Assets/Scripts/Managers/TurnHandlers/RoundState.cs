using UnityEngine;

public class RoundState : ITurnState
{
    private readonly TurnManager manager;

    public RoundState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Round Start");

        manager.ResetActions();

        CombatManager.Instance.RestoreStamina(5);

        manager.ChangeState(new AttackerState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}
