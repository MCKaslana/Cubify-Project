using UnityEngine;

public class SetupState : ITurnState
{
    private readonly TurnManager manager;

    public SetupState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Setup State");

        CubeSpawner.Instance.SpawnAICubes();

        CubePlacement.Instance.OnCubesPlaced += HandlePlacementComplete;
        CubePlacement.Instance.StartPlacement();

        CubePlacement.Instance.StartPlacement();
    }

    public void HandlePlacementComplete()
    {
        Debug.Log("Player completed cube placement");
        CubePlacement.Instance.OnCubesPlaced -= HandlePlacementComplete;

        manager.RollDiceAndAssignRoles();

        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}
