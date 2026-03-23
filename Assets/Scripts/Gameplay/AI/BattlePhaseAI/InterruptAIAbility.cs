using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class InterruptAIAbility : IAIAttackAction
{
    private AbilityCard _interruptAbility;

    public InterruptAIAbility(AbilityCard ability)
    {
        _interruptAbility = ability;
    }

    public bool CanExecute()
    {
        return CombatManager.Instance.IsInReactionWindow; /*&&
            Random.value < 0.5f;*/
    }

    public IEnumerator Execute()
    {
        Debug.Log("EXECUTING AI INTERRUPT");

        var cubes = CubeSpawner.Instance.GetAllCubes();
        List<CubeControl> enemyCubes = new();

        if (cubes.Count == 0) yield break;

        foreach (var cube in cubes)
        {
            if (cube.GetTeam() == Team.Enemy)
                enemyCubes.Add(cube);
        }

        var user = enemyCubes[0];

        CombatManager.Instance.RequestInterrupt();
        CombatManager.Instance.SpendStamina(Team.Enemy, _interruptAbility.staminaCost);

        yield return _interruptAbility.Execute(user, null);
    }
}
