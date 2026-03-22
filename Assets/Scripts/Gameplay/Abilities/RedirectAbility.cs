using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Redirect")]
public class RedirectAbility : AbilityCard
{
    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        var originalTarget = SelectionManager.Instance.CurrentTarget;

        if (originalTarget == null || target == null)
            yield break;

        user.PlaySound(2);
        CombatManager.Instance.SetRedirect(originalTarget, target);

        Debug.Log($"{originalTarget.name} redirected to {target.name}");
    }
}
