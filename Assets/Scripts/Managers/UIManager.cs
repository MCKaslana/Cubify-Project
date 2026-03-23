using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    [Header("Phase UIs")]
    [SerializeField] private GameObject _setupUI;
    [SerializeField] private GameObject _prepUI;
    [SerializeField] private GameObject _attackUI;

    [Header("Phase transition indicatiors")]
    [SerializeField] private GameObject _setupIndicator;
    [SerializeField] private GameObject _prepIndicator;
    [SerializeField] private GameObject _battleIndicator;
    [SerializeField] private GameObject _endIndicator;

    [Header("Reaction Window")]
    [SerializeField] private GameObject _reactionUI;

    private void OnEnable()
    {
        CombatManager.Instance.OnReactionWindowStart += ShowReactionUI;
        CombatManager.Instance.OnReactionWindowEnd += HideReactionUI;
    }

    private void OnDisable()
    {
        CombatManager.Instance.OnReactionWindowStart -= ShowReactionUI;
        CombatManager.Instance.OnReactionWindowEnd -= HideReactionUI;
    }

    private void ShowReactionUI(CubeControl target)
    {
        Debug.Log("REACTION WINDOW OPEN");

        target.Highlight();

        _reactionUI.SetActive(true);
    }

    private void HideReactionUI()
    {
        Debug.Log("REACTION WINDOW CLOSED");

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
