using NUnit.Framework.Interfaces;
using UnityEngine;

public class SetupState : ITurnState
{
    private TurnManager manager;

    public SetupState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Setup State");

        // TODO:
        // Spawn cubes (small, medium, big)

        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}
