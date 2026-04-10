using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Super Big Cube")]
public class SuperBigCubeAbility : AbilityCard
{
    public int durationAmount = 3;

    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target))
            return false;
        return target != null;
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null) yield break;

        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        //play sound

        target.TemporarySizeIncrease();

        CombatManager.Instance.RegisterTemporaryEffect(
            new TemporaryEffectHandler(durationAmount, () =>
            {
                if (target != null)
                    target.ResetTemporarySizeIncrease();
            })
        );

        yield return null;
    }
}
