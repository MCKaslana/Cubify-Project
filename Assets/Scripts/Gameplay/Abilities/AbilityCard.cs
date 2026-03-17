using UnityEngine;
using System.Collections;

public abstract class AbilityCard : ScriptableObject
{
    public string abilityName;
    public int staminaCost;

    public virtual bool CanExecute(CubeControl user, CubeControl target)
    {
        return CombatManager.Instance.HasEnoughStamina(user.GetTeam(), staminaCost);
    }

    public virtual void OnExecute(CubeControl user)
    {
        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);
    }

    public abstract IEnumerator Execute(CubeControl user, CubeControl target);
}
