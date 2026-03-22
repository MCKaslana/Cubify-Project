using UnityEngine;
using System.Collections;

public class RoundState : ITurnState
{
    private readonly TurnManager manager;

    private bool _isPlayerFinished = false;
    private bool _isAIFinished = false;

    public RoundState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Round Start");

        manager.ResetActions();
        StartPrepPhase();
    }

    private void StartPrepPhase()
    {
        UIManager.Instance.ShowPrepUI(true);
        PrepPhaseUIManager.Instance.OnPlayerFinished += HandlePlayerFinished;
    }

    private void HandlePlayerFinished()
    {
        PrepPhaseUIManager.Instance.OnPlayerFinished -= HandlePlayerFinished;
        UIManager.Instance.ShowPrepUI(false);

        _isPlayerFinished = true;

        HandleAITurn();
    }

    private void HandleAITurn()
    {
        manager.StartCoroutine(AITurnRoutine());
    }

    private IEnumerator AITurnRoutine()
    {
        yield return manager.StartCoroutine(manager.PrepAIController.ExecuteAITurn());

        _isAIFinished = true;

        EndPrepPhase();
    }

    private void EndPrepPhase()
    {
        if (_isPlayerFinished && _isAIFinished)
        {
            manager.ChangeState(new AttackerState(manager));
        }
    }

    public void Exit() { }
    public void Execute() { }
}
