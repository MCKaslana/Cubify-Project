using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Swap")]
public class SwapAbility : AbilityCard
{
    [SerializeField] private bool _canSwapWithEnemy = false;

    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        if (!base.CanExecute(user, target)) return false;
        if (user == null || target == null) return false;
        if (user == target) return false;

        bool sameTeam = user.GetTeam() == target.GetTeam();

        if (!_canSwapWithEnemy && !sameTeam) return false;

        if (_canSwapWithEnemy && !sameTeam)
        {
            return user.GetLane() == target.GetLane();
        }

        return AreAdjacent(user, target);
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        if (target == null) yield break;

        user.PlaySound(5);
        yield return user.SwapWith(target);

        Debug.Log($"{user.name} swapped with {target.name}");
    }

    private bool AreAdjacent(CubeControl a, CubeControl b)
    {
        int laneA = (int)a.GetLane();
        int laneB = (int)b.GetLane();

        return Mathf.Abs(laneA - laneB) == 1;
    }
}