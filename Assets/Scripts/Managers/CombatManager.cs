using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : Singleton<CombatManager>
{
    protected override bool IsPersistent => false;

    public bool IsInReactionWindow { get; private set; }

    public Action<CubeControl> OnReactionWindowStart;
    public Action OnReactionWindowEnd;

    [Header("Combat Settings")]
    [SerializeField] private float _actionDelay = 2f;
    [SerializeField] private float _reactionWindowDuration = 1.5f;

    [Header("Stamina")]
    [SerializeField] private int playerMaxStamina = 5;
    [SerializeField] private int aiMaxStamina = 5;
    [SerializeField] private StaminaBar _staminaBar;
    [SerializeField] private StaminaBar _enemyStaminaBar;

    [Header("Visuals")]
    [SerializeField] private GameObject _preventionIndicator;
    [SerializeField] private GameObject _redirectIndicator;

    private int _playerStamina;
    private int _opponentStamina;

    private Queue<QueuedAction> _actionQueue = new();
    private bool _isProcessingQueue = false;

    public bool IsProcessingQueue => _isProcessingQueue;
    public bool AllowReactions { get; set; } = false;

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

        UpdateStaminaBar();
    }

    public void RestoreStamina(int amount)
    {
        _playerStamina = Mathf.Min(_playerStamina + amount, playerMaxStamina);
        _opponentStamina = Mathf.Min(_opponentStamina + amount, aiMaxStamina);

        UpdateStaminaBar();
    }

    public void RestorePlayerStamina(int amount)
    {
        _playerStamina = Mathf.Min(_playerStamina + amount, playerMaxStamina);
        UpdateStaminaBar();
    }
    public void RestoreEnemyStamina(int amount)
    {
        _opponentStamina = Mathf.Min(_opponentStamina + amount, aiMaxStamina);
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        if (_staminaBar == null) return;

        _staminaBar.SetStamina(_playerStamina * 20);
        _enemyStaminaBar.SetStamina(_opponentStamina * 20);
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

        if (AllowReactions)
        {
            IsInReactionWindow = true;
            OnReactionWindowStart?.Invoke(target);

            float timer = 0f;

            while (timer < _reactionWindowDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            IsInReactionWindow = false;
            OnReactionWindowEnd?.Invoke();

            if (interruptRequested)
            {
                Debug.Log("Ability Interrupted!");

                StartCoroutine(ShowInterruption());

                isResolving = false;
                yield break;
            }

            if (target != null && redirectMap.ContainsKey(target))
            {
                target = redirectMap[target];
                StartCoroutine(ShowRedirect());
                Debug.Log("Target Redirected!");
            }
        }

        yield return ability.Execute(user, target);

        ClearRedirects();

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

    private IEnumerator ShowRedirect()
    {
        if (_redirectIndicator != null)
        {
            _redirectIndicator.SetActive(true);
            yield return new WaitForSeconds(1f);
            _redirectIndicator.SetActive(false);
        }
    }

    public void ResetReactionState()
    {
        interruptRequested = false;
        IsInReactionWindow = false;
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

    private IEnumerator ShowInterruption()
    {
        if (_preventionIndicator != null)
        {
            _preventionIndicator.SetActive(true);
            yield return new WaitForSeconds(1f);
            _preventionIndicator.SetActive(false);
        }
    }

    #endregion

        #region --- Temporary Effects ---

    public void ApplyTemporaryScale(CubeControl target, float scale, int turns)
    {
        if (target == null) return;

        target.IncreaseSize();
        StartCoroutine(RemoveScaleAfterTurns(target, scale, turns));
    }

    private IEnumerator RemoveScaleAfterTurns(CubeControl target, float scale, int turns)
    {
        yield return new WaitForSeconds(turns * 2f);

        if (target != null)
            target.DecreaseSize();
    }

    #endregion

    #region --- Action Queue ---

    public void QueueAbility(CubeControl user, CubeControl target, AbilityCard ability)
    {
        if (IsProcessingQueue) return;

        if (!ability.CanExecute(user, target))
        {
            Debug.Log("Ability cannot be used.");
            return;
        }

        _actionQueue.Enqueue(new QueuedAction(user, target, ability));

        if (!_isProcessingQueue)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        _isProcessingQueue = true;

        while (_actionQueue.Count > 0)
        {
            var action = _actionQueue.Dequeue();

            yield return StartCoroutine(
                ResolveAbility(action.User, action.Target, action.Ability)
            );

            yield return new WaitForSeconds(_actionDelay);
        }

        _isProcessingQueue = false;
    }

    #endregion
}