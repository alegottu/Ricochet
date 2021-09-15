using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static event Action<GameState, GameState> OnGameStateChange;

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
    }
    private GameState _currentState = GameState.PREGAME;
    public GameState currentState
    {
        get { return _currentState; }
        private set { currentState = value; }
    }

    public void UpdateState(GameState state)
    {
        GameState previousState = _currentState;
        _currentState = state;

        if (state == GameState.PAUSED)
        {
            Time.timeScale = 0; // Possibly have pause happen in a separate method for efficiency
        } 
        else
        {
            Time.timeScale = 1;
        }

        OnGameStateChange?.Invoke(previousState, _currentState);
    }

    public void TogglePause()
    {
        UpdateState(_currentState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }
}
