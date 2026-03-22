using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    protected override bool IsPersistent => false;

    [Header("Audio Refs")]
    private AudioSource _source;
    [SerializeField] private SoundData _skipActionSFX;

    [Header("AI Refs")]
    public PrepAIController PrepAIController { get; private set; }
    [SerializeField] private PrepAIController _prepAI;

    public AttackAIController AttackAIController { get; private set; }
    [SerializeField] private AttackAIController _attackAI;

    public enum Team { Player, AI }

    private ITurnState currentState;

    public Team Attacker { get; private set; }
    public Team Defender { get; private set; }

    public int AttackerActions { get; private set; }
    public int DefenderActions { get; private set; }

    private const int MAX_ACTIONS = 3;

    private bool _hasRolledForRoles = false;

    public bool IsAttackPhase()
    {
        return currentState is AttackerState;
    }

    public override void Awake()
    {
        base.Awake();
        PrepAIController = _prepAI;

        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChangeState(new SetupState(this));
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(ITurnState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    #region --- Round Setup ---

    public void RollDiceAndAssignRoles()
    {
        if (_hasRolledForRoles)
            return;

        int playerRoll = Random.Range(1, 7);
        int aiRoll = Random.Range(1, 7);

        if (playerRoll >= aiRoll)
        {
            Attacker = Team.Player;
            Defender = Team.AI;
        }
        else
        {
            Attacker = Team.AI;
            Defender = Team.Player;
        }

        _hasRolledForRoles = true;

        Debug.Log($"Attacker: {Attacker} | Defender: {Defender}");
    }

    public void ResetActions()
    {
        AttackerActions = MAX_ACTIONS;
        DefenderActions = MAX_ACTIONS;
    }

    #endregion

    #region --- Actions ---

    public void UseAttackerAction()
    {
        AttackerActions--;
    }

    public void UseDefenderAction()
    {
        DefenderActions--;
    }

    public void SkipAction()
    {
        AttackerActions--;
        CombatManager.Instance.RestorePlayerStamina(1);
        AudioManager.Instance.PlaySFX(_skipActionSFX, _source);
    }

    #endregion

    #region --- Flow Helpers ---

    public bool AttackerHasActions() => AttackerActions > 0;

    public void SwapRoles()
    {
        (Defender, Attacker) = (Attacker, Defender);
    }

    #endregion

    #region --- Getters ---

    public Team GetAttacker() => Attacker;
    public Team GetDefender() => Defender;
    public int GetAttackerActions() => AttackerActions;
    public int GetDefenderActions() => DefenderActions;

    #endregion
}
