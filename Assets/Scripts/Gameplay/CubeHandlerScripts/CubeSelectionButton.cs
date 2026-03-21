using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CubeSelectionButton : MonoBehaviour
{
    public CubeControl cube;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        Debug.Log("Selected Cube");
        SelectionManager.Instance.SelectCube(cube);
    }
}
