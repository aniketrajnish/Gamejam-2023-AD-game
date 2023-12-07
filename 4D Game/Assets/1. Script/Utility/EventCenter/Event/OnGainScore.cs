using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGainScore : IEventWithData
{ 
    public int Delta;
    public OnGainScore(int delta)
    {
        Delta = delta;
    }
}
