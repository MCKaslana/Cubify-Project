using UnityEngine;

public class CombatSimulationDebug : MonoBehaviour
{
    public CubeControl selectedCube;
    public CubeControl targetCube;
    public AttackAbility attackAbility;

    public void OnAttackButtonPressed()
    {
        if (TurnManager.Instance.GetAttacker() != TurnManager.Team.Player) return;

        CombatManager.Instance.ExecuteAbility(selectedCube, targetCube, attackAbility);
        TurnManager.Instance.UseAttackerAction();
    }
}
