using UnityEngine;
using UnityEngine.UI;

public class CardManagerUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _battleCardsUI;
    [SerializeField] private GameObject _specialCardsUI;

    [Header("Toggle Button")]
    [SerializeField] private Button _toggleButton;

    private bool _showingSpecialCards = false;

    private void Start()
    {
        _battleCardsUI.SetActive(true);
        _specialCardsUI.SetActive(false);

        _toggleButton.onClick.AddListener(ToggleCards);
    }

    private void ToggleCards()
    {
        _showingSpecialCards = !_showingSpecialCards;

        if (_showingSpecialCards )
        {
            _battleCardsUI.SetActive(false);
            _specialCardsUI.SetActive(true);
        }
        else
        {
            _battleCardsUI.SetActive(true);
            _specialCardsUI.SetActive(false);
        }
    }
}
