using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackAIController : MonoBehaviour
{
    private List<IAIAttackAction> _actions = new();

    public void Initialize(
        AbilityCard attack,
        AbilityCard swap,
        AbilityCard redirect,
        AbilityCard interrupt)
    {
        _actions.Clear();
    }

    public IEnumerator ExecuteTurn(int maxActions)
    {
        int actionsUsed = 0;

        while (actionsUsed < maxActions)
        {
            var action = _actions[Random.Range(0, _actions.Count)];

            if (!action.CanExecute())
                continue;

            yield return action.Execute();

            actionsUsed++;

            yield return new WaitForSeconds(0.3f);
        }
    }
}
