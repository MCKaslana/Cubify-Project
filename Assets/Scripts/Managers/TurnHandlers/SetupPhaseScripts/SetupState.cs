using UnityEngine;

public class SetupState : ITurnState
{
    private readonly TurnManager manager;
    public TurnPhase Phase => TurnPhase.Setup;

    public SetupState(TurnManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        CombatManager.Instance.AllowReactions = false;

        UIManager.Instance.ShowSetupUI(true);
        CubeSpawner.Instance.SpawnAICubes();

        CubePlacement.Instance.OnCubesPlaced += HandlePlacementComplete;
        CubePlacement.Instance.StartPlacement();
    }

    public void HandlePlacementComplete()
    {
        CubePlacement.Instance.ClearAllHighlights();
        CubePlacement.Instance.OnCubesPlaced -= HandlePlacementComplete;
        
        UIManager.Instance.ShowSetupUI(false);
        manager.RollDiceAndAssignRoles();

        manager.ChangeState(new RoundState(manager));
    }

    public void Exit() { }
    public void Execute() { }
}
