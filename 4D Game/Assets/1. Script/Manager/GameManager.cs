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

    private void Start()
    {
        ChangeState(GameState.Start);
        ChangeState(GameState.Play);
    }

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
                MapManager.Instance.Init();
                LevelManager.Instance.Init();
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
        EventCenter.PostEvent(new OnGameStateChange(State));
    }
}
