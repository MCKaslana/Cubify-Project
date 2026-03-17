using UnityEngine;

public class AttackerState : ITurnState
{
    private TurnManager manager;

    public AttackerState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Attacker Phase");
    }

    public void Execute()
    {
        if (!manager.AttackerHasActions())
        {
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

        Debug.Log("AI takes action");

        manager.UseAttackerAction();
    }

    public void Exit() { }
}
