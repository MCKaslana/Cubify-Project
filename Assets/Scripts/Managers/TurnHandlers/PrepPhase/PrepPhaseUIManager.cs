using System;
using UnityEngine;

public class PrepPhaseUIManager : Singleton<PrepPhaseUIManager>
{
    public event Action OnPlayerFinished;

    public void EnablePrepPhaseUI(bool enable)
    {
        UIManager.Instance.ShowPrepUI(enable);
    }

    public void NotifyAbilityUsed()
    {
        Debug.Log("Player used an ability");
    }

    public void EndPhase()
    {
        OnPlayerFinished?.Invoke();
    }
}
