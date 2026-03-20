using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private AbilityCard _ability;
    [SerializeField] private Button _button;

    private CubeControl _selectedCube;
    private CubeControl _targetCube;

    private void Awake()
    {
        _button.GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    private void OnEnable()
    {
        SelectionManager.Instance.OnSelectedCubeChanged += SetUser;
        SelectionManager.Instance.OnTargetCubeChanged += SetTarget;
    }

    private void OnDisable()
    {
        SelectionManager.Instance.OnSelectedCubeChanged -= SetUser;
        SelectionManager.Instance.OnTargetCubeChanged -= SetTarget;
    }

    public void SetUser(CubeControl cube)
    {
        _selectedCube = cube;
    }

    public void SetTarget(CubeControl target)
    {
        _targetCube = target;
    }

    private void OnClicked()
    {
        if (_selectedCube == null || _targetCube == null)
        {
            Debug.Log("Select both a user and a target cube");
            return;
        }

        if (!_ability.CanExecute(_selectedCube, _targetCube))
        {
            Debug.Log("Cannot use ability");
            return;
        }

        StartCoroutine(_ability.Execute(_selectedCube, _targetCube));

        PrepPhaseUIManager.Instance.NotifyAbilityUsed();
    }
}
