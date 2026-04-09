using System;
using UnityEngine;

public class PointManager : Singleton<PointManager>
{
    public event Action<int> OnPointsChanged;

    [Header("Point Settings")]
    [SerializeField] private int _basePointsPerAction = 10;
    [SerializeField] private float _pointMultiplier = 1.0f;

    private int _points;
    public int GetScore() => _points;

    public override void Awake()
    {
        base.Awake();

        ResetPoints();
    }

    public void AddPoints(int amount)
    {
        _points += amount;

        OnPointsChanged?.Invoke(_points);
    }

    public void SpendPoints(int amount)
    {
        if (amount > _points)
        {
            Debug.LogWarning("Not enough points to spend!");
            return;
        }

        _points -= amount;
        OnPointsChanged?.Invoke(_points);
    }

    public void ResetPoints()
    {
        _points = 0;
        OnPointsChanged?.Invoke(_points);
    }

    public int CalculatePointGain(CubeControl attacker, CubeControl defender)
    {
        int sizeDifference = (int)attacker.GetCubeSize() - (int)defender.GetCubeSize();

        switch (sizeDifference)
        {
            case 1: return Mathf.RoundToInt(_basePointsPerAction * _pointMultiplier * 0.5f);
            case 0: return Mathf.RoundToInt(_basePointsPerAction * _pointMultiplier);
            case -1: return Mathf.RoundToInt(_basePointsPerAction * _pointMultiplier * 1.5f);
            default: return 10;
        }
    }
}
