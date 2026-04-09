using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private GameObject _shopUI;

    public void BuyAbility(AbilityCard ability, int cost)
    {
        if (PointManager.Instance.GetScore() < cost)
        {
            Debug.Log("Not enough points");
            return;
        }

        PointManager.Instance.AddPoints(-cost);
        PlayerAbilityInventory.Instance.AddAbility(ability);

        Debug.Log($"Bought {ability.name}");
    }

    public void EnterShop()
    {
        _shopUI.SetActive(true);
    }

    public void ExitShop()
    {
        _shopUI.SetActive(false);
    }
}
