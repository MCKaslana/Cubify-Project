using UnityEngine;
using System.Collections;

public class InterruptAIAbility : IAIAttackAction
{
    private AbilityCard _interruptAbility;

    public InterruptAIAbility(AbilityCard ability)
    {
        _interruptAbility = ability;
    }

    public bool CanExecute()
    {
        if (CombatManager.Instance.GetAIStamina() < _interruptAbility.staminaCost)
            return false;
        return CombatManager.Instance.IsResolving();
    }

    public IEnumerator Execute()
    {
        CombatManager.Instance.RequestInterrupt();
        CombatManager.Instance.SpendStamina(Team.Enemy, _interruptAbility.staminaCost);

        Debug.Log("AI used Interrupt!");

        yield return null;
    }
}
