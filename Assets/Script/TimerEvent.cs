using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    [SerializeField] private float Count;
    [SerializeField] private bool IsReset;
    private bool isActive = false;
    public UnityEvent OnBeforeTimerStart;
    public UnityEvent OnTimerStart;
    public UnityEvent DuringTimerCount;
    public UnityEvent OnTimerPause;
    public UnityEvent DuringTimerPause;
    public UnityEvent OnTimerUnpause;
    public UnityEvent OnTimerOver;
    public UnityEvent OnReset;
    public UnityEvent OnEnd;

    public Timer timer;

    public EventState eventState { get; private set; }

    public enum EventState
    {
        BEFORE_TIMER_START,
        ON_TIMER_START,
        DURING_TIMER_COUNT,
        ON_TIMER_PAUSE,
        DURING_TIMER_PAUSE,
        ON_TIMER_UNPAUSE,
        ON_TIMER_OVER,
        ON_RESET,
        END,
    }

    private void Start()
    {
        timer = new Timer(Count);
        OnTimerStart.AddListener(timer.StartCount);
        OnReset.AddListener(timer.Reset);
    }
    private void Update()
    {
        if (isActive)
        {
            timer.Update();

            switch (eventState)
            {
                case EventState.BEFORE_TIMER_START:
                    OnBeforeTimerStart?.Invoke();
                    eventState = EventState.ON_TIMER_START;
                    break;
                case EventState.ON_TIMER_START:
                    OnTimerStart?.Invoke();
                    eventState = EventState.DURING_TIMER_COUNT;
                    break;
                case EventState.DURING_TIMER_COUNT:
                    DuringTimerCount?.Invoke();
                    if (timer.IsOver) eventState = EventState.ON_TIMER_OVER;
                    else if (timer.isPaused) eventState = EventState.ON_TIMER_PAUSE;
                    break;
                case EventState.ON_TIMER_PAUSE:
                    OnTimerPause?.Invoke();
                    eventState = EventState.DURING_TIMER_PAUSE;
                    break;
                case EventState.DURING_TIMER_PAUSE:
                    DuringTimerPause?.Invoke();
                    if (!timer.isPaused) eventState = EventState.ON_TIMER_UNPAUSE;
                    break;
                case EventState.ON_TIMER_UNPAUSE:
                    OnTimerUnpause?.Invoke();
                    eventState = EventState.DURING_TIMER_COUNT;
                    break;
                case EventState.ON_TIMER_OVER:
                    OnTimerOver?.Invoke();
                    if (IsReset) eventState = EventState.ON_RESET;
                    else eventState = EventState.END;
                    break;
                case EventState.END:
                    if(!IsInvoking()) OnEnd?.Invoke();
                    break;
                case EventState.ON_RESET:
                    OnReset?.Invoke();
                    eventState = EventState.BEFORE_TIMER_START;
                    break;
                default:
                    break;
            }
        }
    }

    public void StartEvent()
    {
        isActive = true;
    }

    public void PauseEvent()
    {
        isActive = false;
    }

}

