using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackAIController : MonoBehaviour
{
    [SerializeField] private AbilityCard _attackAbility;
    [SerializeField] private AbilityCard _swapAbility;
    [SerializeField] private AbilityCard _redirectAbility;
    [SerializeField] private AbilityCard _interruptAbility;
    [SerializeField] private AbilityCard _shrinkAbility;

    private List<IAIAttackAction> _actions = new();

    private void Awake()
    {
        Initialize
            (_attackAbility, _swapAbility, _redirectAbility, _interruptAbility, _shrinkAbility);
    }

    public void Initialize(
        AbilityCard attack,
        AbilityCard swap,
        AbilityCard redirect,
        AbilityCard interrupt,
        AbilityCard shrink)
    {
        _actions.Clear();

        _actions.Add(new AttackAction(attack));

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
