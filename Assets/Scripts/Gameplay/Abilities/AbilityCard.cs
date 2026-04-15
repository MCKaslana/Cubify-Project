using UnityEngine;
using System.Collections;

public enum TargetType
{
    Self,
    Ally,
    Enemy,
    Any
}

public enum TargetingShape
{
    None,
    Adjacent,
    SameLane,
    Any
}

public abstract class AbilityCard : ScriptableObject
{
    public string abilityName;
    public int staminaCost;

    [Header("Targeting")]
    [SerializeField] private TargetType targetType = TargetType.Enemy;
    public TargetType GetTargetType() => targetType;
    [SerializeField] private TargetingShape targetingShape = TargetingShape.Any;

    public virtual bool CanExecute(CubeControl user, CubeControl target)
    {
        if (user == null) return false;

        if (!CombatManager.Instance.HasEnoughStamina(user.GetTeam(), staminaCost)) 
            return false;

        if (!ValidateTarget(user, target))
            return false;

        return true;
    }

    protected bool ValidateTarget(CubeControl user, CubeControl target)
    {
        if (target == null) return false;

        bool sameTeam = user.GetTeam() == target.GetTeam();

        switch (targetType)
        {
            case TargetType.Self:
                if (target != user) return false;
                break;

            case TargetType.Ally:
                if (!sameTeam || target == user) return false;
                break;

            case TargetType.Enemy:
                if (sameTeam) return false;
                break;

            case TargetType.Any:
                break;
        }

        switch (targetingShape)
        {
            case TargetingShape.Adjacent:
                if (!AreAdjacent(user, target)) return false;
                break;

            case TargetingShape.SameLane:
                if (user.GetLane() != target.GetLane()) return false;
                break;

            case TargetingShape.Any:
                break;
        }

        return true;
    }

    public virtual void OnExecute(CubeControl user)
    {
        CombatManager.Instance.SpendStamina(user.GetTeam(), staminaCost);
    }

    protected bool AreAdjacent(CubeControl a, CubeControl b)
    {
        int laneA = (int)a.GetLane();
        int laneB = (int)b.GetLane();

        return Mathf.Abs(laneA - laneB) == 1;
    }

    public abstract IEnumerator Execute(CubeControl user, CubeControl target);
}
