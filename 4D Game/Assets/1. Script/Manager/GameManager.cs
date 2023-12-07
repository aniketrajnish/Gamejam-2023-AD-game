using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Start,
    Play,
    Pause,
    End,
    Win
}

public class GameManager : PersistantSingleton<GameManager>
{
    public GameState State { get; private set; }

    [SerializeField] private List<int> winTotalCoins = new List<int>();

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

    public void SetWinTotalCoins(int coins, int level)
    {
        if (level <= 0)
            return;

        if (winTotalCoins.Count < level)
            winTotalCoins.Add(coins);
        else
            winTotalCoins[level-1] = coins;
    }

    public int GetWinTotalCoins(int level)
    {
        if (level <= winTotalCoins.Count && level > 0)
            return winTotalCoins[level-1];

        return -1;
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
                //AudioManager.Instance.PlayMusic("BGM");
                break;
            case GameState.Play:
                LevelManager.Instance.SpawnEnemies();
                break;
            case GameState.Pause:
                break;
            case GameState.End:
                break;
            case GameState.Win:
                break;
            default:
                break;
        }
        EventCenter.PostEvent(new OnGameStateChange(State));
    }
}
