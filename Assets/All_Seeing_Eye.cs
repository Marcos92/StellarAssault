using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Seeing_Eye : MonoBehaviour
{
    public float turnTime;
    public float movementTime;
    public float actionsTime;

    public enum GameState
    {
        Turn,
        Movement,
        Actions
    }

    public GameState GetState() { return _state; }

    private int turnNumber = 0;
    private GameState _state { get; set; }
    private float startTime;

    public delegate void TurnTickCallbackDelegate(float turnPercentage);
    public delegate void TurnEndCallbackDelegate(int turnNumber);

    public delegate void MovementCallbackDelegate(float step);
    public delegate void MovementEndCallbackDelegate();

    public delegate void ActionCallbackDelegate();
    public delegate void ActionEndCallbackDelegate();

    private List<TurnTickCallbackDelegate> _turnTickCallbacks = new List<TurnTickCallbackDelegate>();
    private List<TurnEndCallbackDelegate> _turnEndCallbacks = new List<TurnEndCallbackDelegate>();

    private List<MovementCallbackDelegate> _movementCallbacks = new List<MovementCallbackDelegate>();
    private List<MovementEndCallbackDelegate> _movementEndCallbacks = new List<MovementEndCallbackDelegate>();

    private List<ActionCallbackDelegate> _actionCallbacks = new List<ActionCallbackDelegate>();
    private List<ActionEndCallbackDelegate> _actionEndCallbacks = new List<ActionEndCallbackDelegate>();

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;

        switch(_state) {
            case GameState.Turn:
                if ((elapsedTime - turnTime) >= 0)
                {
                    ProceedToMovementState();
                }

                float turnPerc = elapsedTime / turnTime;

                if (turnPerc >= 1.0f) 
                {
                    turnPerc = 1.0f; 
                }
                TickTurn(turnPerc);
                break;
            case GameState.Movement:
                if ((elapsedTime - movementTime) >= 0)
                {
                    ProceedToActionsState();
                }
                else
                {
                    MoveElements(elapsedTime/movementTime);
                }
                break;
            case GameState.Actions:
                if ((elapsedTime - actionsTime) >= 0)
                {
                    ProceedToTurnState();
                }
                else
                {
                    ActElements();
                }
                break;
        }
    }

    #region Callbacks

    public void RegisterTurnTickCallback(TurnTickCallbackDelegate callback)
    {
        _turnTickCallbacks.Add(callback);
    }

    public void RegisterTurnEndCallback(TurnEndCallbackDelegate callback)
    {
        _turnEndCallbacks.Add(callback);
    }

    public void RegisterMovementCallback(MovementCallbackDelegate callback)
    {
        _movementCallbacks.Add(callback);

    }

    public void RegisterMovementEndCallback(MovementEndCallbackDelegate callback)
    {
        _movementEndCallbacks.Add(callback);
    }

    public void RegisterActionCallback(ActionCallbackDelegate callback)
    {
        _actionCallbacks.Add(callback);
    }

    public void RegisterActionEndCallback(ActionEndCallbackDelegate callback)
    {
        _actionEndCallbacks.Add(callback);
    }

    #endregion

    #region State Controls

    private void ProceedToMovementState()
    {
        GoToState(GameState.Movement);

        foreach (TurnEndCallbackDelegate callback in _turnEndCallbacks)
        {
            callback(turnNumber++);
        }
    }

    private void ProceedToActionsState()
    {
        GoToState(GameState.Actions);

        foreach(MovementEndCallbackDelegate callback in _movementEndCallbacks)
        {
            callback();
        }
    }

    private void ProceedToTurnState()
    {
        GoToState(GameState.Turn);

        foreach(ActionEndCallbackDelegate callback in _actionEndCallbacks)
        {
            callback();
        }
    }

    private void GoToState(GameState state)
    {
        startTime = Time.time;
        _state = state;
    }

    #endregion

    private void TickTurn(float v)
    {
        foreach(TurnTickCallbackDelegate callback in _turnTickCallbacks)
        {
            callback(v);
        }
    }

    private void MoveElements(float step)
    {
        foreach (MovementCallbackDelegate callback in _movementCallbacks)
        {
            callback(step);
        }
    }

    private void ActElements()
    {
        foreach (ActionCallbackDelegate callback in _actionCallbacks)
        {
            callback();
        }
    }
}
