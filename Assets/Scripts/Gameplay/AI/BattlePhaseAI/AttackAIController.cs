using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackAIController : MonoBehaviour
{
    [SerializeField] private AbilityCard _attackAbility;
    [SerializeField] private AbilityCard _swapAbility;
    [SerializeField] private AbilityCard _shrinkAbility;

    [SerializeField] private float _actionDelay = 2f;

    private List<IAIAttackAction> _actions = new();

    private void Awake()
    {
        Initialize
            (_attackAbility, _swapAbility, _shrinkAbility);
    }

    public void Initialize(
        AbilityCard attack,
        AbilityCard swap,
        AbilityCard shrink)
    {
        _actions.Clear();

        _actions.Add(new AttackAction(attack));
        _actions.Add(new SwapAIAbility(swap));
        _actions.Add(new ShrinkAIAbility(shrink));
    }

    public IEnumerator ExecuteTurn(int maxActions)
    {
        int actionsUsed = 0;

        while (actionsUsed < maxActions)
        {
            while (CombatManager.Instance.IsInReactionWindow || CombatManager.Instance.IsProcessingQueue)
                yield return null;

            var action = _actions[Random.Range(0, _actions.Count)];

            if (!action.CanExecute())
                continue;

            yield return action.Execute();
            TurnManager.Instance.UseAttackerAction();

            actionsUsed++;

            yield return new WaitForSeconds(_actionDelay);
        }
    }
}
