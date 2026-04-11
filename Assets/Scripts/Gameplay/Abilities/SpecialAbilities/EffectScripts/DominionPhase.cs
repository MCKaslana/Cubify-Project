using UnityEngine;

public class DominionPhase : TimedEffect
{
    private int _remainingUses;

    public DominionPhase(Team team, int uses)
        : base(team, 999)
    {
        _remainingUses = uses;
    }

    public bool TryConsume()
    {
        _remainingUses--;

        if (_remainingUses <= 0)
        {
            _remainingUses = 0;
            return true;
        }
        return false;
    }
}