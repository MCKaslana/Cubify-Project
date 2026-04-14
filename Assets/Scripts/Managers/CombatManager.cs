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
    private List<TimedEffect> _activeEffects = new();

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
    public bool IsPlayerTurn { get; set; } = true;
    public bool IsDominionActive { get; set; } = false;

    //Needed for redirect system
    public CubeControl CurrentIncomingTarget { get; private set; }

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
        var dominion = GetEffect<DominionPhase>(team);

        if (dominion != null)
        {
            bool finished = dominion.TryConsume();

            if (finished)
            {
                RemoveEffect(dominion);
                IsDominionActive = false;
            }

            Debug.Log("No Stamina/Points Used | dominion active");

            return;
        }

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

        CurrentIncomingTarget = target;

        ability.OnExecute(user);

        if (AllowReactions)
        {
            IsInReactionWindow = true;
            OnReactionWindowStart?.Invoke(target);

            float timer = 0f;
            bool aiHasReacted = false;

            while (timer < _reactionWindowDuration)
            {
                timer += Time.deltaTime;

                if (!aiHasReacted && IsPlayerTurn)
                {
                    TryAiReaction(target);
                    aiHasReacted = true;
                }

                yield return null;
            }

            IsInReactionWindow = false;
            OnReactionWindowEnd?.Invoke();

            if (interruptRequested)
            {
                StartCoroutine(ShowInterruption());

                isResolving = false;
                yield break;
            }

            if (target != null && redirectMap.ContainsKey(target))
            {
                target = redirectMap[target];
                StartCoroutine(ShowRedirect());
            }
        }

        yield return ability.Execute(user, target);

        ClearRedirects();

        CurrentIncomingTarget = null;

        isResolving = false;
    }

    public void TryAiReaction(CubeControl target)
    {
        if (target == null || target.GetTeam() != Team.Enemy)
            return;

        var aiCubes = CubeSpawner.Instance.GetAllSpawnedCubes(Team.Enemy);

        if (aiCubes.Count == 0)
            return;

        var user = aiCubes[UnityEngine.Random.Range(0, aiCubes.Count)];

        float roll = UnityEngine.Random.value;

        if (roll < 0.2f)
        {
            RequestInterrupt();
            user.PlaySound(2);
            return;
        }

        if (roll < 0.7f && aiCubes.Count > 1)
        {
            CubeControl newTarget;

            do
            {
                newTarget = aiCubes[UnityEngine.Random.Range(0, aiCubes.Count)];
            }
            while (newTarget == target);

            SetRedirect(target, newTarget);
            user.PlaySound(3);
        }
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

    #region --- Action Queue ---

    public void QueueAbility(CubeControl user, CubeControl target, AbilityCard ability)
    {
        if (IsProcessingQueue) return;

        if (!ability.CanExecute(user, target)) return;

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

    #region --- Temporary Effects ---

    public void AddTimedEffect(TimedEffect effect)
    {
        if (effect == null) return;

        _activeEffects.Add(effect);
        effect.ApplyEffect();
    }

    public void RemoveEffect(TimedEffect effect)
    {
        if (effect == null) return;

        effect.OnExpire();
        _activeEffects.Remove(effect);
    }

    public void TickEffects()
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            _activeEffects[i].Tick();
            if (_activeEffects[i].IsExpired())
            {
                RemoveEffect(_activeEffects[i]);
            }
        }
    }

    public T GetEffect<T>(Team team) where T : TimedEffect
    {
        foreach(var effect in _activeEffects)
        {
            if (effect is T typed && effect.Team == team)
                return typed;
        }

        return null;
    }

    #endregion
}