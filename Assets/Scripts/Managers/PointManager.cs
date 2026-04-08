using UnityEngine;

public class PointManager : Singleton<PointManager>
{
    [Header("Point Settings")]
    [SerializeField] private int _basePointsPerAction = 10;
    [SerializeField] private float _pointMultiplier = 1.0f;

    private int _points;

    public void AddPoints(int amount)
    {
        _points += amount;

        //Update UI
    }

    public void SpendPoints(int amount)
    {
        if (amount > _points)
        {
            Debug.LogWarning("Not enough points to spend!");
            return;
        }

        _points -= amount;
        //Update UI
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
