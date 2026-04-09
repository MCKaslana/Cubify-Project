using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityInventory : Singleton<PlayerAbilityInventory>
{
    private Dictionary<AbilityCard, int> _abilities = new();

    public void AddAbility(AbilityCard ability, int amount = 1)
    {
        if (_abilities.ContainsKey(ability))
            _abilities[ability] += amount;
        else
            _abilities.Add(ability, amount);
    }
    
    public bool HasAbility(AbilityCard ability)
    {
        return _abilities.ContainsKey(ability) && _abilities[ability] > 0;
    }

    public void UseAbility(AbilityCard ability)
    {
        if (!HasAbility(ability)) return;

        _abilities[ability]--;

        if (_abilities[ability] <= 0)
            _abilities.Remove(ability);
    }

    public int GetCount(AbilityCard ability)
    {
        return _abilities.TryGetValue(ability, out int count) ? count : 0;
    }
}
