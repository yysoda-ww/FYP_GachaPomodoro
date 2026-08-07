using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text numberLabel;
    [SerializeField] private TMP_Text titleLabel;
    [SerializeField] private Toggle completeToggle;
    [SerializeField] private Button deleteButton;

    private TaskService taskService;
    private TaskData taskData;

    public void Bind(TaskService service, TaskData task, int index)
    {
        taskService = service;
        taskData = task;

        if (numberLabel != null)
            numberLabel.text = (index + 1).ToString();

        if (titleLabel != null)
            titleLabel.text = task.title;

        if (completeToggle != null)
        {
            completeToggle.onValueChanged.RemoveAllListeners();
            completeToggle.SetIsOnWithoutNotify(task.isCompleted);
            completeToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(DeletePressed);
        }

        UpdateVisual();
    }

    private void OnToggleChanged(bool _)
    {
        if (taskService == null || taskData == null) return;
        taskService.ToggleCompleted(taskData.id);
        UpdateVisual();
    }

    private void DeletePressed()
    {
        if (taskService == null || taskData == null) return;
        taskService.DeleteTask(taskData.id);
    }

    private void UpdateVisual()
    {
        if (titleLabel == null || taskData == null) return;

        titleLabel.fontStyle = taskData.isCompleted
            ? FontStyles.Strikethrough
            : FontStyles.Normal;
    }
}