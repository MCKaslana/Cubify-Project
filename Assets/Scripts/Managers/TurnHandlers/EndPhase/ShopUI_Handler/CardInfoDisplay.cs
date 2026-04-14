using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;

public class CardInfoDisplay : Singleton<CardInfoDisplay>
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _descriptionImage;

    [Header("Animation")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _panel;

    public void ShowCard(ShopItem item)
    {
        _nameText.text = item.AbilityCard.name;
        _costText.text = "Cost: " + item.Cost;
        _descriptionImage.sprite = item.AbilityCard.Icon;

        _canvasGroup.alpha = 0;
        _panel.anchoredPosition += new Vector2(50f, 0);

        _canvasGroup.DOFade(1, 0.3f);
        _panel.DOAnchorPosX(_panel.anchoredPosition.x - 50f, 0.3f)
            .SetEase(Ease.OutCubic);
    }

    public void ShowCardSprite(ShopItem item)
    {
        Image deckImage = item.CardBack;
        deckImage.sprite = item.AbilityCard.Icon;

        deckImage.DOFade(1, 0.3f);
        deckImage.transform.DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack);
    }
}