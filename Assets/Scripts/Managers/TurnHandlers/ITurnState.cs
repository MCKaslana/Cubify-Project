using UnityEngine;

public enum TurnPhase
{
    Setup,
    Preparation,
    Battle,
    End
}

public interface ITurnState
{
    public TurnPhase Phase { get; }

    void Enter();
    void Execute();
    void Exit();
}
