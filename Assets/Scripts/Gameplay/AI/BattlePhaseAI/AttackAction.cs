using UnityEngine;
using System.Collections;

public class AttackAction : IAIAttackAction
{
    private AbilityCard _attackAbility;

    public AttackAction(AbilityCard attackAbility)
    {
        _attackAbility = attackAbility;
    }

    public bool CanExecute()
    {
        return true; // refine later
    }

    public IEnumerator Execute()
    {
        var enemies = CubeSpawner.Instance.ReturnPlayerCubes();
        var myCubes = CubeSpawner.Instance.ReturnAICubes();

        if (myCubes.Count == 0 || enemies.Count == 0)
            yield break;

        var user = myCubes[Random.Range(0, myCubes.Count)];

        var validTargets = enemies.FindAll(c => c.GetLane() == user.GetLane());

        if (validTargets.Count == 0)
            yield break;

        var target = validTargets[Random.Range(0, validTargets.Count)];

        CombatManager.Instance.QueueAbility(user, target, _attackAbility);

        yield return new WaitForSeconds(0.2f);
    }
}
