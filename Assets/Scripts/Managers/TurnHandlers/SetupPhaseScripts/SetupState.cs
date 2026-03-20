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

        UIManager.Instance.ShowSetupUI(true);
        CubeSpawner.Instance.SpawnAICubes();

        CubePlacement.Instance.OnCubesPlaced += HandlePlacementComplete;
        CubePlacement.Instance.StartPlacement();

        CubePlacement.Instance.StartPlacement();
    }

    public void HandlePlacementComplete()
    {
        Debug.Log("Player completed cube placement");
        CubePlacement.Instance.ClearAllHighlights();
        CubePlacement.Instance.OnCubesPlaced -= HandlePlacementComplete;
        
        UIManager.Instance.ShowSetupUI(false);
        manager.RollDiceAndAssignRoles();

        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}
