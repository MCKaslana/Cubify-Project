using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Interrupt")]
public class InterruptAbility : AbilityCard
{
    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        return CombatManager.Instance.IsInReactionWindow &&
               CombatManager.Instance.HasEnoughStamina(user.GetTeam(), staminaCost);
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        Debug.Log("Interrupt triggered!");

        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);
        
        user.PlaySound(1);

        CombatManager.Instance.RequestInterrupt();

        yield return null;
    }
}
