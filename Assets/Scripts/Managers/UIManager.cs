using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _setupUI;
    [SerializeField] private GameObject _prepUI;
    [SerializeField] private GameObject _attackUI;

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
}
