using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceCubeButton : MonoBehaviour
{
    [SerializeField] private int _slotIndex;
    [SerializeField] private Button _button;
    private TextMeshProUGUI _buttonText;
    private Image _buttonImage;

    private bool _hasConfirmed = false;

    private void Awake()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
            _buttonText = _button.GetComponentInChildren<TextMeshProUGUI>();
            _buttonImage = _button.GetComponent<Image>();
        }

        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (CubePlacement.Instance == null)
        {
            Debug.LogError("CubePlacement instance not found!");
            return;
        }

        if (_hasConfirmed)
        {
            CubePlacement.Instance.PlaceCurrentCube(_slotIndex);
            _button.interactable = false;
        }

        CubePlacement.Instance.ChangeCubePreviewPosition(_slotIndex);
        _buttonText.text = "Confirm Placement?";
        _buttonImage.color = Color.red;

        _hasConfirmed = true;
    }
}
