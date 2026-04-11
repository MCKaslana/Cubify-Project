using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/DominionPhase")]
public class DominionPhaseAbility : AbilityCard
{
    public int freeUses = 2;

    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        return base.CanExecute(user, target);
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        CombatManager.Instance.AddTimedEffect(
            new DominionPhase(user.GetTeam(), freeUses)
        );

        yield return null;
    }
}
