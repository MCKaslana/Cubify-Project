using System.Collections;
using UnityEngine;

public class AttackerState : ITurnState
{
    private readonly TurnManager manager;

    private bool _isAIRunning = false;

    public AttackerState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Attacker Phase");

        UIManager.Instance.ShowAttackUI(true);

        if (manager.Attacker == TurnManager.Team.Player)
        {
            manager.StartCoroutine(manager.ShowRoleScreenIndicator(TurnManager.Team.Player));
        }
        else
        {
            manager.StartCoroutine(manager.ShowRoleScreenIndicator(TurnManager.Team.AI));
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
        if (_isAIRunning)
            return;

        _isAIRunning = true;
        manager.StartCoroutine(AITurnRoutine());

        Debug.Log("AI Does their turn here");

        manager.UseAttackerAction();
    }

    private IEnumerator AITurnRoutine()
    {
        yield return manager.StartCoroutine(
            manager.AttackAIController.ExecuteTurn(3));
    }

    public void Exit() { }
}
