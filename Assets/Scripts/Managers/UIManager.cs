using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _setupUI;
    [SerializeField] private GameObject _prepUI;
    [SerializeField] private GameObject _attackUI;
    [SerializeField] private GameObject _endUI;

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
}
