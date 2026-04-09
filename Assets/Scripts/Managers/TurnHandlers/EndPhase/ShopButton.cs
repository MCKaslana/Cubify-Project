using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private ShopItem _shopItem;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void Update()
    {
        _button.interactable = PointManager.Instance.GetScore() >= _shopItem.Cost;
    }

    private void OnButtonClicked()
    {
        ShopManager.Instance.BuyAbility(_shopItem.AbilityCard, _shopItem.Cost);
    }
}
