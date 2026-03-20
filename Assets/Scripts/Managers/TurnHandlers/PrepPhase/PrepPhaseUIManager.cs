using System;
using UnityEngine;

public class PrepPhaseUIManager : Singleton<PrepPhaseUIManager>
{
    public event Action OnPlayerFinished;

    public void EnablePrepPhaseUI(bool enable)
    {
        gameObject.SetActive(enable);

        //reset anything if needed
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
