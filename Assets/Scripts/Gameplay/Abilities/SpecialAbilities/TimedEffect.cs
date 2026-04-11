using UnityEngine;

public abstract class TimedEffect
{
    public Team Team { get; private set; }
    public int Duration { get; private set; }

    public TimedEffect(Team team, int duration)
    {
        Team = team;
        Duration = duration;
    }

    public void Tick()
    {
        Duration--;
        if (Duration <= 0)
        {
            OnExpire();
        }
    }

    public bool IsExpired() => Duration <= 0;

    public virtual void ApplyEffect() { }
    public virtual void OnExpire() { }
}
