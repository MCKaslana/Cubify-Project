using UnityEngine;

public class ShopDeckSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _prepDeck;
    [SerializeField] private GameObject _battleDeck;
    [SerializeField] private GameObject _speciaDeck;

    public void ShowPrepDeck()
    {
        SetActiveDeck(_prepDeck);
    }

    public void ShowBattleDeck()
    {
        SetActiveDeck(_battleDeck);
    }

    public void ShowSpecialDeck()
    {
        SetActiveDeck(_speciaDeck);
    }

    private void SetActiveDeck(GameObject activeDeck)
    {
        _prepDeck.SetActive(activeDeck == _prepDeck);
        _battleDeck.SetActive(activeDeck == _battleDeck);
        _speciaDeck.SetActive(activeDeck == _speciaDeck);
    }
}
