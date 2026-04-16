using UnityEngine;

public class RoundProgressionManager : Singleton<RoundProgressionManager>
{
    private int _currentRound = 1;

    [Header("Scaling Settings")]
    [SerializeField] private float _pointMultiplier = 1f;
    [SerializeField] private float _pointIncreaseMultiplier = 0.2f;

    private int _bonusActions = 0;
    [SerializeField] private int _roundsUntilBonusAction = 5;

    [SerializeField] private int _bonusActionIncrease = 1;
    [SerializeField] private int _maxBonusActions = 5;

    public int GetCurrentRound => _currentRound;

    public void OnNewRound()
    {
        _currentRound++;

        _pointMultiplier += _pointIncreaseMultiplier;

        if (_currentRound % _bonusActionIncrease == 0)
        {
            _bonusActions = Mathf.Min(_bonusActions + _bonusActionIncrease, _maxBonusActions);
        }
    }

    public int GetScaledPoints(int basePoints)
    {
        return Mathf.RoundToInt(basePoints * _pointMultiplier);
    }

    public int GetBonusActions()
    {
        return _bonusActions;
    }
}
