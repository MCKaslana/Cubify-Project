using UnityEngine;

public class SuperBigCube : TimedEffect
{
    private CubeControl _target;

    public SuperBigCube(CubeControl target, int duration)
        : base(target.GetTeam(), duration)
    {
        _target = target;
    }

    public override void ApplyEffect()
    {
        if (_target != null)
            _target.TemporarySizeIncrease();
    }

    public override void OnExpire()
    {
        if (_target != null)
            _target.ResetTemporarySizeIncrease();
    }
}
