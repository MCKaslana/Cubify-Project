using System.Collections;
using UnityEngine;

public class AttackAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target))
            return false;

        return target != null;
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null) yield break;

        yield return user.MoveTo(target.transform.position);

        Debug.Log("used attack ability");
        target.TakeDamage(10); // Example damage value

        yield return new WaitForSeconds(0.5f); // Short delay after attack
        yield return user.ReturnToOriginalPosition();
    }
}
