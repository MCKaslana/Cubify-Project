using UnityEngine;
using System.Collections;

public class ShrinkAIAbility : IAIAttackAction
{
    private AbilityCard _shrinkAbility;

    public ShrinkAIAbility(AbilityCard ability)
    {
        _shrinkAbility = ability;
    }

    public bool CanExecute()
    {
        if (CombatManager.Instance.IsInReactionWindow)
            return false;
        return CombatManager.Instance.GetAIStamina() >= _shrinkAbility.staminaCost;
    }

    public IEnumerator Execute()
    {
        var enemies = CubeSpawner.Instance.ReturnPlayerCubes();
        var myCubes = CubeSpawner.Instance.ReturnAICubes();

        if (enemies.Count == 0 || myCubes.Count == 0)
            yield break;

        var user = myCubes[Random.Range(0, myCubes.Count)];
        var target = enemies[Random.Range(0, enemies.Count)];

        CombatManager.Instance.QueueAbility(user, target, _shrinkAbility);
        CombatManager.Instance.SpendStamina(Team.Enemy, _shrinkAbility.staminaCost);

        yield return new WaitForSeconds(0.2f);
    }
}
