using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGainScore : IEventWithData
{ 
    public int score;
    public OnGainScore(int score)
    {
        this.score = score;
    }
}
