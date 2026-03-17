using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Swap")]
public class SwapAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target)) return false;
        if (target == null) return false;

        return user.IsPlayerUnit() == target.IsPlayerUnit() || user.GetLane() == target.GetLane();
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null) yield break;

        Vector3 userPos = user.transform.position;
        Vector3 targetPos = target.transform.position;

        yield return user.MoveTo(targetPos);
        yield return target.MoveTo(userPos);

        user.SetLane(target.GetLane());
        target.SetLane(user.GetLane());

        //check if cube has swapped with an enemy cube
        //if so change team 

        Debug.Log($"{user.name} swapped with {target.name}");
    }
}
