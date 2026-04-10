using System;
using UnityEngine;

public class TemporaryEffectHandler
{
    private int _remainingTurns = 3;
    private Action _onExpired;

    public TemporaryEffectHandler(int turns, Action onExpired)
    {
        _remainingTurns = turns;
        _onExpired = onExpired;
    }

    public void TickDown()
    {
        _remainingTurns--;

        if (_remainingTurns <= 0)
        {
            _onExpired?.Invoke();
        }
    }

    public bool IsExpired() => _remainingTurns <= 0;
}
