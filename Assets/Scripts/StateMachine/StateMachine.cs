using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private PlayerState _currentState;

    public StateMachine()
    {
        this._currentState = null;
    }

    public void Initialize(PlayerState startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
