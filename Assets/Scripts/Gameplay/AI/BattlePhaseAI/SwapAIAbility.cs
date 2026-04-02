using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwapAIAbility : IAIAttackAction
{
    private AbilityCard _swapAbility;

    public SwapAIAbility(AbilityCard ability)
    {
        _swapAbility = ability;
    }

    public bool CanExecute()
    {
        if (CombatManager.Instance.IsInReactionWindow)
            return false;
        return CombatManager.Instance.GetAIStamina() >= _swapAbility.staminaCost;
    }

    public IEnumerator Execute()
    {
        var allCubes = CubeSpawner.Instance.GetAllCubes();

        if (allCubes.Count < 2)
            yield break;

        var user = allCubes[Random.Range(0, allCubes.Count)];
        CubeControl target;

        do
        {
            target = allCubes[Random.Range(0, allCubes.Count)];
        }
        while (target == user);

        CombatManager.Instance.QueueAbility(user, target, _swapAbility);
        CombatManager.Instance.SpendStamina(Team.Enemy, _swapAbility.staminaCost);

        yield return new WaitForSeconds(0.2f);
    }
}
