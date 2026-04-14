using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/ShopItem")]
public class ShopItem : ScriptableObject
{
    public AbilityCard AbilityCard;
    public Sprite CardBack;
    public int Cost;
}
