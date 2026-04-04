using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Redirect")]
public class RedirectAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        var original = CombatManager.Instance.CurrentIncomingTarget;

        return CombatManager.Instance.IsInReactionWindow &&
               original != null &&
               target != null &&
               target != original &&
               CombatManager.Instance.HasEnoughStamina(user.GetTeam(), staminaCost);
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        var original = CombatManager.Instance.CurrentIncomingTarget;

        if (original == null || target == null) yield break;

        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        user.PlaySound(2);
        CombatManager.Instance.SetRedirect(original, target);

        yield return null;
    }
}
