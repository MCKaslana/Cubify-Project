using UnityEngine;

public class AISwap : IAIPrepAction
{
    private AbilityCard _ability;

    public AISwap(AbilityCard ability)
    {
        _ability = ability;
    }

    public bool TryExecute()
    {
        var cubes = CubeSpawner.Instance.GetAllSpawnedCubes(Team.Enemy);

        if (cubes.Count < 2) return false;

        var cubeA = cubes[Random.Range(0, cubes.Count)];
        var cubeB = cubes[Random.Range(0, cubes.Count)];

        if (cubeA == cubeB) return false;

        if (!_ability.CanExecute(cubeA, cubeB))
            return false;

        CombatManager.Instance.ExecuteAbility(cubeA, cubeB, _ability);
        CombatManager.Instance.SpendStamina(Team.Enemy, _ability.staminaCost);

        return true;
    }
}
