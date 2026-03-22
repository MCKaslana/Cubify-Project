using UnityEngine;

public class AIModify : IAIPrepAction
{
    private AbilityCard _ability;

    public AIModify(AbilityCard ability)
    {
        _ability = ability;
    }

    public bool TryExecute()
    {
        var cubes = CubeSpawner.Instance.ReturnAICubes();

        if (cubes.Count == 0) return false;

        var user = cubes[Random.Range(0, cubes.Count)];
        var target = user;

        if (!_ability.CanExecute(user, target))
            return false;

        CombatManager.Instance.ExecuteAbility(user, target, _ability);

        Debug.Log($"AI used Modify on {user.name}");

        return true;
    }
}
