using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Abilities/Attack")]
public class AttackAbility : AbilityCard
{
    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null) yield break;

        yield return user.MoveTo(target.transform.position);

        user.PlaySound(0);
        Debug.Log("used attack ability");
        target.TakeDamage(DamageCalculator.CalculateDamage(user, target));

        yield return new WaitForSeconds(0.5f); // Short delay after attack
        yield return user.ReturnToOriginalPosition();
    }
}
