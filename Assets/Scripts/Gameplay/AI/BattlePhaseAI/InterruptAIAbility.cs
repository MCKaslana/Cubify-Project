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
        return CombatManager.Instance.IsResolving();
    }

    public IEnumerator Execute()
    {
        CombatManager.Instance.RequestInterrupt();

        Debug.Log("AI used Interrupt!");

        yield return null;
    }
}
