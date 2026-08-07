using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddTaskUI : MonoBehaviour
{
    [SerializeField] private TaskService taskService;
    [SerializeField] private TMP_InputField taskInput;
    [SerializeField] private Button addButton;

    private void Start()
    {
        if (addButton != null)
            addButton.onClick.AddListener(AddPressed);

        if (taskInput != null)
            taskInput.onSubmit.AddListener(OnSubmitTask);
    }

    public void AddPressed()
    {
        if (taskService == null || taskInput == null)
            return;

        string text = taskInput.text.Trim();

        if (string.IsNullOrEmpty(text))
            return;

        taskService.AddTask(text);
        taskInput.text = "";
        taskInput.ActivateInputField();
    }

    private void OnSubmitTask(string _)
    {
        AddPressed();
    }
}