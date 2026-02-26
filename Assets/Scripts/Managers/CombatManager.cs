using UnityEngine;
using System.Collections;
using System;

public class CombatManager : Singleton<CombatManager>
{
    protected override bool IsPersistent => false;

    [SerializeField] private Transform[] _combatPositions;
    private GameObject[] _combatants = new GameObject[2];
    private int _joinedCombatants = 0;

    public event Action OnCombatEnded;

    public void JoinCombat(GameObject combatant)
    {
        Debug.Log("Combatant joined combat: " + combatant.name);

        if (_joinedCombatants < 2)
        {
            _combatants[_joinedCombatants] = combatant;
            _joinedCombatants++;

            if (_joinedCombatants == 2)
            {
                InitateCombat(_combatants[0], _combatants[1]);
            }
        }
    }

    public void LeaveCombat(GameObject combatant)
    {
        Debug.Log("Combatant left combat: " + combatant.name);
        for (int i = 0; i < _combatants.Length; i++)
        {
            if (_combatants[i] == combatant)
            {
                _combatants[i] = null;
                _joinedCombatants--;
                break;
            }
        }
    }

    public void InitateCombat(GameObject attacker, GameObject defender)
    {
        Debug.Log("Combat initiated between: " + attacker.name + " and " + defender.name);
        _combatants[0] = attacker;
        _combatants[1] = defender;

        StartCoroutine(MoveCombatants(attacker.transform, defender.transform.position));
    }

    private IEnumerator MoveCombatants(Transform unit, Vector3 target)
    {
        while (Vector3.Distance(unit.position, target) > 0.1f)
        {
            unit.position = Vector3.MoveTowards(unit.position, target, 5f * Time.deltaTime);
            yield return null;
        }

        unit.position = target;
        OnCombatEnded?.Invoke();
    }
}
