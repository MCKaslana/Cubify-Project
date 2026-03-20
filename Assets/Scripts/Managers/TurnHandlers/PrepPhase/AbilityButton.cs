using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private AbilityCard ability;
    [SerializeField] private Button button;

    private CubeControl _selectedCube;
    private CubeControl _targetCube;

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
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
        if (_selectedCube == null)
        {
            Debug.Log("No cube selected");
            return;
        }

        if (!ability.CanExecute(_selectedCube, _targetCube))
        {
            Debug.Log("Cannot use ability");
            return;
        }

        StartCoroutine(ability.Execute(_selectedCube, _targetCube));

        PrepPhaseUIManager.Instance.NotifyAbilityUsed();
    }
}
