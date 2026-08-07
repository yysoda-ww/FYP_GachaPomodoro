using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Controls the UI of the Pomodoro timer, updates the display and user input

public class FocusUIController : MonoBehaviour
{
    [Header("Timer Reference")]
    [SerializeField] private PomodoroTimer timer;

    [Header("UI Labels")]
    [SerializeField] private TMP_Text modeLabel;
    [SerializeField] private TMP_Text timeLabel;

    [Header("Optional Status Text")]
    [SerializeField] private TMP_Text statusLabel;

    [Header("UI Progress")]
    [SerializeField] private Slider progressBar;

    [Header("UI Dropdowns")]
    [SerializeField] private TMP_Dropdown workDropdown;
    [SerializeField] private TMP_Dropdown breakDropdown;

    [Header("UI Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button stopButton;

    private int _lastSelectedWorkMinutes = 25;

    private void OnEnable()
    {
        if (timer == null) return;

        timer.OnPhaseChanged += HandlePhaseChanged;
        timer.OnTick += HandleTick;
        timer.OnWorkCompleted += HandleWorkCompleted;
        timer.OnBreakCompleted += HandleBreakCompleted;
    }

    private void OnDisable()
    {
        if (timer == null) return;

        timer.OnPhaseChanged -= HandlePhaseChanged;
        timer.OnTick -= HandleTick;
        timer.OnWorkCompleted -= HandleWorkCompleted;
        timer.OnBreakCompleted -= HandleBreakCompleted;
    }

    private void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(StartPressed);
        if (pauseButton != null) pauseButton.onClick.AddListener(() => timer.Pause());
        if (resumeButton != null) resumeButton.onClick.AddListener(() => timer.Resume());
        if (stopButton != null) stopButton.onClick.AddListener(() => timer.Stop());

        if (timer != null)
        {
            HandlePhaseChanged(timer.CurrentPhase);
            HandleTick(timer.RemainingSeconds, timer.TotalSeconds);
        }

        UpdateIdleStatusText();
    }

    private void StartPressed()
    {
        if (timer == null) return;

        _lastSelectedWorkMinutes = GetDropdownMinutes(workDropdown, 25);
        timer.StartWork(_lastSelectedWorkMinutes);

        if (statusLabel != null)
            statusLabel.text = $"Session started: {_lastSelectedWorkMinutes} min = {_lastSelectedWorkMinutes * 10} EXP";
    }

    private void HandleWorkCompleted()
    {
        int workMinutes = _lastSelectedWorkMinutes;
        int breakMinutes = GetDropdownMinutes(breakDropdown, 5);
        int expEarned = workMinutes * 10;

        if (PlayerLevelSystem.Instance != null)
        {
            PlayerLevelSystem.Instance.AddExp(expEarned);
            PlayerLevelSystem.Instance.AddStudyMinutes(workMinutes);
        }

        if (statusLabel != null)
            statusLabel.text = $"Completed {workMinutes} min study session. Earned {expEarned} EXP.";

        if (timer != null)
            timer.StartBreak(breakMinutes);
    }

    private void HandleBreakCompleted()
    {
        if (statusLabel != null)
            statusLabel.text = "Break completed";

        if (timer != null)
            timer.Stop();
    }

    private void HandlePhaseChanged(PomodoroTimer.Phase phase)
    {
        if (modeLabel != null)
        {
            modeLabel.text = phase switch
            {
                PomodoroTimer.Phase.Work => "Work",
                PomodoroTimer.Phase.Break => "Break",
                PomodoroTimer.Phase.Paused => "Paused",
                _ => "Idle"
            };
        }

        bool isRunning = phase == PomodoroTimer.Phase.Work || phase == PomodoroTimer.Phase.Break;
        bool isPaused = phase == PomodoroTimer.Phase.Paused;
        bool isIdle = phase == PomodoroTimer.Phase.Idle;

        if (startButton != null) startButton.interactable = isIdle;
        if (pauseButton != null) pauseButton.interactable = isRunning;
        if (resumeButton != null) resumeButton.interactable = isPaused;
        if (stopButton != null) stopButton.interactable = !isIdle;

        if (workDropdown != null) workDropdown.interactable = isIdle;
        if (breakDropdown != null) breakDropdown.interactable = isIdle;

        if (isIdle)
            UpdateIdleStatusText();
    }

    private void HandleTick(float remaining, float total)
    {
        if (timeLabel != null)
            timeLabel.text = FormatTime(remaining);

        if (progressBar != null)
        {
            float t = (total <= 0f) ? 0f : 1f - (remaining / total);
            progressBar.value = Mathf.Clamp01(t);
        }
    }

    private void UpdateIdleStatusText()
    {
        if (statusLabel == null) return;

        int minutes = GetDropdownMinutes(workDropdown, 25);
        int exp = minutes * 10;
        statusLabel.text = $"Ready. {minutes} min session = {exp} EXP";
    }

    private static int GetDropdownMinutes(TMP_Dropdown dropdown, int fallback)
    {
        if (dropdown == null || dropdown.options == null || dropdown.options.Count == 0)
            return fallback;

        string text = dropdown.options[dropdown.value].text.Trim();

        string digitsOnly = "";
        foreach (char c in text)
        {
            if (char.IsDigit(c))
                digitsOnly += c;
        }

        if (int.TryParse(digitsOnly, out int minutes))
            return Mathf.Clamp(minutes, 1, 180);

        return fallback;
    }

    private static string FormatTime(float seconds)
    {
        int s = Mathf.CeilToInt(seconds);
        int mins = s / 60;
        int secs = s % 60;
        return $"{mins:00}:{secs:00}";
    }
}