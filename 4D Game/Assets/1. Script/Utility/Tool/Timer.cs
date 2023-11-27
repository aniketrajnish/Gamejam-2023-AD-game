using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple timer class.
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] private float duration = 0;
    [SerializeField] private float time = 0;

    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isFinished = true;

    private Coroutine timerRoutine;

    public float currentTime
    {
        get { return time; }
    }

    public bool IsFinished()
    {
        return isFinished;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void StartTimer(float duration)
    {
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);
        
        SetDuration(duration);
        isRunning = true;
        timerRoutine = StartCoroutine(TimerCoroutine());
    }

    private void SetDuration(float duration)
    {
        this.duration = duration;
        isFinished = false;
    }

    private IEnumerator TimerCoroutine()
    {
        //Debug.Log("Timer Start " + gameObject.name);
        time = 0;
        while (time <= duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isFinished = true;
        isRunning = false;
    }
}
