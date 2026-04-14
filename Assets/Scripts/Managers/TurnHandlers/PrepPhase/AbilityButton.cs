using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private AbilityCard _ability;
    private TextMeshProUGUI _abilityAmount;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _abilityAmount = GetComponentInChildren<TextMeshProUGUI>();
        _button.onClick.AddListener(OnClicked);
    }

    private void Update()
    {
        int amount = 
            PlayerAbilityInventory.Instance.HasAbility(_ability) ? PlayerAbilityInventory.Instance.GetCount(_ability) : 0;
        _abilityAmount.text = amount > 0 ? amount.ToString() : "< 0 >";
        _button.interactable = amount > 0;
    }

    private void OnClicked()
    {
        if (!PlayerAbilityInventory.Instance.HasAbility(_ability))
        {
            Debug.Log("No ability to use");
            return;
        }

        PlayerAbilityInventory.Instance.UseAbility(_ability);

        if (_ability is InterruptAbility)
        {
            var playerCubes = CubeSpawner.Instance.GetAllCubes();

            if (playerCubes.Count == 0) return;

            var user = playerCubes[0];

            if (!_ability.CanExecute(user, null)) return;

            StartCoroutine(_ability.Execute(user, null));

            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        if (_ability is RedirectAbility)
        {
            AbilityManager.Instance.StartRedirectAbility(_ability);
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        CubeCycleManager.Instance.StartAbilityFlow(_ability);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
