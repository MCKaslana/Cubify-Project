using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Swap")]
public class SwapAbility : AbilityCard
{
    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null || 
            !user.IsSelectable ||
            !target.IsSelectable) yield break;

        user.PlaySound(5);
        yield return user.SwapWith(target);
    }
}