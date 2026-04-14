using UnityEngine;

public class ShopDeckSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _prepDeck;
    [SerializeField] private GameObject _battleDeck;


    public void ShowPrepDeck()
    {
        SetActiveDeck(_prepDeck);
    }

    public void ShowBattleDeck()
    {
        SetActiveDeck(_battleDeck);
    }


    private void SetActiveDeck(GameObject activeDeck)
    {
        _prepDeck.SetActive(false);
        _battleDeck.SetActive(false);

        activeDeck.SetActive(true);
    }
}
