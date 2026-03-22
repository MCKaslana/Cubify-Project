using UnityEngine;
using System.Collections;

public class RedirectAIAbility : IAIAttackAction
{
    private AbilityCard _redirectAbility;

    public RedirectAIAbility(AbilityCard ability)
    {
        _redirectAbility = ability;
    }

    public bool CanExecute()
    {
        return CombatManager.Instance.GetAIStamina() >= _redirectAbility.staminaCost;
    }

    public IEnumerator Execute()
    {
        var myCubes = CubeSpawner.Instance.ReturnAICubes();

        if (myCubes.Count < 2)
            yield break;

        var from = myCubes[Random.Range(0, myCubes.Count)];
        CubeControl to;

        do
        {
            to = myCubes[Random.Range(0, myCubes.Count)];
        }
        while (to == from);

        CombatManager.Instance.SetRedirect(from, to);

        Debug.Log($"AI redirects {from.name} -> {to.name}");

        yield return new WaitForSeconds(0.2f);
    }
}
