using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : Singleton<ShopManager>
{
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
}
