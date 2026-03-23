using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PrepAIController : MonoBehaviour
{
    [SerializeField] private AbilityCard _swapAbility;
    [SerializeField] private AbilityCard _modifyAbility;

    private List<IAIPrepAction> _actions = new();

    private void Awake()
    {
        Initialize(_swapAbility, _modifyAbility);
    }

    public void Initialize(AbilityCard swap, AbilityCard modify)
    {
        _actions.Clear();

        _actions.Add(new AIModify(modify));
        _actions.Add(new AISwap(swap));
    }

    public IEnumerator ExecuteAITurn()
    {
        int attempts = 3;

        while (attempts > 0)
        {
            attempts--;

            var action = _actions[Random.Range(0, _actions.Count)];

            if (action.TryExecute())
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
