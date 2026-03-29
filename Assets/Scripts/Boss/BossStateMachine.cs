using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateMachine : MonoBehaviour
{
    public BossState CurrentState => _currentState;
    BossState _currentState;

    protected bool InTransition;

    public void ChangeState<T>() where T : BossState
    {
        T targetState = GetComponent<T>();

        if ( targetState == null) { return; }
        
        InitiateNewState(targetState);
    }       

    public void InitiateNewState(BossState targetState)
    {
        if(_currentState != targetState && !InTransition)
        {
            CallNewState(targetState);
        }
    }

    public void CallNewState(BossState newState)
    {
        InTransition = true;
        //
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
        //
        InTransition = false;
    }

    public void Update()
    {
        if(CurrentState != null && !InTransition)
        {
            CurrentState.Action();
        }
    }
}   

