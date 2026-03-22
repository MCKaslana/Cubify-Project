using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Redirect")]
public class RedirectAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        return CombatManager.Instance.IsInReactionWindow &&
               target != null &&
               target.GetTeam() == user.GetTeam();
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        user.PlaySound(2);
        CombatManager.Instance.SetRedirect(user, target);

        Debug.Log($"{user.name} redirected to {target.name}");

        yield return null;
    }
}
