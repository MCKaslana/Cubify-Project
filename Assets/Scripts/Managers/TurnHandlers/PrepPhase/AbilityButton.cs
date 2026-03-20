using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private AbilityCard _ability;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        var ability = _ability;
        var user = SelectionManager.Instance.CurrentUser;
        var target = SelectionManager.Instance.CurrentTarget;

        if (user == null || target == null)
        {
            Debug.Log("Select both a user and a target cube.");
            return;
        }

        if (!ability.CanExecute(user, target))
        {
            Debug.Log("Cannot use ability on this target.");
            return;
        }

        StartCoroutine(ability.Execute(user, target));

        PrepPhaseUIManager.Instance?.NotifyAbilityUsed();

        SelectionManager.Instance.ResetSelection();
    }
}
