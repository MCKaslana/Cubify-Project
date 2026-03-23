using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Redirect")]
public class RedirectAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        return CombatManager.Instance.IsInReactionWindow &&
               CombatManager.Instance.CurrentIncomingTarget != null &&
               target != null &&
               CombatManager.Instance.HasEnoughStamina(user.GetTeam(), staminaCost);
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        var original = CombatManager.Instance.CurrentIncomingTarget;

        if (original == null || target == null) yield break;

        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        user.PlaySound(2);
        CombatManager.Instance.SetRedirect(user, target);

        Debug.Log($"{user.name} redirected to {target.name}");

        yield return null;
    }
}
