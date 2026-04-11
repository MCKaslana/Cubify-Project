using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private GameObject _shopUI;

    public void BuyAbility(AbilityCard ability, int baseCost)
    {
        int finalCost = GetFinalCost(Team.Player, baseCost);

        if (PointManager.Instance.GetScore() < finalCost)
        {
            Debug.Log("Not enough points");
            return;
        }

        PointManager.Instance.AddPoints(-finalCost);
        PlayerAbilityInventory.Instance.AddAbility(ability);

        Debug.Log($"Bought {ability.name} for {finalCost}");
    }

    public int GetFinalCost(Team team, int baseCost)
    {
        var discount = CombatManager.Instance.GetEffect<Discount>(team);

        if (discount != null)
        {
            return Mathf.CeilToInt(baseCost * discount.Multiplier);
        }

        return baseCost;
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
