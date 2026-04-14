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

    public void ShowSpeicalDeck()
    {
        SetActiveDeck(_speciaDeck);
    }

    private void SetActiveDeck(GameObject activeDeck)
    {
        _prepDeck.SetActive(activeDeck);
        _battleDeck.SetActive(activeDeck);
        _speciaDeck.SetActive(activeDeck);

        activeDeck.SetActive(true);
    }
}
