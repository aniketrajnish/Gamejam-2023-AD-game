using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for reusing multiple timer.
/// </summary>
public class TimerManager : SimpleSingleton<TimerManager>
{
    [SerializeField] private SimpleObjectPool timerPool;
    
    public Timer GetTimer()
    {
        Timer timer = timerPool.GetPooledObject().GetComponent<Timer>();
        return timer;
    }
}
