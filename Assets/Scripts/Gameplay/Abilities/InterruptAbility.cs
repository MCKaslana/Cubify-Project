using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Interrupt")]
public class InterruptAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target)) return false;

        return CombatManager.Instance.IsResolving();
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        Debug.Log("Interrupt triggered!");

        CombatManager.Instance.RequestInterrupt();

        yield return null;
    }
}
