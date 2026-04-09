using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class UIManager : Singleton<UIManager>
{
    public event Action OnNextRoundActivated;

    [Header("Phase UIs")]
    [SerializeField] private GameObject _setupUI;
    [SerializeField] private GameObject _prepUI;
    [SerializeField] private GameObject _attackUI;
    [SerializeField] private GameObject _endUI;

    [Header("Phase transition indicatiors")]
    [SerializeField] private GameObject _setupIndicator;
    [SerializeField] private GameObject _prepIndicator;
    [SerializeField] private GameObject _battleIndicator;
    [SerializeField] private GameObject _endIndicator;

    [SerializeField] private TextMeshProUGUI _pointAmount;

    [Header("Reaction Window")]
    [SerializeField] private GameObject _reactionUI;

    private void OnEnable()
    {
        CombatManager.Instance.OnReactionWindowStart += ShowReactionUI;
        CombatManager.Instance.OnReactionWindowEnd += HideReactionUI;
        PointManager.Instance.OnPointsChanged += UpdatePointAmount;
    }

    private void OnDisable()
    {
        CombatManager.Instance.OnReactionWindowStart -= ShowReactionUI;
        CombatManager.Instance.OnReactionWindowEnd -= HideReactionUI;
        PointManager.Instance.OnPointsChanged -= UpdatePointAmount;
    }

    public void ActivateNextRound()
    {
        OnNextRoundActivated?.Invoke();
    }

    private void UpdatePointAmount(int amount)
    {
        _pointAmount.text = "Points: " + amount;
    }

    private void ShowReactionUI(CubeControl target)
    {
        target.Highlight();

        _reactionUI.SetActive(true);
    }

    private void HideReactionUI()
    {
        _reactionUI.SetActive(false);
    }

    public void ShowSetupUI(bool enable)
    {
        if (_setupUI != null)
        {
            _setupUI.SetActive(enable);
        }
        else
        {
            Debug.LogWarning("Setup UI GameObject is not assigned in the inspector.");
        }
    }

    public void ShowPrepUI(bool enable)
    {
        if (_prepUI != null)
        {
            _prepUI.SetActive(enable);
        }
        else
        {
            Debug.LogWarning("Prep UI GameObject is not assigned in the inspector.");
        }
    }

    public void ShowAttackUI(bool enable)
    {
        if (_attackUI != null)
        {
            _attackUI.SetActive(enable);
        }
        else
        {
            Debug.LogWarning("Attack UI GameObject is not assigned in the inspector.");
        }
    }

    public void ShowEndUI(bool enable)
    {
        if (_endUI != null)
        {
            _endUI.SetActive(enable);
        }
        else
        {
            Debug.LogWarning("End UI GameObject is not assigned in the inspector.");
        }
    }

    public void ShowCurrentPhaseScreenIndicator(TurnPhase phase)
    {
        GameObject indicatorToShow = null;
        switch (phase)
        {
            case TurnPhase.Setup:
                indicatorToShow = _setupIndicator;
                break;
            case TurnPhase.Preparation:
                indicatorToShow = _prepIndicator;
                break;
            case TurnPhase.Battle:
                indicatorToShow = _battleIndicator;
                break;
            case TurnPhase.End:
                indicatorToShow = _endIndicator;
                break;
        }

        if (indicatorToShow != null)
        {
            indicatorToShow.SetActive(true);
            StartCoroutine(HideIndicatorAfterDelay(indicatorToShow, 1.5f));
        }
    }

    private IEnumerator HideIndicatorAfterDelay(GameObject indicator, float delay)
    {
        yield return new WaitForSeconds(delay);
        indicator.SetActive(false);
    }
}
