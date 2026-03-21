using UnityEngine;

public class QueuedAction
{
    public CubeControl User;
    public CubeControl Target;
    public AbilityCard Ability;

    public QueuedAction(CubeControl user, CubeControl target, AbilityCard ability)
    {
        User = user;
        Target = target;
        Ability = ability;
    }
}
