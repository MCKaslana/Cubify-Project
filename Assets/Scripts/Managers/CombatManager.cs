using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : Singleton<CombatManager>
{
    protected override bool IsPersistent => false;

    [Header("Stamina")]
    [SerializeField] private int playerMaxStamina = 10;
    [SerializeField] private int aiMaxStamina = 10;

    private int _playerStamina;
    private int _opponentStamina;

    public override void Awake()
    {
        base.Awake();
        _playerStamina = playerMaxStamina;
        _opponentStamina = aiMaxStamina;
    }

    public bool HasEnoughStamina(Team team, int cost)
    {
        return team == Team.Player
        ? _playerStamina >= cost
        : _opponentStamina >= cost;
    }

    public void SpendStamina(Team team, int cost)
    {
        if (team == Team.Player)
            _playerStamina -= cost;
        else
            _opponentStamina -= cost;
    }

    public void RestoreStamina(int amount)
    {
        _playerStamina = Mathf.Min(_playerStamina + amount, playerMaxStamina);
        _opponentStamina = Mathf.Min(_opponentStamina + amount, aiMaxStamina);
    }

    public int GetPlayerStamina() => _playerStamina;
    public int GetAIStamina() => _opponentStamina;

    #region --- Ability Helpers ---

    private bool interruptRequested = false;
    private bool isResolving = false;

    public void ExecuteAbility(CubeControl user, CubeControl target, AbilityCard ability)
    {
        if (!ability.CanExecute(user, target))
        {
            Debug.Log("Ability cannot be used.");
            return;
        }

        StartCoroutine(ResolveAbility(user, target, ability));
    }

    private IEnumerator ResolveAbility(CubeControl user, CubeControl target, AbilityCard ability)
    {
        isResolving = true;
        interruptRequested = false;

        ability.OnExecute(user);

        yield return new WaitForSeconds(0.25f);

        if (interruptRequested)
        {
            Debug.Log("Ability Interrupted!");
            isResolving = false;
            yield break;
        }

        if (target != null && redirectMap.ContainsKey(target))
        {
            target = redirectMap[target];
            Debug.Log("Target Redirected!");
        }

        yield return ability.Execute(user, target);

        isResolving = false;
    }

    public bool IsResolving() => isResolving;

    #endregion

    #region --- Redirect System ---

    private Dictionary<CubeControl, CubeControl> redirectMap = new();

    public void SetRedirect(CubeControl originalTarget, CubeControl newTarget)
    {
        if (originalTarget == null || newTarget == null) return;

        redirectMap[originalTarget] = newTarget;
    }

    public void ClearRedirects()
    {
        redirectMap.Clear();
    }

    #endregion

    #region --- Interrupt System ---

    public void RequestInterrupt()
    {
        if (!isResolving) return;

        interruptRequested = true;
    }

    #endregion

    #region --- Temporary Effects ---

    public void ApplyTemporaryScale(CubeControl target, float scale, int turns)
    {
        if (target == null) return;

        target.Modify(scale, Color.yellow);
        StartCoroutine(RemoveScaleAfterTurns(target, scale, turns));
    }

    private IEnumerator RemoveScaleAfterTurns(CubeControl target, float scale, int turns)
    {
        yield return new WaitForSeconds(turns * 2f);

        if (target != null)
            target.Modify(1f / scale, Color.white);
    }

    #endregion
}