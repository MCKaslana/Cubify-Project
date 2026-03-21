using System.Collections.Generic;
using UnityEngine;

public class PrepAIController : MonoBehaviour
{
    private List<IAIPrepAction> _actions = new();

    public void Initialize(AbilityCard swap, AbilityCard modify)
    {
        _actions.Clear();

        _actions.Add(new AIModify(modify));
        _actions.Add(new AISwap(swap));
    }

    public void ExecuteTurn()
    {
        int attempts = 2;

        while (attempts > 0)
        {
            attempts--;

            var action = _actions[Random.Range(0, _actions.Count)];

            bool success = action.TryExecute();

            if (!success)
                continue;
        }
    }
}
