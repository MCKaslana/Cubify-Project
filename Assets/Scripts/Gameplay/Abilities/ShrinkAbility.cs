using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Shrink")]
public class ShrinkAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target))
            return false;

        if (target == null)
            return false;

        return target.GetCubeSize() != CubeSize.Small;
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null)
            yield break;

        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);

        user.PlaySound(3);

        yield return new WaitForSeconds(0.15f);

        target.DecreaseSize();

        Debug.Log($"{user.name} shrunk {target.name}");

        yield return null;
    }
}
