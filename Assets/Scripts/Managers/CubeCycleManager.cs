using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeCycleManager : Singleton<CubeCycleManager>
{
    [Header("Visual Feedback Elements")]
    [SerializeField] private GameObject _selectionInformation;
    [SerializeField] private TextMeshProUGUI _pendingAbilityName;
    [SerializeField] private TextMeshProUGUI _staminaCostText;

    [SerializeField] private GameObject _userConfirmed;
    [SerializeField] private GameObject _targetConfirmed;

    private enum Phase { None, SelectingUser, SelectingTarget }

    private Phase _phase = Phase.None;
    private AbilityCard _pendingAbility;

    private List<CubeControl> _candidates = new List<CubeControl>();
    private int _index = 0;

    private CubeControl _hoveredCube;
    private CubeControl _confirmedUser;

    private void Start()
    {
        InputManager.Instance.OnCycleLeft += () => Cycle(-1);
        InputManager.Instance.OnCycleRight += () => Cycle(1);
        InputManager.Instance.OnConfirmed += Confirm;
        InputManager.Instance.OnGoBack += GoBack;

        InitializeInformation();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnCycleLeft -= () => Cycle(-1);
        InputManager.Instance.OnCycleRight -= () => Cycle(1);
        InputManager.Instance.OnConfirmed -= Confirm;
        InputManager.Instance.OnGoBack -= GoBack;

        if (_selectionInformation != null)
            _selectionInformation.SetActive(false);
    }

    private void InitializeInformation()
    {
        _selectionInformation.SetActive(false);
        _pendingAbilityName.text = "Ability In Use: None";
        _staminaCostText.text = "Stamina Cost: 0";
        _userConfirmed.SetActive(false);
        _targetConfirmed.SetActive(false);
    }

    public void StartAbilityFlow(AbilityCard ability)
    {
        _pendingAbility = ability;
        _pendingAbilityName.text = "Ability In Use: " + ability.abilityName;
        _staminaCostText.text = "Stamina Cost: " + ability.staminaCost;
        _selectionInformation.SetActive(true);
        Debug.Log($"Starting ability flow for {ability.abilityName}");
        _confirmedUser = null;
        EventSystem.current.SetSelectedGameObject(null);
        EnterPhase(Phase.SelectingUser);
    }

    private void EnterPhase(Phase phase)
    {
        ClearHover();
        _phase = phase;
        _candidates.Clear();
        _index = 0;

        if (phase == Phase.SelectingUser)
        {
            foreach (var cube in CubeSpawner.Instance.GetAllCubes())
                if (cube.GetTeam() == Team.Player && cube.IsAlive)
                    _candidates.Add(cube);
        }
        else if (phase == Phase.SelectingTarget)
        {
            foreach (var cube in CubeSpawner.Instance.GetAllCubes())
                if (cube.IsAlive && _pendingAbility.CanExecute(_confirmedUser, cube))
                    _candidates.Add(cube);
        }

        if (_candidates.Count == 0)
        {
            Debug.Log("No valid candidates for this phase.");
            Cancel();
            return;
        }

        HoverCube(_candidates[0]);
    }

    private void Cycle(int direction)
    {
        if (_phase == Phase.None || _candidates.Count == 0) return;
        ClearHover();
        _index = (_index + direction + _candidates.Count) % _candidates.Count;
        HoverCube(_candidates[_index]);
    }

    private void Confirm()
    {
        if (_phase == Phase.None || _hoveredCube == null) return;

        if (_phase == Phase.SelectingUser)
        {
            _confirmedUser = _hoveredCube;
            _userConfirmed.SetActive(true);

            if (IsSelfTargeting())
            {
                FireAbility(_confirmedUser, _confirmedUser);
                _targetConfirmed.SetActive(true);
                return;
            }

            EnterPhase(Phase.SelectingTarget);
        }
        else if (_phase == Phase.SelectingTarget)
        {
            FireAbility(_confirmedUser, _hoveredCube);
        }
    }

    private void FireAbility(CubeControl user, CubeControl target)
    {
        ClearHover();

        bool isAttackPhase = TurnManager.Instance.IsAttackPhase();
        if (isAttackPhase)
        {
            CombatManager.Instance.QueueAbility(user, target, _pendingAbility);
            TurnManager.Instance.UseAttackerAction();
        }
        else
        {
            CombatManager.Instance.ExecuteAbility(user, target, _pendingAbility);
            PrepPhaseUIManager.Instance.NotifyAbilityUsed();
        }

        _selectionInformation.SetActive(false);
        _userConfirmed.SetActive(false);
        _targetConfirmed.SetActive(false);
        ResetCubeSelection();
    }

    private bool IsSelfTargeting()
    {
        return _pendingAbility.GetTargetType() == TargetType.Self;
    }

    private void GoBack()
    {
        if (_phase == Phase.None) return;

        if (_phase == Phase.SelectingTarget)
        {
            _targetConfirmed.SetActive(false);
            _confirmedUser = null;
            EnterPhase(Phase.SelectingUser);
        }
        else if (_phase == Phase.SelectingUser)
        {
            _userConfirmed.SetActive(false);
            Cancel();
        }
    }

    private void Cancel()
    {
        ClearHover();
        ResetCubeSelection();
        Debug.Log("Ability selection cancelled.");
    }

    private void HoverCube(CubeControl cube)
    {
        _hoveredCube = cube;
        _hoveredCube.SetHoverHighlight(true);
    }

    private void ClearHover()
    {
        if (_hoveredCube != null)
        {
            _hoveredCube.SetHoverHighlight(false);
            _hoveredCube = null;
        }
    }

    private void ResetCubeSelection()
    {
        _phase = Phase.None;
        _selectionInformation.SetActive(false);
        _pendingAbility = null;
        _confirmedUser = null;
        _candidates.Clear();
        _index = 0;
    }
}
