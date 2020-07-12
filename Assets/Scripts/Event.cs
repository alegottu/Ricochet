using UnityEngine.Events;

public class Event
{
    [System.Serializable] public class GameStateEvent : UnityEvent<GameManager.GameState, GameManager.GameState> { }
    [System.Serializable] public class FadeOutEvent : UnityEvent<bool> { }
}
