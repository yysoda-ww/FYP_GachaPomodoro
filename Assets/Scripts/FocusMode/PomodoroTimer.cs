using System;
using UnityEngine;

//controls the pomodoro timer functions like sents events every tick when work or break finishes

public class PomodoroTimer : MonoBehaviour
{
    public enum Phase { Idle, Work, Break, Paused }

    public Phase CurrentPhase { get; private set; } = Phase.Idle;

    public float RemainingSeconds { get; private set; } = 0f;
    public float TotalSeconds { get; private set; } = 0f;

    private Phase _phaseBeforePause = Phase.Idle;

    //does not control UI directly, sends events and UI responds

    public event Action<Phase> OnPhaseChanged;
    public event Action<float, float> OnTick;     
    public event Action OnWorkCompleted;
    public event Action OnBreakCompleted;

    private void Awake()
    {
        enabled = false;
    }

    public void StartWork(int workMinutes)
    {
        StartPhase(Phase.Work, Mathf.Clamp(workMinutes, 1, 180) * 60f);
    }

    public void StartBreak(int breakMinutes)
    {
        StartPhase(Phase.Break, Mathf.Clamp(breakMinutes, 1, 180) * 60f);
    }

    private void StartPhase(Phase phase, float seconds)
    {
        CurrentPhase = phase;
        TotalSeconds = Mathf.Max(1f, seconds);
        RemainingSeconds = TotalSeconds;

        enabled = true;

        OnPhaseChanged?.Invoke(CurrentPhase);
        OnTick?.Invoke(RemainingSeconds, TotalSeconds);
    }

    public void Pause()
    {
        if (CurrentPhase == Phase.Work || CurrentPhase == Phase.Break)
        {
            _phaseBeforePause = CurrentPhase;
            CurrentPhase = Phase.Paused;
            OnPhaseChanged?.Invoke(CurrentPhase);
        }
    }

    public void Resume()
    {
        if (CurrentPhase == Phase.Paused)
        {
            CurrentPhase = _phaseBeforePause;
            OnPhaseChanged?.Invoke(CurrentPhase);
        }
    }

    public void Stop()
    {
        enabled = false;

        CurrentPhase = Phase.Idle;
        RemainingSeconds = 0f;
        TotalSeconds = 0f;

        OnPhaseChanged?.Invoke(CurrentPhase);
        OnTick?.Invoke(RemainingSeconds, TotalSeconds);
    }

    private void Update()
    {
        if (CurrentPhase != Phase.Work && CurrentPhase != Phase.Break)
            return;

        RemainingSeconds -= Time.deltaTime;
        if (RemainingSeconds < 0f) RemainingSeconds = 0f;

        OnTick?.Invoke(RemainingSeconds, TotalSeconds);

        if (RemainingSeconds <= 0f)
        {
            enabled = false;

            if (CurrentPhase == Phase.Work)
                OnWorkCompleted?.Invoke();
            else if (CurrentPhase == Phase.Break)
                OnBreakCompleted?.Invoke();
        }
    }
}
