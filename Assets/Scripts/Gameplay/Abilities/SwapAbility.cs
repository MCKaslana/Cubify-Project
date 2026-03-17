using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Swap")]
public class SwapAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target)) return false;
        if (target == null) return false;

        return true;
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null) yield break;

        Vector3 userPos = user.transform.position;
        Vector3 targetPos = target.transform.position;

        user.PlaySound(5);
        yield return user.SwapWith(target);

        Debug.Log($"{user.name} swapped with {target.name}");
    }
}