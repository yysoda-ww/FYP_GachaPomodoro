using UnityEngine;

public class TaskListUI : MonoBehaviour
{
    [SerializeField] private TaskService taskService;
    [SerializeField] private Transform contentParent;
    [SerializeField] private TaskRowUI rowPrefab;

    private void Start()
    {
        Rebuild();
    }

    private void OnEnable()
    {
        if (taskService != null)
            taskService.OnTasksChanged += Rebuild;
    }

    private void OnDisable()
    {
        if (taskService != null)
            taskService.OnTasksChanged -= Rebuild;
    }

    public void Rebuild()
    {
        if (taskService == null || contentParent == null || rowPrefab == null)
            return;

        for (int i = contentParent.childCount - 1; i >= 0; i--)
            Destroy(contentParent.GetChild(i).gameObject);

        for (int i = 0; i < taskService.Tasks.Count; i++)
        {
            TaskRowUI row = Instantiate(rowPrefab, contentParent);
            row.Bind(taskService, taskService.Tasks[i], i);
        }
    }
}