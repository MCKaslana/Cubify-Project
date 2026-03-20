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

    private void OnEnable()
    {
        SelectionManager.Instance.OnSelectedCubeChanged += OnSelectedCubeChanged;
        SelectionManager.Instance.OnTargetCubeChanged += OnTargetCubeChanged;
    }

    private void OnDisable()
    {
        SelectionManager.Instance.OnSelectedCubeChanged -= OnSelectedCubeChanged;
        SelectionManager.Instance.OnTargetCubeChanged -= OnTargetCubeChanged;
    }

    private void OnSelectedCubeChanged(CubeControl cube)
    {
        // Called automatically by SelectionManager
        // Could add UI feedback here if needed
    }

    private void OnTargetCubeChanged(CubeControl cube)
    {
        // Called automatically by SelectionManager
        // Could add UI feedback here if needed
    }

    private void OnClicked()
    {
        var user = SelectionManager.Instance.SelectedCube;
        var target = SelectionManager.Instance.TargetCube;

        if (user == null || target == null)
        {
            Debug.Log("Select both a user and a target cube before using this ability.");
            return;
        }

        if (!_ability.CanExecute(user, target))
        {
            Debug.Log("Cannot use ability on this target.");
            return;
        }

        StartCoroutine(_ability.Execute(user, target));

        PrepPhaseUIManager.Instance?.NotifyAbilityUsed();
    }
}
