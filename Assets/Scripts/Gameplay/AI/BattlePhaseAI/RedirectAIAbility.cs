using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedirectAIAbility : IAIAttackAction
{
    private AbilityCard _redirectAbility;

    public RedirectAIAbility(AbilityCard ability)
    {
        _redirectAbility = ability;
    }

    public bool CanExecute()
    {
        return CombatManager.Instance.IsInReactionWindow &&
               CombatManager.Instance.CurrentIncomingTarget != null; /* &&
               Random.value < 0.5f;*/
    }

    public IEnumerator Execute()
    {
        Debug.Log("EXECUTING AI REDIRECT");

        var allCubes = CubeSpawner.Instance.GetAllCubes();
        List<CubeControl> myCubes = new();

        if (allCubes.Count < 2)
            yield break;

        foreach (var cube in allCubes)
        {
            if (cube.GetTeam() == Team.Enemy)
                myCubes.Add(cube);
        }

        var target = myCubes[Random.Range(0, myCubes.Count)];

        var user = myCubes[0];

        CombatManager.Instance.SpendStamina(Team.Enemy, _redirectAbility.staminaCost);

        yield return _redirectAbility.Execute(user, target);
    }
}
