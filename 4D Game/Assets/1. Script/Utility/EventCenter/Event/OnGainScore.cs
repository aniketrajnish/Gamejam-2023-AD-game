using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGainScore : IEventWithData
{ 
    public int Score;
    public OnGainScore(int score)
    {
        this.Score = score;
    }
}
