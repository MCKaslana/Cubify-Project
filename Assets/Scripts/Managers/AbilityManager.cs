using UnityEngine;

public class AbilityManager : Singleton<AbilityManager>
{
    private AbilityCard _pendingRedirectAbility;

    public void StartRedirectAbility(AbilityCard ability)
    {
        if (!CombatManager.Instance.IsInReactionWindow)
        {
            return;
        }

        _pendingRedirectAbility = ability;
    }

    public AbilityCard GetPendingRedirect() => _pendingRedirectAbility;

    public void ClearRedirectMode()
    {
        _pendingRedirectAbility = null;
    }
}
