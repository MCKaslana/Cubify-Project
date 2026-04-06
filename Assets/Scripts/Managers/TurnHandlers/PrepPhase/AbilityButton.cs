using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private AbilityCard _ability;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
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
