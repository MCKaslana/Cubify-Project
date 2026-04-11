using UnityEngine;

public class Discount : TimedEffect
{
    public float Multiplier { get; private set; }

    public Discount(Team team, int duration, float multiplier) 
        : base(team, duration)
    {
        Multiplier = multiplier;
    }
}
