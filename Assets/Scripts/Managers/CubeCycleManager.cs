using System.Collections.Generic;
using UnityEngine;

public class CubeCycleManager : Singleton<CubeCycleManager>
{
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
    }

    private void OnDisable()
    {
        InputManager.Instance.OnCycleLeft -= () => Cycle(-1);
        InputManager.Instance.OnCycleRight -= () => Cycle(1);
        InputManager.Instance.OnConfirmed -= Confirm;
        InputManager.Instance.OnGoBack -= GoBack;
    }

    public void StartAbilityFlow(AbilityCard ability)
    {
        _pendingAbility = ability;
        _confirmedUser = null;
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
        if (_candidates.Count == 0) return;
        ClearHover();
        _index = (_index + direction + _candidates.Count) % _candidates.Count;
        HoverCube(_candidates[_index]);
    }

    private void Confirm()
    {
        if (_hoveredCube == null) return;

        if (_phase == Phase.SelectingUser)
        {
            _confirmedUser = _hoveredCube;
            SelectionManager.Instance.ResetSelection();
            SelectionManager.Instance.SelectCube(_confirmedUser);

            if (IsSelfTargeting())
            {
                FireAbility(_confirmedUser, _confirmedUser);
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
        SelectionManager.Instance.SelectCube(target);

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

        SelectionManager.Instance.ResetSelection();
        Reset();
    }

    private bool IsSelfTargeting()
    {
        return _pendingAbility.GetTargetType() == TargetType.Self;
    }

    private void GoBack()
    {
        if (_phase == Phase.SelectingTarget)
        {
            SelectionManager.Instance.ResetSelection();
            _confirmedUser = null;
            EnterPhase(Phase.SelectingUser);
        }
        else if (_phase == Phase.SelectingUser)
        {
            Cancel();
        }
    }

    private void Cancel()
    {
        ClearHover();
        SelectionManager.Instance.ResetSelection();
        Reset();
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

    private void Reset()
    {
        _phase = Phase.None;
        _pendingAbility = null;
        _confirmedUser = null;
        _candidates.Clear();
        _index = 0;
    }
}
