using UnityEngine;
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
        var ability = _ability;
        var user = SelectionManager.Instance.CurrentUser;
        var target = SelectionManager.Instance.CurrentTarget;

        if (ability is InterruptAbility)
        {
            var playerCubes = CubeSpawner.Instance.GetAllCubes();

            if (playerCubes.Count == 0) return;

            user = playerCubes[0];

            if (!ability.CanExecute(user, null))
            {
                Debug.Log("Cannot use Interrupt");
                return;
            }

            StartCoroutine(ability.Execute(user, null));

            SelectionManager.Instance.ResetSelection();
            return;
        }

        if (ability is RedirectAbility)
        {
            AbilityManager.Instance.StartRedirectAbility(ability);
        }

        if (user == null || target == null)
        {
            Debug.Log("Select both a user and a target cube.");
            return;
        }

        if (!ability.CanExecute(user, target))
        {
            Debug.Log("Cannot use ability on this target.");
            return;
        }

        bool isAttackingPhase = TurnManager.Instance.IsAttackPhase();

        if (isAttackingPhase)
        {
            CombatManager.Instance.QueueAbility(user, target, ability);
            TurnManager.Instance.UseAttackerAction();
        }
        else
        {
            CombatManager.Instance.ExecuteAbility(user, target, ability);
            PrepPhaseUIManager.Instance.NotifyAbilityUsed();
        }

        SelectionManager.Instance.ResetSelection();
    }
}
