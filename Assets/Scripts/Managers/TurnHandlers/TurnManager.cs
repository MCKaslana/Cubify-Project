using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    protected override bool IsPersistent => false;

    public enum Team { Player, AI }

    private ITurnState currentState;

    public Team Attacker { get; private set; }
    public Team Defender { get; private set; }

    public int AttackerActions { get; private set; }
    public int DefenderActions { get; private set; }

    private const int MAX_ACTIONS = 3;
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

    public bool CanDefenderReact(Team team)
    {
        return team == Defender && DefenderActions > 0;
    }

    public void SkipAction(Team team)
    {
        CombatManager.Instance.RestoreStamina(1);

        if (team == Attacker)
            AttackerActions--;
        else
            DefenderActions--;
    }

    #endregion

    #region --- Flow Helpers ---

    public bool AttackerHasActions() => AttackerActions > 0;

    public void SwapRoles()
    {
        (Attacker, Defender) = (Defender, Attacker);
    }

    #endregion
}
