using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Discount")]
public class DiscountAbility : AbilityCard
{
    public int durationInRounds = 999;
    public float multiplier = 0.5f;

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        CombatManager.Instance.AddTimedEffect(
            new Discount(user.GetTeam(), durationInRounds, multiplier)
        );

        yield return null;
    }
}
