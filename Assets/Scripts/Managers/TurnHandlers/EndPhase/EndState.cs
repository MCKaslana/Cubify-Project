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
        CombatManager.Instance.AllowReactions = false;

        CombatManager.Instance.RestoreStamina(1);
        CombatManager.Instance.ClearRedirects();

        UIManager.Instance.ShowEndUI(true);

        manager.SwapRoles();

        UIManager.Instance.OnNextRoundActivated += ContinueToNextRound;
    }

    public void ContinueToNextRound()
    {
        UIManager.Instance.OnNextRoundActivated -= ContinueToNextRound;
        UIManager.Instance.ShowEndUI(false);
        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}