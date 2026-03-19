using UnityEngine;
using UnityEngine.UI;

public class PlaceCubeButton : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private Button button;

    private bool _isOccupied = false;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (CubePlacement.Instance == null)
        {
            Debug.LogError("CubePlacement instance not found!");
            return;
        }

        if (_isOccupied)
        {
            Debug.Log("Slot already occupied");
            return;
        }

        CubePlacement.Instance.PlaceCurrentCube(slotIndex);

        _isOccupied = true;
        button.interactable = false;
    }
}
