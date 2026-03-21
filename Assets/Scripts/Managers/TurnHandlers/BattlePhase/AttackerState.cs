using UnityEngine;

public class AttackerState : ITurnState
{
    private readonly TurnManager manager;

    public AttackerState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Attacker Phase");

        manager.SwapRoles();

        UIManager.Instance.ShowAttackUI(true);

        if (manager.Attacker == TurnManager.Team.Player)
        {
            Debug.Log("Player is Attacking");
        }
        else
        {
            Debug.Log("Enemy is Attacking");
        }
    }

    public void Execute()
    {
        if (CombatManager.Instance.IsResolving() || CombatManager.Instance.IsProcessingQueue)
            return;

        if (!manager.AttackerHasActions())
        {
            UIManager.Instance.ShowAttackUI(false);
            manager.ChangeState(new EndState(manager));
            return;
        }

        if (manager.Attacker == TurnManager.Team.Player)
        {
            // Wait for player input
        }
        else
        {
            RunAI();
        }
    }

    private void RunAI()
    {
        // TODO: AI LOGIC HERE

        Debug.Log("AI Does their turn here");

        manager.UseAttackerAction();
    }

    public void Exit() { }
}
