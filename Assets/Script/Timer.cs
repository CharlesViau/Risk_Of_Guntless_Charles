using UnityEngine;

public class Timer
{

    public float InitialTime { get; set; }
    public bool IsOver => RemainingTime <= 0f;
    public float RemainingTime { get; private set; }
    public bool HasStart { get; private set; } = false;
    public bool isPaused { get; private set; } = false;

    //Variable for Pause Function
    private float initialPauseTime = 0;
    private float PassedPauseTime = 0;
    private bool CanBeUnpaused => initialPauseTime - PassedPauseTime <= 0;
    private bool isUndefinedPause = true;

    public Timer(float timeInSeconds)
    {
        Set(timeInSeconds);
    }

    public void Set(float timeInSeconds)
    {
        InitialTime = timeInSeconds;
        RemainingTime = InitialTime;
    }

    public void Reset()
    {
        RemainingTime = InitialTime;
        HasStart = false;
        isPaused = false;
        initialPauseTime = 0;
        PassedPauseTime = 0;
        isUndefinedPause = true;
    }

    public void StartCount()
    {
        HasStart = true;
    }
    public void Pause()
    {
        isPaused = true;
    }
    public void Unpause()
    {
        isPaused = false;
    }

    public void PauseTimerFor(float timeInSeconds)
    {
        if (initialPauseTime < timeInSeconds)
        {
            initialPauseTime = timeInSeconds;
            PassedPauseTime = 0;
        }
        if (timeInSeconds > 0f && !isPaused)
        {
            initialPauseTime = timeInSeconds;
            this.Pause();
            isUndefinedPause = false;
        }
    }

    public void Update()
    {
        if (HasStart && !IsOver)
        {
            if (!isPaused) RemainingTime -= Time.deltaTime;
            else if (isPaused && !isUndefinedPause)
            {
                PassedPauseTime += Time.deltaTime;
                if (CanBeUnpaused)
                {
                    initialPauseTime = 0;
                    PassedPauseTime = 0;
                    isUndefinedPause = true;
                    this.Unpause();
                }
            }
        }
    }
}
