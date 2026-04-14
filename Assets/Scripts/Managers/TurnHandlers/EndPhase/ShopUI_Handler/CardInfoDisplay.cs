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
    

    public void ShowCard(ShopItem item)
    {
        _nameText.text = item.AbilityCard.name;
        _costText.text = "Cost: " + item.Cost;
        _descriptionImage.sprite = item.CardBack;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.3f);
    }
}