using UnityEngine;

public interface ITurnState
{
    void Enter();
    void Execute();
    void Exit();
}
