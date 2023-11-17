using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Start,
    Play,
    Pause,
    End
}

public class GameManager : PersistantSingleton<GameManager>
{
    public GameState State { get; private set; }

    public void ChangeState(GameState state)
    {
        State = state;
        OnStateChange();
    }

    private void OnStateChange() 
    {         
        switch (State)
        {
            case GameState.None:
                break;
            case GameState.Start:
                break;
            case GameState.Play:
                break;
            case GameState.Pause:
                break;
            case GameState.End:
                break;
            default:
                break;
        }
    }
}
