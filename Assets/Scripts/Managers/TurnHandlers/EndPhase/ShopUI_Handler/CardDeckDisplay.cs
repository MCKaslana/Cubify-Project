using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardDeckDisplay : Singleton<CardDeckDisplay>
{
    [SerializeField] private Image _deckImage;

    public void ShowCard(ShopItem item)
    {
        _deckImage.sprite = item.AbilityCard.Icon;

        _deckImage.DOFade(1, 0.3f);
        _deckImage.transform.DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack);
    }
}
