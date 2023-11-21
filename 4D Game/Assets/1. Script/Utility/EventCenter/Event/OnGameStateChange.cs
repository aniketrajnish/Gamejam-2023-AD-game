using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameStateChange : IEventWithData
{
    public GameState State;

    public OnGameStateChange(GameState state)
    {
        State = state;
    }
}
