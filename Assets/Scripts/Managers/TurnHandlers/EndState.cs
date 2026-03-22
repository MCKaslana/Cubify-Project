using UnityEngine;

public class EndState : ITurnState
{
    private readonly TurnManager manager;
    public TurnPhase Phase => TurnPhase.End;

    public EndState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        CombatManager.Instance.RestoreStamina(1);
        CombatManager.Instance.ClearRedirects();

        manager.SwapRoles();

        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}