using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackAction : IAIAttackAction
{
    private AbilityCard _attackAbility;

    public AttackAction(AbilityCard attackAbility)
    {
        _attackAbility = attackAbility;
    }

    public bool CanExecute()
    {
        if (CombatManager.Instance.IsInReactionWindow)
            return false;
        return true;
    }

    public IEnumerator Execute()
    {
        var allCubes = CubeSpawner.Instance.GetAllCubes();

        List<CubeControl> playerCubes = new();
        List<CubeControl> aiCubes = new();

        if (allCubes.Count == 0)
            yield break;

        foreach (var cube in allCubes)
        {
            if (cube.GetTeam() == Team.Player)
                playerCubes.Add(cube);
            if (cube.GetTeam() == Team.Enemy)
                aiCubes.Add(cube);
        }

        var user = aiCubes[Random.Range(0, playerCubes.Count)];

        var validTargets = playerCubes.FindAll(c => c.GetLane() == user.GetLane());

        if (validTargets.Count == 0)
            yield break;

        var target = validTargets[Random.Range(0, validTargets.Count)];

        CombatManager.Instance.QueueAbility(user, target, _attackAbility);

        yield return new WaitForSeconds(0.2f);
    }
}
