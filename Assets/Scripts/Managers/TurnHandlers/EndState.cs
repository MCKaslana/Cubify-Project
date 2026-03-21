using UnityEngine;

public class EndState : ITurnState
{
    private readonly TurnManager manager;

    public EndState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("End Phase");

        CombatManager.Instance.ClearRedirects();

        
        manager.SwapRoles();

        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}